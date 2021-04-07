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
        int result;
        double time;
    }

    int pointsMe = 0;
    int pointsOther = 0;
    int totalRounds = 0;

    double roundLengthInSeconds = 42;

    [SyncVar]
    double timeLeft;

    //get público, set privado
    static public RoundManager instance { get; private set; }

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
        //myId = netId;
    }

    //for testing

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M))
            //TriggerRoundEnd();
    }
    
    public void TriggerRoundEnd(bool localPlayer)
    {
        Debug.Log("Did I win? " + localPlayer);

        if (localPlayer)
            RoundEnd(0);
        else
            RoundEnd(1);
    }
    
    private void RoundEnd(int result)
    {

        //!localPlayer == no soy yo
        //if(!isLocalPlayer)
        //    gameObject.SetActive(false);

        //RoundManager.instance.RoundEnd(1 - (int)isLocalPlayer);

        //0.5

        if (result == 1)
            pointsMe++;
        else
            pointsOther++;

        totalRounds++;


        ////cómo lo hacemos????
        //if (totalRounds >= 3)
        //    GameManager.instance.RestartScene();
        //else
        //    GameManager.instance.RestartScene();
    }
}
