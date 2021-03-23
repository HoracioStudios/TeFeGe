using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrethealth : health
{
    public SpriteRenderer cannon;
    public turretShoot shoot_;
    public TurretRotation rotation_;
    public FMODUnity.StudioEventEmitter emitter;

    private bool isDead = false;
    public float deactivationTime = 1f;

    private float deactivationCount = 0;

    private void Update()
    {
        if(isDead)
        {
            deactivationCount += Time.deltaTime;
            if(deactivationCount >= deactivationTime)
            {
                currentHealth = maxHealth;

                isDead = false;
                
                emitter.Play();
                emitter.SetParameter("TurretType", 1);
                

                if (cannon)
                    cannon.enabled = true;
                if (shoot_)
                    shoot_.enabled = true;
                if (rotation_)
                    rotation_.enabled = true;
            }
        }
    }

    public override void takeDamage(float dmg)
    {
        if (!isDead)
        {
            currentHealth -= dmg;
            if (currentHealth <= 0)
            {
                deactivationCount = 0;
                
                emitter.Play();
                emitter.SetParameter("TurretType", 0);

                isDead = true;

                if (cannon)
                    cannon.enabled = false;
                if (shoot_)
                    shoot_.enabled = false;
                if (rotation_)
                    rotation_.enabled = false;
            }
        }
    }
}
