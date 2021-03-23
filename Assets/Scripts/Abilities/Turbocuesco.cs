using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbocuesco : MonoBehaviour
{
    public float despawnTime;
    float actualCD;
    string damageTag;

    private void Update()
    {
        if (actualCD < despawnTime)
            actualCD += Time.deltaTime;
        else
            Destroy(this.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == damageTag)
        {
            other.GetComponent<health>().takeDamage(0.1f);
        }
    }
    public void setTag(string tag)
    {
        damageTag = tag == "ATeam" ? "BTeam" : "ATeam";
    }
}
