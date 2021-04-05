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

    void Start()
    {
        currentHealth = maxHealth;
        init = transform.position;
    }

    public virtual void TakeDamage(float dmg)
    {
        CmdTakeDamage(dmg);
        if (currentHealth <= 0)
        {
            //Respawn o eliminar el objeto
            transform.position = init;

            CmdResetHP();
            //currentHealth = maxHealth;

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

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public void kill()
    {
        TakeDamage(currentHealth);
    }
}
