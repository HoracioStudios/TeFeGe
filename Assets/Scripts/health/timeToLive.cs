using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class timeToLive : health
{
    public int bounces = 3;
    public float time_ = 1f;
    int numBounces = 0;

    void Start()
    {
        Invoke("CmdDestroyInServer", time_);
    }

    [Command]
    void CmdDestroyInServer()
    {
        NetworkServer.Destroy(gameObject);
    }

    void Update()
    {
        if (numBounces >= bounces)
            Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        numBounces++;
    }
}