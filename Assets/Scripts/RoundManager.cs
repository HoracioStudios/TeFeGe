using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;

public class RoundManager : NetworkBehaviour
{
    [SyncVar]
    double roundLengthInSeconds = 45;
    
    double timeLeft;

    public Text timeTxt;
    public Image[] points;

    private void Awake()
    {
        GameManager.instance.roundManager = this;
    }

    private void Start()
    {

        TimeStart();

        if (isClient)
        {
            timeTxt.text = ((int)roundLengthInSeconds).ToString("D2");

            List<GameManager.RoundResult> results = GameManager.instance.results;

            for (int i = 0; i < results.Count && i < points.Length; i++)
            {
                GameManager.RoundResult r = results[i];

                Color col = Color.white;

                switch (r.result)
                {
                    case 1.0:
                        col = Color.green;
                        break;

                    case 0.5:
                        col = Color.yellow;
                        break;

                    case 0.0:
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
            TimeUpdate();
        }

        if(isClient)
        {
            int truncatedTime = (int)roundLengthInSeconds;

            if (timeLeft != roundLengthInSeconds)
                truncatedTime = (int)timeLeft + 1;

            timeTxt.text = truncatedTime.ToString("D2");
        }
    }

    [ClientRpc]
    public void TriggerRoundEnd(double time)
    {
        Debug.Log("Did I win? " + !isLocalPlayer);

        if (isLocalPlayer)
            RoundEnd(0, time);
        else
            RoundEnd(1, time);
    }

    [Client]
    public void TriggerDraw()
    {
        Debug.Log("Draw!");

        TimeStart();

        RoundEnd(0.5, 0.0);
    }

    [Client]
    private void RoundEnd(double result, double time)
    {
        GameManager.instance.results.Add(new GameManager.RoundResult(result, 1.0 - (time / roundLengthInSeconds)));

        GameManager.instance.currentRound++;

        Debug.Log("round: " + GameManager.instance.currentRound);

        if (GameManager.instance.currentRound >= 3)
            SceneReload();
        else
            SceneReload();
    }

    [Server]
    public void ServerRoundEnd()
    {
        TriggerRoundEnd(timeLeft);
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

    [Command]
    private void SceneReload()
    {
        TimeStart();

        NetworkManager.singleton.ServerChangeScene(NetworkManager.singleton.onlineScene);
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
            TimeStart();
            RpcDraw();
            //NetworkManager.singleton.ServerChangeScene(NetworkManager.singleton.onlineScene);
        }
    }

    [ClientRpc]
    private void SyncTime(double time)
    {
        timeLeft = time;
    }
}
