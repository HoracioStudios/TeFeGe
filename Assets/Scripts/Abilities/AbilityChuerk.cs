using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AbilityChuerk : Abilities
{
    public GameObject cuesco;

    [SyncVar]
    float gas = 0.4f;
    float cooldown = 0.04f;
    float speed_;


    //protected FMODUnity.StudioEventEmitter emitter;

    float actualCD = 0.0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        emitter = gameObject.GetComponent<FMODUnity.StudioEventEmitter>();
        speed_ = GetComponent<basicMovement3D>().speed;

        base.Start();
    }

    public void FartOff()
    {
        emitter.SetParameter("IsFart", 1);
    }

    // Update is called once per frame
    protected override void Update()
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

        GetComponent<basicMovement3D>().speed = (((Input.GetAxis("FireAbility") != 0 && !GameManager.instance.isControllerMode) || (Input.GetAxis("FireAbility_Joy") != 0 && GameManager.instance.isControllerMode))) && gas > 0.0f ? speed_ + 5f : speed_;        

        if (((Input.GetAxis("FireAbility") != 0 && !GameManager.instance.isControllerMode) || (Input.GetAxis("FireAbility_Joy") != 0 && GameManager.instance.isControllerMode)))
        {
            if (gas > 0.0f && !emitter.IsPlaying())
            {
                if(!emitter.IsPlaying())
                    GameManager.instance.StartVibration(0.1f);
                emitter.SetParameter("IsFart", 0);
                emitter.Play();
            }
            else if (gas <= 0.0f)
            {
                emitter.SetParameter("IsFart", 1);
            }

            CmdAddGas(-Time.deltaTime);
            //CmdSubstractCD(Time.deltaTime);
            actualCD += Time.deltaTime;
            if (actualCD > cooldown && gas > 0.0f)
            {
                CmdSpawnCuesco();
                actualCD = 0.0f;
                //emitter.SetParameter("IsFart", 1);
            }
        }
        else
        {
            emitter.SetParameter("IsFart", 1);

            if (gas < 0.8f)
            {
                CmdAddGas(Time.deltaTime);
            }
        }
    }

    [Command]
    private void CmdSpawnCuesco()
    {
        GameObject newCuesco = Instantiate(cuesco);
        newCuesco.transform.position = gameObject.transform.position;
        newCuesco.layer = gameObject.layer;

        NetworkServer.Spawn(newCuesco);
        RpcSetTag(newCuesco);
    }

    [ClientRpc]
    void RpcSetTag(GameObject newCuesco)
    {
        newCuesco.GetComponent<Turbocuesco>().setTag(gameObject.tag);
    }

    [Command]
    private void CmdAddGas(float g)
    {
        gas += g;
        if (gas < 0.0f)
            gas = 0.0f;
    }

    public float getGas() { return gas; }

    private void OnDestroy()
    {
        emitter.Stop();
    }
}
