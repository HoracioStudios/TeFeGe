using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class badBabyShoot : normalShoot
{
    [Header("Shield Parameters")]
    public GameObject shield;
    public float distShield; //Distance from the character to the shield
    public float speedRot; //Speed the bullets rotate

    [Header("Shield Staff")]
    public List<GameObject> shieldNotes;
    public List<GameObject> shieldGone;

    [Header("Debug Parameters")]
    public float phase = 0.0f;
    public float phaseSpeed = -20f;

    private bool startUpdate = false;

    private void Awake()
    {
        // Si se hace esto las balas atraviesan. Si se quita el if, hay problemas con el 
        // cliente que spawnee primero
        if(!isLocalPlayer)
            BadBabyShieldBehaviour.ShieldCollision += killShield;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (!isLocalPlayer) return;
        createShield();
    }


    protected override void Update()
    {
        if (!isLocalPlayer) return;

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

        CmdSetActive(obj, false);
        obj.SetActive(false);

        shieldGone.Add(obj);
        shieldNotes.RemoveAt(0);

        updateShield();
    }

    //updates bullets that rotate around the player
    private void updateShield()
    {
        UpdateVectors();
        if (!reloading && startUpdate)
        {
            for (int i = 0; i < (int)actualBullets; i++)
            {
                shieldNotes[i].transform.position = spawn.position + Rotate(new Vector3(1.0f, 0.0f, 1.0f), 360 * ((float)i / (float)actualBullets) + phase).normalized * distShield;
                CmdMoveShield(shieldNotes[i], spawn.position + Rotate(new Vector3(1.0f, 0.0f, 1.0f), 360 * ((float)i / (float)actualBullets) + phase).normalized * distShield);
            }
        }
    }

    private void UpdateVectors()
    {
        for (int i = 0; i < shieldNotes.Count; i++)
        {
            GameObject obj = shieldNotes[i];

            if (!obj.activeSelf)
            {
                shieldGone.Add(obj);
                shieldNotes.Remove(obj);
                i--;
            }
        }
    }

    [Command]
    private void CmdMoveShield(GameObject obj, Vector3 pos)
    {
        obj.transform.position = pos;
    }

    //updates bullets that rotate around the player
    private void resetShield()
    {
        foreach (GameObject obj in shieldGone)
        {
            CmdSetActive(obj, true);
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
        shieldNotes = new List<GameObject>();
        shieldGone = new List<GameObject>();
        CmdCreateShield(gameObject.layer);
    }

    [Command]
    private void CmdSetActive(GameObject obj, bool state)
    {
        obj.SetActive(state);
        RpcSetActive(obj, state);
    }

    [ClientRpc]
    private void RpcSetActive(GameObject obj, bool state)
    {
        obj.SetActive(state);
    }

    [Command]
    private void CmdCreateShield(int layer)
    {
        for (int i = 0; i < actualBullets; i++)
        {
            GameObject obj = Instantiate(shield, transform.position, Quaternion.identity);
            //obj.transform.SetParent(spawn); //Set the bullets as a child from the spawn point
            obj.transform.localPosition = Vector3.zero;
            obj.transform.Translate(Rotate(new Vector3(1.0f, 0.0f, 1.0f), 360 * ((float)i / (float)actualBullets)).normalized * distShield);
            obj.layer = layer;
            NetworkServer.Spawn(obj);

            AddToShield(obj, layer);
        }
    }



    [ClientRpc]
    private void AddToShield(GameObject obj, int layer)
    {
        //obj.transform.SetParent(spawn); //Set the bullets as a child from the spawn point
        obj.layer = gameObject.layer;
        shieldNotes.Add(obj);
        startUpdate = true;
    }

    //Return the number of bullets you have
    private void checkShield()
    {
        if (!reloading && actualBullets > GameObject.FindGameObjectsWithTag("BBShield").Length)
        {
            //actualBullets = GameObject.FindGameObjectsWithTag("BBShield").Length;
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
        if (isLocalPlayer)
        {
            actualBullets--;
            CmdSubstractShield(1);
            UpdateVectors();
            CmdKillShield();
        }      
    }

    [Command]
    private void CmdSubstractShield(float b)
    {
        actualBullets -= b;
    }

    [Command]
    private void CmdKillShield()
    {
        RpcKillShield();
    }

    [ClientRpc]
    private void RpcKillShield()
    {
        if (isLocalPlayer)
        {            
            GameObject obj = shieldNotes[0];

            CmdSetActive(obj, false);
            obj.SetActive(false);

            shieldGone.Add(obj);
            shieldNotes.RemoveAt(0);
            updateShield();
        }
    }

}
