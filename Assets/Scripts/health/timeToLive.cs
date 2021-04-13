using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class timeToLive : health
{
    public int bounces = 3;
    public float time_ = 1f;

    int numBounces = 0;
    bool dead = false;

    private void Update()
    {
        time_ -= Time.deltaTime;
        if (time_ < 0 && !dead)
        {
            NetworkServer.Destroy(gameObject);
            dead = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        numBounces++;
        if (numBounces >= bounces)
            NetworkServer.Destroy(gameObject);
    }
}