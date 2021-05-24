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
        //if (collision.gameObject.layer == LayerMask.NameToLayer("BTeam")) return;

        //Ignore collisions with other bullets and handle collisions with "enemies"
        if ("Wall" != collision.gameObject.tag)
        {
            collision.gameObject.GetComponent<health>().TakeDamage(dmg);
            NetworkServer.Destroy(gameObject);
        }
        else if (emitter)
            emitter.Play();

    }
}
