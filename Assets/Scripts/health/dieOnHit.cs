using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieOnHit : timeToLive
{
    public string[] ignore;

    virtual protected void OnCollisionEnter(Collision collision)
    {
        bool col = false;
        foreach (string i  in ignore)
        {
            col = col || collision.gameObject.tag == i;
        }

        col = col && collision.gameObject.tag != gameObject.tag;

        //Other bullets cant destroy this bullet
        if (col)
        {
            //And ignore the collision with bullets that can collide
            Physics.IgnoreCollision(collision.collider, GetComponent<Collision>().collider);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
