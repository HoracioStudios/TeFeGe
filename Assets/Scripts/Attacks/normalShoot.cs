using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class normalShoot : NetworkBehaviour
{

    protected bool block_ = false;


    [Header("State")]
    public float time_ = 0f;
    [SyncVar]
    public float actualBullets;
    public bool reloading = false;

    [Header("Camera Behaviour")]
    public CameraBehaviour cam;

    [Header("Shoot stuff")]
    public gunRotation gunRot;
    public Transform spawn;
    public GameObject shot;

    [Header("Parameters")]
    public float speed = 1f;
    public float cadence = 1f;
    public float reloadTime = 1f; //Time it takes to reload
    public float maxBullets = 1;
    public bool rotateBullet = true;
    public float innacuracy = 0.0f;
    public bool semiautomatic = false;
    //public float bulletTTL = 1f; //Time bullets live

    protected StateMachine states;

    protected FMODUnity.StudioEventEmitter emitter;
    protected FMODUnity.StudioEventEmitter reloadEmitter;


    bool semiautoomaticTrigger_ = false;

    protected virtual void Start()
    {
        states = gameObject.GetComponent<StateMachine>();
        actualBullets = maxBullets;

        foreach (FMODUnity.StudioEventEmitter em in gameObject.GetComponents<FMODUnity.StudioEventEmitter>())
        {
            if(em.Event == "event:/Reload")
            {
                reloadEmitter = em;
            }
            else if (em.Event == "event:/Shooting")
            {
                emitter = em;
            }
        }

    }

    protected virtual void Update()
    {
        // Si no es el jugador local no se hace el Update
        if (!isLocalPlayer) return;

        if(Input.GetAxis("Fire") == 0 && Input.GetAxis("Fire_Joy") == 0 && semiautomatic)
        {
            semiautoomaticTrigger_ = false;
        }
        if (!reloading && !block_ && time_ <= 0f && actualBullets > 0 && (Input.GetAxis("Fire") != 0 || Input.GetAxis("Fire_Joy") != 0))
        {
            if (states && states.GetState().state <= States.Root && !semiautoomaticTrigger_) {
                Shoot();
                cam.startShaking();
                time_ = cadence;
                if (semiautomatic)
                    semiautoomaticTrigger_ = true;
            }
        }
        else if ((!reloading && actualBullets <= 0) || (!reloading && actualBullets > 0 && actualBullets < maxBullets && Input.GetKeyDown(KeyCode.R)))
        {
            Reload();
        }
        else if (time_ > 0f)
        {
            time_ -= Time.deltaTime;
            if (reloading)
            {
                actualBullets += ((Time.deltaTime * maxBullets) / reloadTime);
                if (time_ <= 0f)
                {
                    actualBullets = maxBullets;
                    reloading = false;

                    //reloadEmitter.Play();
                }
            }
        }
    }

    //This is a virtual method and will be different for each character
    public virtual void Shoot()
    {
        if (emitter)
        {
            emitter.Play();
        }

        ServerShoot(gunRot.getGunDir(), gameObject.layer);        
    }

    [Command]
    private void ServerShoot(Vector3 gunRotation, int layer)
    {
        GameObject obj;
        
        if (rotateBullet)
            obj = Instantiate(shot, transform.position, transform.rotation);
        else
            obj = Instantiate(shot, spawn.position, Quaternion.identity);

        obj.GetComponent<Rigidbody>().velocity = (gunRotation + Random.insideUnitSphere * innacuracy) * speed;

        //fixes rotation so bullet looks in the direction it's shot
        if (rotateBullet)
        {
            obj.transform.rotation = Quaternion.LookRotation(obj.GetComponent<Rigidbody>().velocity, Vector3.up);
            obj.transform.rotation *= Quaternion.Euler(90, -90, 0);
        }
        obj.layer = layer;
        Debug.Log("GameObject layer (segun cliente): " + layer);
        Debug.Log("GameObject layer (segun server): " + gameObject.layer);

        NetworkServer.Spawn(obj);        
        RpcChangeBulletLayer(obj, gameObject.layer);
        actualBullets--;
    }

    [ClientRpc]
    protected void RpcChangeBulletLayer(GameObject obj, int layer)
    {
        Debug.Log("Bullet layer: " + gameObject.layer);
        obj.layer = gameObject.layer;
    }

    [Client]
    protected virtual void Reload()
    {
        time_ = reloadTime * (maxBullets - actualBullets)/maxBullets;
        reloading = true;

        reloadEmitter.Play();
    }

    protected Vector3 Rotate(Vector3 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.z;
        v.x = (cos * tx) - (sin * ty);
        v.z = (sin * tx) + (cos * ty);
        return v.normalized;
    }

    public float getCurrentBullets()
    {
        return actualBullets;
    }

    public float getMaxBullets()
    {
        return maxBullets;
    }

    // Puede que tanto desde el servidor como desde el cliente
    public void SetBlockShoot(bool set)
    {
        block_ = set;
    }

}
