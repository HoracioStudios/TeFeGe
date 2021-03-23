using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public float coolDown;
    protected float currentCD_;
    public GameObject template;

    public bool abilityUp = true;
    protected bool preparing_ = false;
    private StateMachine states_;

    protected FMODUnity.StudioEventEmitter emitter;

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
        else if(abilityUp && !preparing_ && (Input.GetAxis("FireAbility") != 0 /*|| Input.GetAxis("FireAbility_Joy") != 0*/))
        {
            preparing_ = PrepareAbility();
        }

        if(preparing_ && (Input.GetAxis("FireAbility") == 0 /*|| Input.GetAxis("FireAbility_Joy") == 0*/))
        {
            UseAbility();
            currentCD_ = 0.0f;
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
            if (currentCD_ < coolDown)
                currentCD_ += Time.deltaTime;
            //Debug.Log("CurrentCD: " + currentCD_);
        }
        else if (abilityUp)
            currentCD_ = coolDown;
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

    protected void SetAbilityUp()
    {
        abilityUp = true;
    }

    public float getCurrentCD()
    {
        return currentCD_;
    }
}
