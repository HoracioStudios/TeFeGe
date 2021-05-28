using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BadBabySpecialBullet : dieOnHit
{
    public float charmTime; // Amount of damage it does

    private bool shouldDie = false;

    private FMODUnity.StudioEventEmitter emitter;

    private Transform shooter;

    private void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void Update()
    {
        if (shouldDie && (!emitter || !emitter.IsPlaying()))
        {
            CmdDestroy();
        }
    }

    //Only do damage when collides with other team pj
    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
            return;

        //Ignore collisions with other bullets and handle collisions with "enemies"
        if ("Wall" != collision.gameObject.tag)
        {
            StateMachine state = collision.gameObject.GetComponent<StateMachine>();

            if (state)
            {
                Vector3 dir = transform.position - collision.gameObject.transform.position;
                state.SetState(States.Charm, charmTime, dir.normalized, 0.3f);
            }

            if (GetComponent<FMODUnity.StudioEventEmitter>())
            {
                GetComponent<FMODUnity.StudioEventEmitter>().Play();
            }

            shouldDie = true;

            transform.GetChild(0).gameObject.SetActive(false);

            if(GetComponent<SphereCollider>())
                GetComponent<SphereCollider>().enabled = false;
        }

        base.OnCollisionEnter(collision);
    }

    [Command]
    private void CmdDestroy()
    {
        NetworkServer.Destroy(gameObject);
    }

    public void setShooter(Transform sh)
    {
        shooter = sh;
    }
}
