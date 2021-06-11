using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Abilities : NetworkBehaviour
{
    [Header("Parameters")]
    public GameObject template;
    public FMODUnity.StudioEventEmitter emitter;
    public gunRotation gunRot;
    public float coolDown;
    
    [Header("Control")]
    public bool abilityUp = true;

    protected bool preparing_ = false;

    // Solo necesario si tambien se ve el cd de la habilidad del enemigo
    [SyncVar]
    protected float currentCD_;

    protected StateMachine states_;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentCD_ = coolDown;
        states_ = GetComponentInParent<StateMachine>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(states_.GetState().state >= States.Charm)
        {
            //we have to test it
            template.SetActive(false);
            preparing_ = false;
        }
        else if(abilityUp && !preparing_ && ((Input.GetAxis("FireAbility") != 0 && !GameManager.instance.isControllerMode) || (Input.GetAxis("FireAbility_Joy") != 0 && GameManager.instance.isControllerMode)))
        {
            preparing_ = PrepareAbility();
        }

        if (preparing_ && Input.GetMouseButtonDown(0))
        {
            cancelAbility();
            preparing_ = false;
            Debug.Log("Cancel");
        }

        if (preparing_ && ((Input.GetAxis("FireAbility") == 0 && !GameManager.instance.isControllerMode) || (Input.GetAxis("FireAbility_Joy") == 0 && GameManager.instance.isControllerMode)))
        {
            UseAbility();
            CmdSetCD(0.0f);
            abilityUp = false;
            preparing_ = false;
            Invoke("SetAbilityUp", coolDown); //Puede que se necesite el timer para dar el porcentaje
        }

        updateCD();
    }

    protected void updateCD()
    {
        if (!preparing_ && !abilityUp)
        {
            if (currentCD_ > 0)
                CmdSubstractCD(-Time.deltaTime);
            //Debug.Log("CurrentCD: " + currentCD_);
        }
        else if (abilityUp)
            CmdSetCD(coolDown);
    }
    //Override this method
    protected virtual void UseAbility()
    {
        //Debug.Log("Habilidad usada");
    }

    //Show the ability template
    protected virtual bool PrepareAbility()
    {
        //Debug.Log("Lo estoy enseñando");
        return true;
    }

    protected virtual void cancelAbility()
    {

    }

    protected virtual void SetAbilityUp()
    {
        abilityUp = true;
    }

    public float getCurrentCD()
    {
        return currentCD_;
    }

    [Command]
    protected void CmdSubstractCD(float t)
    {
        currentCD_ -= t;
    }

    [Command]
    protected void CmdSetCD(float t)
    {
        currentCD_ = t;
    }



    // Cambia el layer del objeto dentro del juego de cada cliente segun corresponda
    [ClientRpc]
    protected void RpcChangeBulletLayer(GameObject obj)
    {
        obj.layer = gameObject.layer;
    }
}
