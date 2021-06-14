using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;

public class RoundManager : NetworkBehaviour
{
    double sceneReloadWaitTime = 0;
    double sceneWaitTime;
    double waitUntilStart = 3;
    double timeWaitToStart = 15.0f; //Segundos de espera hasta que se conecte el segundo cliente

    const float roundLengthInSeconds = 45;

    [SyncVar]
    int currentRound = 0;

    float timeLeft;

    public Text timeTxt;
    public Image[] points;

    bool waitingForReload = false;

    //[SyncVar]
    public bool bothConnected = false;
    private bool gameStarted = false;

    private bool resultsSent = false;
    private float timeBeforeStart = 0.0f;
    private bool exit = false;

    public Countdown countdown;
    static public RoundManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            // guardamos en la instancia el objeto creado
            // debemos guardar el componente ya que _instancia es del tipo GameManager
            instance = this;

            GameManager.instance.roundManager = this;
        }
        else
        {
            waitingForReload = false;
        }
    }

    private void Start()
    {
        countdown.gameObject.SetActive(true);
        TimeStart();

        //if (isServer)
        timeBeforeStart = Time.realtimeSinceStartup;

        GameManager.instance.isServer = isServer;

        if (isClient)
        {
            currentRound = GameManager.instance.currentRound;

            timeTxt.text = ((int)roundLengthInSeconds).ToString("D2");

            List<RoundResult> results = GameManager.instance.results;

            for (int i = 0; i < results.Count && i < points.Length; i++)
            {
                RoundResult r = results[i];

                Color col = Color.white;

                switch (r.result)
                {
                    case 1.0f:
                        col = Color.green;
                        break;

                    case 0.5f:
                        col = Color.yellow;
                        break;

                    case 0.0f:
                        col = Color.red;
                        break;

                    default:
                        break;
                }

                points[i].color = col;
            }
        }
        Time.timeScale = 0;
    }

    //for testing
    private void Update()
    {
        if (bothConnected && isClient)
        {
            if (waitUntilStart > Time.realtimeSinceStartup - timeBeforeStart)
            {
                countdown.updateCountdown(Time.realtimeSinceStartup - timeBeforeStart);

                return;
            }
            else if (countdown.gameObject.activeSelf)
            {
                countdown.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
        }

        if (isServer)
        {
            if (NetworkManager.singleton.numPlayers == 1)
            {
                if(Time.realtimeSinceStartup - timeBeforeStart > timeWaitToStart)
                {
                    // Si solo un jugador se conecta, y despues de timeWaitToStart, la partida termina sin resultado
                    Finish();
                    Application.Quit();
                }

                if (bothConnected)
                {
                    RpcWinDisconnect();
                    SendResults();
                    Finish();
                    CloseServer();
                    Application.Quit();
                }
            }
            if (NetworkManager.singleton.numPlayers >= 2 && !bothConnected)
            {
                RpcExitQueue();
                bothConnected = true;
            }

            if (!waitingForReload)
                TimeUpdate();
            else
            {
                sceneWaitTime -= Time.deltaTime;

                if(sceneWaitTime <= 0)
                {
                    GameManager.instance.currentRound++;
                    Debug.Log("Numero de rondas: " + GameManager.instance.currentRound);

                    if (GameManager.instance.currentRound < GameManager.instance.totalRounds)
                        SceneReload();
                    else
                    {
                        SendResults();
                        Finish();
                        Application.Quit();
                        //SceneReload();
                    }
                }
            }
        }

        if (isClient)
        {
            int truncatedTime = (int)roundLengthInSeconds;

            if (timeLeft != roundLengthInSeconds)
                truncatedTime = Mathf.RoundToInt((float)timeLeft);

            timeTxt.text = truncatedTime.ToString("D2");
        }
    }

    //[ClientRpc]
    public void TriggerRoundEnd(bool localPlayer)
    {
        RequestSyncTime();

        Debug.Log("Did I win? " + !localPlayer);

        if (localPlayer)
            RoundEnd(0, timeLeft);
        else
            RoundEnd(1, timeLeft);

        RequestSceneReload();
    }

    [ClientRpc]
    private void RpcWinDisconnect()
    {
        WinDisconnect();
    }

    [Client]
    private void WinDisconnect()
    {
        int r = GameManager.instance.results.Count;
        for (int i = r; i < GameManager.instance.totalRounds; i++)
        {
            GameManager.instance.results.Add(new RoundResult(1, 0.0f));
        }
    }

    
    private void LoseDisconnect()
    {
        int r = GameManager.instance.results.Count;
        for (int i = 0; i < r; i++)
        {
            GameManager.instance.results[i] = new RoundResult(0, 0.0f);
        }
        for (int i = r; i < GameManager.instance.totalRounds; i++)
        {
            GameManager.instance.results.Add(new RoundResult(0, 0.0f));
        }
    }

    [Client]
    public void TriggerDraw()
    {
        Debug.Log("Draw!");

        //TimeStart();

        RoundEnd(0.5f, 0.0f);
    }

    [Client]
    private void RoundEnd(float result, float time)
    {
        Debug.Log("Round ended!");

        GameManager.instance.results.Add(new RoundResult(result, 1.0f - (time / roundLengthInSeconds)));

        //Debug.Log("round: " + GameManager.instance.currentRound);
    }

    [Server]
    public void ServerRoundEnd(bool localPlayer)
    {
        //TriggerRoundEnd(timeLeft, localPlayer);

        TriggerSceneReload();
    }

    [Command]
    private void CmdDraw()
    {
        RpcDraw();
    }

    [ClientRpc]
    private void RpcDraw()
    {
        TriggerDraw();
    }
    
    private void TimeStart()
    {
        timeLeft = roundLengthInSeconds + (float)waitUntilStart;
    }

    [Server]
    private void TimeUpdate()
    {
        //Debug.Log("what " + timeLeft);

        if (timeLeft > 0)
        {
            timeLeft = timeLeft - Time.deltaTime;

            SyncTime(timeLeft);

            if (timeLeft < 0) timeLeft = 0;

            //Debug.Log(timeLeft);
        }

        else
        {
            //TimeStart();
            RpcDraw();
            //NetworkManager.singleton.ServerChangeScene(NetworkManager.singleton.onlineScene);

            TriggerSceneReload();
        }
    }

    [ClientRpc]
    private void SyncTime(float time)
    {
        timeLeft = time;
    }

    [ClientRpc]
    private void Finish()
    {
        NetworkManager.singleton.StopClient();
    }
    
    [Command]
    private void RequestSyncTime()
    {
        SyncTime(timeLeft);
    }

    [Command]
    public void RequestSceneReload()
    {
        TriggerSceneReload();
    }

    [Server]
    public void TriggerSceneReload()
    {
        waitingForReload = true;

        sceneWaitTime = sceneReloadWaitTime;

        //ReloadAfterTime(sceneReloadWaitTime);
    }

    [Server]
    private void SceneReload()
    {
        Debug.Log("Reloading scene!");

        TimeStart();

        NetworkManager.singleton.ServerChangeScene(NetworkManager.singleton.onlineScene);
    }

    [ClientRpc]
    private void SendResults()
    {
        SendResultsFromClient();
    }

    
    private void SendResultsFromClient()
    {
        if (resultsSent)
            return;
        //Envio al servidor del resultado
        //Partida Finalizada a controlador de servidores
        GameData gameData = GameManager.instance.gameData;
        gameData.rounds = GameManager.instance.results.ToArray();
        if(gameData.shotsFired != 0)
            gameData.accuracy = (gameData.accuracy / gameData.shotsFired) * 100.0f;

        ServerMessage m;
        int n = 0;
        while (n < 5 && !resultsSent)
        {
            m = ClientCommunication.SendRoundInfo(gameData);

            //Manejo de errores basico
            if(m.code != 200)
            {
                GameManager.instance.ThrowErrorScreen(m.code, ((REST_Error)m).message);
                Debug.Log("Error de envio de resultados");
            }
            else
            {
                Debug.Log("Envio de los resultados");
                resultsSent = true;
            }
            n++;
        }

        CloseServer();

        Time.timeScale = 1.0f;
    }

    void CloseServer()
    {
        ServerMessage m;
        GameData gameData = GameManager.instance.gameData;

        //Comunicacion partida finalizada (puerto libre)
        m = ClientCommunication.FinishMatch(GameManager.instance.ID, gameData.rivalID);

        if (m.code != 200)
        {
            GameManager.instance.ThrowErrorScreen(m.code, ((REST_Error)m).message);
        }
    }

    [ClientRpc]
    private void RpcExitQueue()
    {
        if(GameManager.instance.inQueue)
            ExitQueue();
        bothConnected = true;
    }


    private void ExitQueue()
    {
        ServerMessage m = ClientCommunication.LeaveQueue();
        if (m.code == 403)
        {
            m = ClientCommunication.Refresh();

            if (m.code != 200)
            {
                GameManager.instance.ThrowErrorScreen(m.code, ((REST_Error)m).message);
            }
            else
            {
                ExitQueue();
            }

        }
        else if(m.code != 200)
            GameManager.instance.ThrowErrorScreen(m.code, ((REST_Error)m).message);
        else
            GameManager.instance.inQueue = false;
    }

    [Command]
    public void FinishGameOnDisconnect()
    {
        RpcFinish();
        SendResults();
        Finish();
        Application.Quit();
    }

    [ClientRpc]
    private void RpcFinish()
    {
        if(!exit)
            WinDisconnect();
    }


    private void OnApplicationQuit()
    {
        if (!isServer)
        {
            exit = true;
            LoseDisconnect();
            SendResultsFromClient();
        }
    }
}
