using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class health : NetworkBehaviour
{
    public float maxHealth;

    [SyncVar(hook = nameof(healthUpdate))]
    public float currentHealth;

    public Vector3 init;

    public FMODUnity.StudioEventEmitter respawnEmitter;

    bool block = false;

    void Start()
    {
        currentHealth = maxHealth;
        //CmdResetHP();
        init = transform.position;
    }

    private void Update()
    {
        /*
        if (isClient && Input.GetKeyDown(KeyCode.M))
            TakeDamage(5);
            */
    }

    public virtual void TakeDamage(float dmg)
    {
        if (isLocalPlayer && !block)
            CmdTakeDamage(dmg);
        else if(!block)
            GameManager.instance.gameData.dmgDealt += dmg;
    }

    private void healthUpdate(float oldHealth, float newHealth)
    {
        if (newHealth <= 0 && !block)
        {
            if (gameObject.tag == "Bullet")
            {
                NetworkServer.Destroy(gameObject);
            }

            block = true;

            //Respawn o eliminar el objeto
            //transform.position = init;

            //CmdResetHP();
            //currentHealth = maxHealth;

            Debug.Log("TriggerRoundEnd llamado  isLocalPlayer: " + isLocalPlayer);
            if (gameObject.tag == "ATeam" || gameObject.tag == "BTeam")
                RoundManager.instance.TriggerRoundEnd(isLocalPlayer);

            if (respawnEmitter)
                respawnEmitter.Play();

            //Destroy(gameObject);
        }
    }

    [Command]
    private void CmdResetHP()
    {
        currentHealth = maxHealth;
    }

    [Command]
    private void CmdTakeDamage(float dmg)
    {
        currentHealth -= dmg;
    }

    [Command]
    private void CmdHasDied(bool localPlayer)
    {
        RoundManager.instance.ServerRoundEnd(localPlayer);
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public void kill()
    {
        TakeDamage(currentHealth);
    }
}
