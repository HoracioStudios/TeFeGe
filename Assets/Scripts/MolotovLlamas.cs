using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovLlamas : MonoBehaviour
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
            Destroy(this.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        //other.GetComponent<health>().takeDamage(damagePerTick); //Para testear

        if (other.tag == damageTag)
        {
            other.GetComponent<health>().takeDamage(damagePerTick);
        }
    }

    public void setTag()
    {
        damageTag = (tag == "ATeam") ? "BTeam" : "ATeam";
    }
}
