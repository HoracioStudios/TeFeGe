using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class damage : MonoBehaviour
{
    public float dmg; // Amount of damage it does

    public FMODUnity.StudioEventEmitter emitter;

    //Only do damage when collides with other team pj
    private void OnCollisionEnter(Collision collision)
    {        
        //Ignore collisions with other bullets and handle collisions with "enemies"
        if ("Wall" != collision.gameObject.tag)
        {
            collision.gameObject.GetComponent<health>().TakeFamage(dmg);
            Debug.Log("Entra a la colision " + collision.gameObject.tag + ", " + collision.gameObject.layer + ", " + collision.gameObject.name);
            NetworkServer.Destroy(gameObject);
            //Destroy(gameObject);
        }
        else if (emitter)
            emitter.Play();

    }
}
