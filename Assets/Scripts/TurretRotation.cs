using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotation : gunRotation
{
    public float angle = 0f;

    public float nextAngle;

    bool clockwise = false;

    public float rotSpeed = 1f;

    public float minAngle = 1f;
    public float maxAngle = 10f;

    public turretShoot shoot;

    public Vector3 dir;
    public Vector3 nextDir;


    protected override void Start()
    {
        base.Start();

        angle = transform.localEulerAngles.z;
        nextAngle = angle;

        float auxAngle = UnityEngine.Random.Range(minAngle, maxAngle);

        if (clockwise)
            nextAngle += auxAngle;
        else
            nextAngle -= auxAngle;

        dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
        nextDir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * nextAngle), Mathf.Sin(Mathf.Deg2Rad * nextAngle), 0);
    }

    void Update()
    {
        manageDir(dir);

        if (checkAngle())
        {
            if (shoot)
                shoot.ShootTrigger();
            
            float auxAngle = UnityEngine.Random.Range(minAngle, maxAngle);

            clockwise = !clockwise;

            if (clockwise)
                nextAngle += auxAngle;
            else
                nextAngle -= auxAngle;


            nextDir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * nextAngle), Mathf.Sin(Mathf.Deg2Rad * nextAngle), 0);
        }
        else
        {
            if (clockwise)
                angle += rotSpeed * Time.deltaTime;
            else
                angle -= rotSpeed * Time.deltaTime;

            dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);

            //transform.localEulerAngles = new Vector3(0, 0, angle);
        }
    }

    bool checkAngle()
    {
        if(clockwise)
            return (angle >= nextAngle);

        else
            return (angle <= nextAngle);
    }

    public override bool controllerAim()
    { 
        return false;
    }

    public override void manageDir(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //this way it's always parallel to the ground
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z - 90);
        gunDir = dir.normalized;
        gunDir.z = gunDir.y;
        gunDir.y = 0;
        //_sprite.flipY = gunDir.x < 0;

        //Debug.DrawLine(transform.position, transform.position + gunDir, Color.green);
        //Debug.Log(gunDir);
    }
}
