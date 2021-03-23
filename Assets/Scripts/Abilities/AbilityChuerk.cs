using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityChuerk : Abilities
{
    public GameObject cuesco;
    float gas = 0.4f;
    float cooldown = 0.04f;
    float speed_;


    protected FMODUnity.StudioEventEmitter emitter;

    float actualCD = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        emitter = gameObject.GetComponent<FMODUnity.StudioEventEmitter>();
        speed_ = GetComponent<basicMovement3D>().speed;
    }

    public void FartOff()
    {
        emitter.SetParameter("IsFart", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!emitter.IsPlaying())
            emitter.Stop();

        if (gas <= 0.0f)
        {
            gas = 0.0f;
            abilityUp = false;
        }
        else
            abilityUp = true;

        GetComponent<basicMovement3D>().speed = Input.GetMouseButton(1) && gas > 0.0f ? speed_ + 5f : speed_;        

        if (Input.GetMouseButton(1))
        {
            if (gas > 0.0f && !emitter.IsPlaying())
            {
                emitter.SetParameter("IsFart", 0);
                emitter.Play();
            }
            else if (gas <= 0.0f)
            {
                emitter.SetParameter("IsFart", 1);
            }

            gas -= Time.deltaTime;
            actualCD += Time.deltaTime;
            if (actualCD > cooldown && gas > 0.0f)
            {
                GameObject newCuesco = GameObject.Instantiate(cuesco);
                newCuesco.transform.position = this.transform.position;
                newCuesco.GetComponent<Turbocuesco>().setTag(transform.tag);
                actualCD = 0.0f;
                //emitter.SetParameter("IsFart", 1);
            }
        }
        else
        {
            emitter.SetParameter("IsFart", 1);

            if (gas < 0.8f)
            {
                gas += Time.deltaTime;
            }
        }
    }
}
