using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class badBabyShoot : normalShoot
{
    public float distShield; //Distance from the character to the shield
    public float speedRot; //Speed the bullets rotate
    public GameObject shield;

    public float phase = 0.0f;
    public float phaseSpeed = -20f;

    public List<GameObject> shieldNotes;
    public List<GameObject> shieldGone;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        createShield();
    }


    protected override void Update()
    {
        //Rotate the spawn point to rotate the bullets too
        //spawn.Rotate(new Vector3(0.0f, 1.0f, 0.0f)*speedRot * Time.deltaTime, Space.World);

        phase = (phase + (phaseSpeed * Time.deltaTime)) % 360.0f;

        checkShield();

        updateShield();

        base.Update(); //Se esta llamando al reload mazo por algo, miralo anda, que pasan cosas raras
    }

    public override void Shoot()
    {
        base.Shoot();
        
        GameObject obj = shieldNotes[0];

        obj.SetActive(false);

        shieldGone.Add(obj);
        shieldNotes.RemoveAt(0);

        updateShield();
    }

    //updates bullets that rotate around the player
    private void updateShield()
    {
        for (int i = 0; i < shieldNotes.Count; i++)
        {
            GameObject obj = shieldNotes[i];

            if (!obj.active)
            {
                shieldGone.Add(obj);
                shieldNotes.Remove(obj);
                i--;
            }
        }
        if (!reloading)
        {
            for (int i = 0; i < (int)actualBullets; i++)
            {
                shieldNotes[i].transform.position = spawn.position + Rotate(new Vector3(1.0f, 0.0f, 1.0f), 360 * ((float)i / (float)actualBullets) + phase).normalized * distShield;
            }
        }
    }

    //updates bullets that rotate around the player
    private void resetShield()
    {
        foreach (GameObject obj in shieldGone)
        {
            obj.SetActive(true);
            shieldNotes.Add(obj);
        }

        shieldGone.Clear();
    }

    //Creates the bullets that rotate around the player
    private void createShield()
    {
        if (reloading)
        {
            reloading = false;
        }
        for (int i = 0; i < actualBullets; i++)
        {
            GameObject obj = Instantiate(shield, spawn.position, Quaternion.identity);
            obj.transform.SetParent(spawn); //Set the bullets as a child from the spawn point
            obj.transform.localPosition = Vector3.zero;
            obj.transform.Translate(Rotate(new Vector3(1.0f, 0.0f, 1.0f), 360*((float)i/(float)actualBullets)).normalized*distShield);
            obj.layer = gameObject.layer;

            shieldNotes.Add(obj);
        }
    }

    //Return the number of bullets you have
    private void checkShield()
    {
        if (!reloading && actualBullets > GameObject.FindGameObjectsWithTag("BBShield").Length)
        {
            actualBullets = GameObject.FindGameObjectsWithTag("BBShield").Length;
            //createShield();
        }
    }

    protected override void Reload()
    {
        base.Reload();
        Invoke("resetShield", reloadTime);
    }

    public void killShield()
    {
        actualBullets--;
        //resetShield();
        //createShield();

        GameObject obj = shieldNotes[0];

        obj.SetActive(false);

        shieldGone.Add(obj);
        shieldNotes.RemoveAt(0);


        updateShield();
    }


}
