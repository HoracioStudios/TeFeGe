using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Turbocuesco : NetworkBehaviour
{
    public float despawnTime;

    float actualCD;
    string damageTag;

    private void Update()
    {
        if (actualCD < despawnTime)
            actualCD += Time.deltaTime;
        else
            NetworkServer.Destroy(gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == damageTag)
        {
            other.GetComponent<health>().TakeDamage(0.1f);
        }
    }
    public void setTag(string tag)
    {
        damageTag = tag == "ATeam" ? "BTeam" : "ATeam";
    }
}
