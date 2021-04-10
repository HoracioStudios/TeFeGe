using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class timeToLive : health
{
    public int bounces = 3;
    public float time_ = 1f;

    int numBounces = 0;
    // TODO: Corregir error de balas de bob ojocojo
    void Start()
    {
        Invoke("DestroyInServer", time_);
    }

    void DestroyInServer()
    {
        NetworkServer.Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        numBounces++;
        if (numBounces >= bounces)
            NetworkServer.Destroy(gameObject);
    }
}