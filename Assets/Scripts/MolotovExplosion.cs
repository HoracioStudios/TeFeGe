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
            pos.y = 0.2f;

            if (emitter)
                emitter.Play();

            CreateFire(pos);
            Destroy();

            if (other.tag != "Wall" && other.tag != "Ground")
            {

            }
        }
    }

    private void CreateFire(Vector3 pos)
    {
        GameObject obj = Instantiate(llamas, pos, llamas.transform.rotation);
        obj.tag = gameObject.tag;
        obj.layer = gameObject.layer;
    }

    private void Destroy()
    {
        NetworkServer.Destroy(gameObject);
    }    

    private void Update()
    {
        transform.Rotate(Vector3.back, 1000f * Time.deltaTime);
    }
}