using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;
using System;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    float x = 0;
    int vel = 0;
    public GameObject bullet;
    public Material mat;
    public Camera cam;

    public static event Action NewBall;

    [SyncVar(hook = nameof(OnHolaCountChanged))]
    int holaCount = 0;

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal * 0.1f, moveVertical * 0.1f, 0);
        transform.position = transform.position + movement;
    }

    [Command]
    void HandleShoot()
    {        
        Debug.Log("Llamada disparo");
        GameObject projectile = Instantiate(bullet, transform.position, transform.rotation);
        NetworkServer.Spawn(projectile);
        NewBallClientCall();
    }

    [ClientRpc]
    void NewBallClientCall()
    {
        NewBall.Invoke();
    }

    private void Start()
    {
        if (isLocalPlayer)
            cam.enabled = true;
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            HandleMovement();
            if (Input.GetKeyDown(KeyCode.Space))
                HandleShoot();
        }
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("Material cambiado");
        if (col.gameObject.tag == "Bullet")
        {
            Debug.Log("Material cambiado");
            mat.color = Color.white;
        }
    }


    public override void OnStartServer()
    {
        Debug.Log("Player has been spawned on the server!");
    }

    [Command]
    void Hola()
    {
        Debug.Log("Received Hola from Client!");
        holaCount += 1;
        ReplyHola();
    }

    [TargetRpc]
    void ReplyHola()
    {
        Debug.Log("Received Hola from Server!");
    }

    [ClientRpc]
    void TooHigh()
    {
        Debug.Log("Too high!");
    }

    void OnHolaCountChanged(int oldCount, int newCount)
    {
        Debug.Log($"We had {oldCount} holas, but now we have {newCount} holas!");
    }
}
