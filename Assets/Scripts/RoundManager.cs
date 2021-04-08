using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class RoundManager : NetworkBehaviour
{
    [Serializable]
    public class RoundResult
    {
        public RoundResult(double res, double t) { result = res; time = t; }

        double result;
        double time;
    }

    public int pointsMe = 0;
    public int pointsOther = 0;
    public int totalRounds = 0;

    double roundLengthInSeconds = 5;//45;

    [SyncVar]
    double timeLeft;

    //get público, set privado
    static public RoundManager instance { get; private set; }

    List<RoundResult> results = new List<RoundResult>();

    private void Awake()
    {
        // si es la primera vez que accedemos a la instancia del GameManager,
        // no existira, y la crearemos
        if (instance == null)
        {
            // guardamos en la instancia el objeto creado
            // debemos guardar el componente ya que _instancia es del tipo GameManager
            instance = this;

            // hacemos que el objeto no se elimine al cambiar de escena
            DontDestroyOnLoad(this.gameObject);

            //GameManager.instance.roundManager = instance;

            
        }
    }

    private void Start()
    {
        instance.TimeStart(roundLengthInSeconds);
    }

    //for testing
    private void Update()
    {
        if(isServer)
            instance.TimeUpdate();
    }

    public void TriggerRoundEnd(bool localPlayer)
    {
        Debug.Log("Did I win? " + !localPlayer);

        if (localPlayer)
            RoundEnd(0);
        else
            RoundEnd(1);
    }

    public void TriggerDraw()
    {
        Debug.Log("Draw!");

        instance.TimeStart(roundLengthInSeconds);

        RoundEnd(0.5);
    }

    private void RoundEnd(double result)
    {
        results.Add(new RoundResult(result, 1.0 - timeLeft / roundLengthInSeconds));

        totalRounds++;

        Debug.Log("round: " + totalRounds + "\n" + results);

        if (totalRounds >= 3)
            SceneReload();
        else
            SceneReload();
    }

    [Command]
    private void CmdDraw()
    {
        RpcDraw();
    }

    [ClientRpc]
    private void RpcDraw()
    {
        RoundManager.instance.TriggerDraw();
    }

    [Command]
    private void SceneReload()
    {
        NetworkManager.singleton.ServerChangeScene(NetworkManager.singleton.onlineScene);
    }

    [Command]
    private void TimeStart(double maxTime)
    {
        timeLeft = maxTime;
    }

    [Server]
    private void TimeUpdate()
    {
        Debug.Log(timeLeft);
        return;

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            Debug.Log(timeLeft);
        }

        else
            RpcDraw();
    }
}
