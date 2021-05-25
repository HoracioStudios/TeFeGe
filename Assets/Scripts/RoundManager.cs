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

    [SyncVar]
    float roundLengthInSeconds = 45;

    [SyncVar]
    int currentRound = 0;

    float timeLeft;

    public Text timeTxt;
    public Image[] points;

    [SyncVar]
    bool waitingForReload = false;

    private bool resultsSent = false;
    private float timeBeforeStart = 0.0f;
    private bool gameStarted = false;
    private bool exit = false;

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
    }

    private void Start()
    {
        TimeStart();

        //if (isServer)
        timeBeforeStart = Time.realtimeSinceStartup;

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
        if (waitUntilStart > Time.realtimeSinceStartup - timeBeforeStart)
        {
            return;
        }
        else
        {
            Time.timeScale = 1;
        }
        if (isServer)
        {
            if (NetworkManager.singleton.numPlayers == 1)
            {
                if(Time.realtimeSinceStartup - timeBeforeStart > 60.0f)
                {
                    // Si solo un jugador se conecta, y despues de 1 min, la partida termina sin resultado
                    Finish();
                    Application.Quit();
                }
                if (gameStarted)
                {
                    RpcWinDisconnect();
                    SendResults();
                    Finish();
                    Application.Quit();
                }
            }
            if (NetworkManager.singleton.numPlayers >= 2 && !gameStarted)
            {
                RpcExitQueue();
                gameStarted = true;
            }

            if (!waitingForReload)
                TimeUpdate();
            else
            {

                sceneWaitTime -= Time.deltaTime;

                if(sceneWaitTime <= 0)
                {
                    GameManager.instance.currentRound++;

                    if (GameManager.instance.currentRound < GameManager.instance.totalRounds)
                        SceneReload();
                    else
                    {
                        Debug.Log("AYYYYYYYYYYYYY QUE EH QUE HAY QUE HASER LA TRANSISIÓN AYYYYYYYYYYYY QUE SE HA ACABAO LA PARTIDA");
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
        int r = GameManager.instance.results.Count;
        for (int i = r; i < GameManager.instance.totalRounds; i++)
        {
            GameManager.instance.results.Add(new RoundResult(1, 0.0f));
        }
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

    [Client]
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
        timeLeft = roundLengthInSeconds;
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

    [Client]
    private void SendResultsFromClient()
    {
        if (resultsSent)
            return;

        //Envio al servidor del resultado
        //Partida Finalizada a controlador de servidores
        GameData gameData = GameManager.instance.gameData;
        gameData.rounds = GameManager.instance.results.ToArray();
        Message m = ClientCommunication.SendRoundInfo(gameData);

        //Manejo de errores basico
        if(m.code != 200)
        {
            GameManager.instance.ThrowErrorScreen(m.code);
            Debug.Log("Error de envio de resultados");
        }
        else
        {
            Debug.Log("Envio de los resultados");
            resultsSent = true;
        }

        //Comunicacion partida finalizada (puerto libre)
        m = ClientCommunication.FinishMatch(GameManager.instance.ID, gameData.rivalID);

        if(m.code != 200)
        {
            GameManager.instance.ThrowErrorScreen(m.code);
        }
    }

    [ClientRpc]
    private void RpcExitQueue()
    {
        if(GameManager.instance.inQueue)
            ExitQueue();
    }

    [Client]
    private void ExitQueue()
    {
        Message m = ClientCommunication.LeaveQueue();
        if (m.code == 403)
        {
            m = ClientCommunication.Refresh();

            if (m.code != 200)
            {
                GameManager.instance.ThrowErrorScreen(m.code);
            }
            else
            {
                ExitQueue();
            }

        }
        else if(m.code != 200)
            GameManager.instance.ThrowErrorScreen(m.code);
        else
            GameManager.instance.inQueue = false;
    }

    [Command]
    private void FinishGameOnDisconnect()
    {
        RpcFinish();
        Finish();
        Application.Quit();
    }

    [ClientRpc]
    private void RpcFinish()
    {
        if (exit)
            LoseDisconnect();
        else
            WinDisconnect();
    }

    private void OnApplicationQuit()
    {
        exit = true;
        FinishGameOnDisconnect();

        SendResultsFromClient();
    }
}
