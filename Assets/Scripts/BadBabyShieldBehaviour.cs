using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BadBabyShieldBehaviour : MonoBehaviour
{
    //public GameObject obj;
    public string[] colWith;

    public static event Action ShieldCollision;


    private void OnTriggerEnter(Collider other)
    {
        // Evitamos que los en los dos clientes calcule la colision forzando que sea
        // solo el que dispara

        bool col = false;
        foreach (string i in colWith)
        {
            col = col || other.gameObject.tag == i;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("ATeam") && col)
        {
            NetworkServer.Destroy(other.gameObject);
            return;
        }
        if (col)
        {
            NetworkServer.Destroy(other.gameObject);
            ShieldCollision.Invoke();
            //gameObject.SetActive(false);//BBS.killShield(); //La idea va a ser tener un array en el script badbabyshoot con las bolas y llevar ahi la cuenta (preguntar el comportamiento exacto de esas bolas
                              //para saber si se prefiere que sean simetricas o que desaparezcan sin mas)
        }
    }
}
