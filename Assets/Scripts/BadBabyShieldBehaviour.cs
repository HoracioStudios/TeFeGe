using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadBabyShieldBehaviour : MonoBehaviour
{
    //public GameObject obj;
    public string[] colWith;

    //public badBabyShoot BBS;

    private void Start()
    {
        //BBS = obj.GetComponent<badBabyShoot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        bool col = false;
        foreach (string i in colWith)
        {
            col = col || other.gameObject.tag == i;
        }

        if (col)
        {
            other.gameObject.GetComponent<health>().kill();
            gameObject.SetActive(false);//BBS.killShield(); //La idea va a ser tener un array en el script badbabyshoot con las bolas y llevar ahi la cuenta (preguntar el comportamiento exacto de esas bolas
                              //para saber si se prefiere que sean simetricas o que desaparezcan sin mas)
        }
    }
}
