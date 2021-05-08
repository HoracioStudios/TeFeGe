using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class shotGun : normalShoot
{
    public float shotAngle_;
    public int shotNum_;

    public override void Shoot()
    {
        if (emitter)
        {
            emitter.Play();
        }

        GameManager.instance.shots += shotNum_;
        actualBullets -= shotNum_;
        CmdServerShoot(gunRot.getGunDir(), actualBullets);
    }

    // Pasamos la rotacion del arma y las balas actuales segun el cliente para que se sincronice con el server
    // y los demas clientes
    [Command]
    private void CmdServerShoot(Vector3 gunRotation, float serverActualBullets)
    {
        float incr = shotAngle_ / shotNum_;

        for (int i = 0; i < shotNum_; i++)
        {
            GameObject obj = Instantiate(shot, spawn.position, transform.rotation);
            obj.GetComponent<Rigidbody>().velocity = Rotate(gunRotation + Random.insideUnitSphere * innacuracy, (shotAngle_ / 2.0f) - incr * i) * speed;

            //fixes rotation so bullets look in the direction they move
            obj.transform.rotation = Quaternion.LookRotation(obj.GetComponent<Rigidbody>().velocity, Vector3.up);
            obj.transform.rotation *= Quaternion.Euler(90, -90, 0);

            obj.layer = gameObject.layer;

            //Spawn object in server
            NetworkServer.Spawn(obj);
            RpcChangeBulletLayer(obj);
        }
        actualBullets = serverActualBullets;
    }

}
