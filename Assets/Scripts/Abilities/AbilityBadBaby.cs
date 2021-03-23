using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBadBaby : Abilities
{
    public GameObject bullet;
    public Transform spawnPoint;
    public float speedBullet;

    protected FMODUnity.StudioEventEmitter emitter;

    protected override void Start()
    {
        base.Start();

        foreach (FMODUnity.StudioEventEmitter em in gameObject.GetComponents<FMODUnity.StudioEventEmitter>())
        {
            if (em.Event == "event:/Abilities/ShootingAbilities")
            {
                emitter = em;
            }
            else
            {
                //Debug.Log(em.Event);
            }
        }
    }

    protected override bool PrepareAbility()
    {
        //View template
        base.PrepareAbility();
        template.SetActive(true);
        return true;
    }

    protected override void UseAbility()
    {
        //Shoot the bullet
        GameObject obj = Instantiate(bullet, spawnPoint.position, bullet.transform.rotation);
        gunRotation gunRot = GetComponent<gunRotation>();
        obj.GetComponent<Rigidbody>().velocity = gunRot.getGunDir() * speedBullet;
        obj.gameObject.layer = gameObject.layer;

        //Gets the distance from the Ability Template
        obj.GetComponent<BadBabySpecialBullet>().time_ = (template.transform.localScale.x / speedBullet)*2;

        if(emitter)
            emitter.Play();
        //Set everything false
        base.UseAbility();
        template.SetActive(false);
    }
}
