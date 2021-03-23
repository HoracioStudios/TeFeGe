using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadBabySpecialBullet : dieOnHit
{
    public float charmTime; // Amount of damage it does

    private bool shouldDie = false;

    private FMODUnity.StudioEventEmitter emitter;

    private void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void Update()
    {

        if (shouldDie && (!emitter || !emitter.IsPlaying()))
        {
            Destroy(gameObject);
        }

    }

    //Only do damage when collides with other team pj
    protected override void OnCollisionEnter(Collision collision)
    {
        //Ignore collisions with other bullets and handle collisions with "enemies"
        if ("Wall" != collision.gameObject.tag)
        {
            StateMachine state = collision.gameObject.GetComponent<StateMachine>();

            if(state)
                state.SetState(States.Charm, charmTime, -GetComponent<Rigidbody>().velocity.normalized);

            if (GetComponent<FMODUnity.StudioEventEmitter>())
            {
                GetComponent<FMODUnity.StudioEventEmitter>().Play();
            }

            shouldDie = true;

            transform.GetChild(0).gameObject.SetActive(false);

            GetComponent<SphereCollider>().enabled = false;
        }
        else
        {
            base.OnCollisionEnter(collision);
        }
    }
}
