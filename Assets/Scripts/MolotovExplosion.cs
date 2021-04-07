using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MolotovExplosion : NetworkBehaviour
{
    public GameObject llamas;
    public float delay = 1f;

    float Y = 0.0f;

    public FMODUnity.StudioEventEmitter emitter;

    private void Awake()
    {
        Y = transform.position.y - 0.2f;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag != "Ignore")
        {
            Vector3 pos = transform.position;
            pos.y = Y;

            if (emitter)
                emitter.Play();

            CmdCreateFire(pos);
            CmdDestroy();

            //Debug.Log(other.name);

            if (other.tag != "Wall" && other.tag != "Ground")
            {

            }
        }
    }

    [Command]
    private void CmdCreateFire(Vector3 pos)
    {
        GameObject obj = Instantiate(llamas, pos, llamas.transform.rotation);
        obj.tag = tag;
        NetworkServer.Spawn(obj);
        RpcSetTag(obj);
    }

    [Command]
    private void CmdDestroy()
    {
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    private void RpcSetTag(GameObject obj)
    {
        obj.tag = tag;
    }


    private void Update()
    {
        transform.Rotate(Vector3.back, 1000f * Time.deltaTime);
    }
}
