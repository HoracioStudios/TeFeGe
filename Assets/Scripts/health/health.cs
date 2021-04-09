using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class health : NetworkBehaviour
{
    public float maxHealth;

    [SyncVar]
    public float currentHealth;

    public Vector3 init;

    public FMODUnity.StudioEventEmitter respawnEmitter;

    bool block = false;

    void Start()
    {
        currentHealth = maxHealth;
        init = transform.position;
    }

    public virtual void TakeDamage(float dmg)
    {
        double hp = currentHealth;

        CmdTakeDamage(dmg);
        currentHealth -= dmg;

        if (hp <= 0 && !block)
        {
            block = true;

            //Respawn o eliminar el objeto
            transform.position = init;

            CmdResetHP();
            currentHealth = maxHealth;

            CmdHasDied();

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
    private void CmdHasDied()
    {
        GameManager.instance.roundManager.ServerRoundEnd();
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
