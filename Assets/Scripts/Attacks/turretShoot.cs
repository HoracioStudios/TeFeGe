using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretShoot : normalShoot
{

    protected override void Start()
    {
        base.Start();

        gunRot = gameObject.GetComponent<TurretRotation>();
        actualBullets = maxBullets;
    }

    public void ShootTrigger()
    {
        Shoot();
    }

    public override void Shoot()
    {
        base.Shoot();
        actualBullets = maxBullets;
    }
}
