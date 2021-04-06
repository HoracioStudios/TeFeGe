using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

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

            GameManager.instance.roundManager = instance;
        }
    }

    //for testing

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            TriggerRoundEnd();
    }

    public void TriggerRoundEnd()
    {
        RoundEnd(netId);
    }

    [Command]
    private void RoundEnd(uint id)
    {
        Debug.Log("thrown id = " + id + '\n' + "My id = " + netId);

        if (id == netId)
            pointsMe++;
        else
            pointsOther++;

        totalRounds++;

        if (totalRounds >= 3)
            GameManager.instance.RestartScene();
        else
            GameManager.instance.RestartScene();
    }
}
