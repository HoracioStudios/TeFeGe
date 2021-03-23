using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public Vector3 init;

    public FMODUnity.StudioEventEmitter respawnEmitter;

    void Start()
    {
        currentHealth = maxHealth;
        init = transform.position;
    }

    public virtual void takeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            transform.position = init;

            currentHealth = maxHealth;

            if (respawnEmitter)
                respawnEmitter.Play();

            //Destroy(gameObject);

        }
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public void kill()
    {
        takeDamage(currentHealth);
    }
}
