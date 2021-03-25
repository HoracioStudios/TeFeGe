using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{

    public Rigidbody rb;

    void Start()
    {
        rb.AddForce(new Vector3 (0, 1, 0) * 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
