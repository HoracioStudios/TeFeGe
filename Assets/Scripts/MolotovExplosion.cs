using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovExplosion : MonoBehaviour
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

            GameObject obj = Instantiate(llamas, pos, llamas.transform.rotation);

            obj.tag = tag;

            Destroy(this.gameObject);

            //Debug.Log(other.name);

            if (other.tag != "Wall" && other.tag != "Ground")
            {

            }
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.back, 1000f * Time.deltaTime);
    }
}
