using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotGun : normalShoot
{
    public float shotAngle_;
    public int shotNum_;

    public override void Shoot()
    {
        float incr = shotAngle_ / shotNum_;

        for(int i = 0; i < shotNum_; i++) {
            GameObject obj = Instantiate(shot, spawn.position, transform.rotation);
            obj.GetComponent<Rigidbody>().velocity =  Rotate(gunRot.getGunDir() + Random.insideUnitSphere * innacuracy, (shotAngle_/2.0f) - incr*i ) * speed;

            //fixes rotation so bullets look in the direction they move
            obj.transform.rotation = Quaternion.LookRotation(obj.GetComponent<Rigidbody>().velocity, Vector3.up);
            obj.transform.rotation *= Quaternion.Euler(90, -90, 0);

            obj.layer = gameObject.layer;
        }

        if (emitter)
        {
            emitter.Play();
        }

        actualBullets -= shotNum_;
    }

}
