using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeToLive : health
{
    public int bounces = 3;
    public float time_ = 1f;
    int numBounces = 0;

    void Start()
    {
        Destroy(gameObject, time_);
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