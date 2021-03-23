using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTileScript : MonoBehaviour
{
    bool activated = true;
    public Sprite active;
    public Sprite notActive;
    public GameObject turret;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = notActive;
            turret.GetComponent<TurretRotation>().enabled = false;
            turret.GetComponent<turretShoot>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = active;
            turret.GetComponent<TurretRotation>().enabled = true;
            turret.GetComponent<turretShoot>().enabled = true;
        }

        activated = !activated;
    }
}
