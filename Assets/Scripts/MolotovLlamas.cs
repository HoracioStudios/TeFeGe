using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MolotovLlamas : NetworkBehaviour
{
    //Esto es una burda imitación de Turbocuesco.cs, a falta de temas de balanceado

    public float despawnTime;
    float actualCD = 0;
    public float damagePerTick = 0.1f;
    string damageTag;

    public FMODUnity.StudioEventEmitter emitter;

    // Start is called before the first frame update
    void Start()
    {
        setTag();
        if (emitter)
            emitter.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (actualCD < despawnTime)
            actualCD += Time.deltaTime;
        else
        {
            emitter.SetParameter("FadeOut", 1);
            Destroy();
        }
    }

    private void Destroy()
    {
        NetworkServer.Destroy(gameObject);
    }

    void OnTriggerStay(Collider other)
    {

        if (other.tag == damageTag)
        {
            other.GetComponent<health>().TakeDamage(damagePerTick);
        }
    }

    public void setTag()
    {
        damageTag = (gameObject.tag == "ATeam") ? "BTeam" : "ATeam";
    }
}
