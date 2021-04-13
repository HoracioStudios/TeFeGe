using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacter : MonoBehaviour
{
    public GameObject HealthBar;
    public GameObject AmmoBar;
    public SpriteRenderer AbilityBar;
    public SpriteRenderer LifeBar;
    public SpriteRenderer BalasBar;
    public Sprite[] AbilityBarSprites;
    public Sprite[] AmmoBarSprites;
    public Sprite[] LifeBarSprites;
    public FMODUnity.StudioEventEmitter emitter;

    public Abilities abilities;

    float currentHealth;
    float maxHealth;
    float currentAmmo;
    float maxAmmo;

    float fillHealth;
    float fillAmmo;
    float fillAbility;

    public bool isChuerk = false;

    // Start is called before the first frame update
    void Start()
    {

        //if (emitter)
        //    emitter.Play();

        maxHealth = gameObject.transform.parent.GetComponent<health>().maxHealth;
        //Para que sea automatico hace falta uno estandar
        maxAmmo = gameObject.transform.parent.GetComponent<normalShoot>().getMaxBullets();
    }

    // Update is called once per frame
    void Update()
    {

        //Actualizacion de valores
        currentHealth = gameObject.transform.parent.GetComponent<health>().getCurrentHealth();
        //Para que sea automatico hace falta uno estandar
        currentAmmo = gameObject.transform.parent.GetComponent<normalShoot>().getCurrentBullets();

        fillHealth = currentHealth / maxHealth;
        fillAmmo = currentAmmo / maxAmmo;
        if (!isChuerk)
            fillAbility = abilities.getCurrentCD() / abilities.coolDown;
        else
            fillAbility = abilities.gameObject.GetComponent<AbilityChuerk>().getGas() / 0.8f;


        //Actualización de barras
        changeColorHealth(fillHealth);

        //emitter.SetParameter("Health", fillHealth * 100);

        if (fillHealth >= 0 && fillHealth <= 1)
            HealthBar.transform.localScale = new Vector3(fillHealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);

        if (fillAmmo >= 0 && fillAmmo <= 1)
            AmmoBar.transform.localScale = new Vector3(fillAmmo, AmmoBar.transform.localScale.y, AmmoBar.transform.localScale.z);

        int barHP = (int)(fillHealth * 10);
        if (barHP < 0) barHP = 0;
        else if (barHP >= LifeBarSprites.Length) barHP = LifeBarSprites.Length - 1;

        int barAmmo = (int)(fillAmmo * 10);
        if (barAmmo < 0) barAmmo = 0;
        else if (barAmmo >= AmmoBarSprites.Length) barAmmo = AmmoBarSprites.Length - 1;

        int barAbility = (int)(fillAbility * 8);
        if (barAbility < 0) barAbility = 0;
        else if (barAbility >= AbilityBarSprites.Length) barAbility = AbilityBarSprites.Length - 1;

        LifeBar.sprite = LifeBarSprites[barHP];
        BalasBar.sprite = AmmoBarSprites[barAmmo];
        AbilityBar.sprite = AbilityBarSprites[barAbility];
    }

    void changeColorHealth(float fill_)
    {
        if (fill_ >= 0.5)
        {
            HealthBar.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 100);
        }
        else if (fill_ > 0.25 && fill_ < 0.5)
        {
            HealthBar.GetComponent<SpriteRenderer>().color = new Color(255, 255, 0, 100);
        }
        else if (fill_ <= 0.25)
        {
            HealthBar.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 100);
        }
    }
}
