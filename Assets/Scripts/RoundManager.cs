using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;

public class RoundManager : NetworkBehaviour
{
    double sceneReloadWaitTime = 1;
    double sceneWaitTime;

    [SyncVar]
    float roundLengthInSeconds = 45;

    [SyncVar]
    int currentRound = 0;

    float timeLeft;

    public Text timeTxt;
    public Image[] points;

    [SyncVar]
    bool waitingForReload = false;

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

        if (isClient)
        {
            currentRound = GameManager.instance.currentRound;

            timeTxt.text = ((int)roundLengthInSeconds).ToString("D2");

            List<GameManager.RoundResult> results = GameManager.instance.results;

            for (int i = 0; i < results.Count && i < points.Length; i++)
            {
                GameManager.RoundResult r = results[i];

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
    }

    //for testing
    private void Update()
    {
        if (isServer)
        {
            //Debug.Log("ping!");

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

        GameManager.instance.results.Add(new GameManager.RoundResult(result, 1.0f - (time / roundLengthInSeconds)));

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
        Telemetria.instance.EndGameEvent();
        GameManager.instance.LoadScene("ResultScreen");
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
}
