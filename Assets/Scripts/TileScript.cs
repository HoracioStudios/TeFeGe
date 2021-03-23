using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public int character;
    public bool hp = false;

    public FMODUnity.StudioEventEmitter soundEmitter;

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
        if (other.gameObject.tag == "ATeam")
        {
            if (!hp && !other.gameObject.transform.parent.GetChild(character).gameObject.active)
            {
                Transform t = other.gameObject.transform;

                if (soundEmitter)
                    soundEmitter.Play();

                for (int i = 0; i < 5; i++)
                {
                    if (i == character)
                    {
                        other.gameObject.transform.parent.GetChild(character).gameObject.SetActive(true);

                        MusicHandler music = other.gameObject.transform.parent.GetChild(character).gameObject.GetComponent<MusicHandler>();
                        if (music)
                            music.Play();

                        other.gameObject.transform.parent.GetChild(character).localPosition = t.localPosition;
                    }
                    else
                    {
                        if(i == 0 && other.gameObject.transform.parent.GetChild(i).gameObject.active)
                            other.gameObject.transform.parent.GetChild(i).gameObject.GetComponent<AbilityChuerk>().FartOff();

                        MusicHandler music = other.gameObject.transform.parent.GetChild(i).gameObject.GetComponent<MusicHandler>();
                        if (music)
                            music.Stop();

                        other.gameObject.transform.parent.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            else if (hp)
            {
                if (soundEmitter)
                    soundEmitter.Play();

                health hp = other.gameObject.GetComponent<health>();
                if (hp.currentHealth < hp.maxHealth)
                    hp.currentHealth = (hp.currentHealth + Time.deltaTime * 5 > hp.maxHealth) ? hp.maxHealth : hp.currentHealth + Time.deltaTime * 5;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ATeam")
        {
            if(hp)
            {
                if (soundEmitter && !soundEmitter.IsPlaying())
                    soundEmitter.Play();

                health hp = other.gameObject.GetComponent<health>();
                if (hp.currentHealth < hp.maxHealth)
                    hp.currentHealth = (hp.currentHealth + Time.deltaTime * 5 > hp.maxHealth) ? hp.maxHealth : hp.currentHealth + Time.deltaTime * 5;
            }
        }
    }
}
