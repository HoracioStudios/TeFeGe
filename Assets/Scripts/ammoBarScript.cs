using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ammoBarScript : MonoBehaviour
{
    public Image AmmoBar;
    public float currentAmmo;
    public float maxAmmo;
    private float fill;
    public GameObject gun;
    private normalShoot gunShoot;

    // Start is called before the first frame update
    void Start()
    {
        gunShoot = gun.GetComponent<normalShoot>();
        maxAmmo = gunShoot.getMaxBullets();
    }

    // Update is called once per frame
    void Update()
    {
        currentAmmo = gunShoot.getCurrentBullets();

        fill = currentAmmo / maxAmmo;
        AmmoBar.fillAmount = fill;
    }
}
