using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCamomila : Abilities
{

    public Animator _animator;
    public float dashSpeed;
    public float dashTime;

    public float startTime;

    public GameObject bullet;
    public Transform spawnPoint;

    public FMODUnity.StudioEventEmitter emitter;

    basicMovement3D move_;
    gunRotation gr_;
    normalShoot ns_;

    protected override void Start()
    {
        base.Start();
        move_ = GetComponentInParent<basicMovement3D>();
        gr_ = GetComponent<gunRotation>();
        ns_ = GetComponent<normalShoot>();
    }

    protected override void Update()
    {
        base.Update();
        if (startTime > 0 && Time.fixedTime >= startTime + dashTime)
            EndDash();
    }

    protected override bool PrepareAbility()
    {
        if (!move_.getAnimator().GetBool("moving"))
            return false;

        float x = move_.getMoveX();
        float z = move_.getMoveZ();

        move_.enabled = false;
        //Use ability turbio

        x *= dashSpeed;
        z *= dashSpeed;

        bool spriteFlip = move_.getSpriteFlip();

        move_.getAnimator().SetBool("backwards", (x > 0f && spriteFlip) || (x <= 0f && !spriteFlip));
        move_.getAnimator().ResetTrigger("stopDash");
        move_.getAnimator().SetTrigger("dashing");
        //gameObject.GetComponentInParent<basicMovement3D>().getAnimator().Play("dashing");

        gameObject.GetComponentInParent<Rigidbody>().velocity = new Vector3(x, gameObject.GetComponentInParent<Rigidbody>().velocity.y, z).normalized * dashSpeed;

        startTime = Time.fixedTime;

        emitter.Play();

        return true;
    }

    protected void EndDash()
    {
        startTime = -1;

        gameObject.GetComponentInParent<basicMovement3D>().getAnimator().ResetTrigger("dashing");
        gameObject.GetComponentInParent<basicMovement3D>().getAnimator().SetTrigger("stopDash");
        //gameObject.GetComponentInParent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        gameObject.GetComponentInParent<basicMovement3D>().enabled = true;

        float f = ns_.actualBullets;
        ns_.innacuracy = 0.25f;
        for (int i = 0; i < f; i++)
        {
            ns_.Shoot();
        }
        ns_.innacuracy = 0.0f;
    }
}
