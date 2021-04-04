using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class gunRotation : NetworkBehaviour
{
    protected Vector3 gunDir;
    public SpriteRenderer _sprite;

    protected StateMachine states; //States Machine from the character

    private bool local;
    protected virtual void Start()
    {
        local = GetComponentInParent<PlayerSetup>().IsOurLocalPlayer();
        states = GetComponentInParent<StateMachine>();
    }

    void Update()
    {
        if (!local) return;

        if (!controllerAim() && !GameManager.instancia.isControllerMode)
        {
            mouseAim();
        }
    }

    public virtual void mouseAim()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;

        if (states.GetState().state <= States.Root)
            manageDir(dir);
    }

    public virtual bool controllerAim()
    {
        bool ret = false;

        if (Input.GetAxis("Aim_X") != 0 || Input.GetAxis("Aim_Y") != 0)
        {
            ret = true;
            
            Vector2 dir = new Vector2(Input.GetAxis("Aim_X"), Input.GetAxis("Aim_Y"));
            if (states.GetState().state <= States.Root)
                manageDir(dir);
        }

        return ret;
    }

    public virtual void manageDir(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //this way it's always parallel to the ground
        transform.localEulerAngles = new Vector3(transform.parent.transform.eulerAngles.x / 2.0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        gunDir = dir.normalized;
        gunDir.z = gunDir.y;
        gunDir.y = 0;
        _sprite.flipY = gunDir.x < 0;

        //Debug.DrawLine(transform.position, transform.position + gunDir, Color.green);
        //Debug.Log(gunDir);
    }

    //this way we can get the direction of aim to shoot
    public Vector3 getGunDir() { return gunDir; }

}
