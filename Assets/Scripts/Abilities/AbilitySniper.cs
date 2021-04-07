using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AbilitySniper : Abilities
{
    public GameObject bullet;
    public Transform spawnPoint;
    public float speedBullet;
    public float multiplier;
    public Camera cam_;
    public basicMovement3D bob;

    private float camSize_;
    private normalShoot shootBehaviour_;

    //public FMODUnity.StudioEventEmitter chargeSound;
    //protected FMODUnity.StudioEventEmitter emitter;

    bool charged = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        camSize_ = cam_.orthographicSize;
        shootBehaviour_ = gameObject.GetComponent<normalShoot>();
       
        foreach (FMODUnity.StudioEventEmitter em in gameObject.GetComponents<FMODUnity.StudioEventEmitter>())
        {
            if (em.Event == "event:/Abilities/ShootingAbilities")
            {
                emitter = em;
            }
            else
            {
                //Debug.Log(em.Event);
            }
        }

        base.Start();
    }

    protected override bool PrepareAbility()
    {
        if (!charged)
        {
            shootBehaviour_.SetBlockShoot(true);

            template.SetActive(true);

            StartCoroutine(resizeRoutine(cam_.orthographicSize, 6f, 1));

            //View template
            bob.speed /= 2;
            base.PrepareAbility();

            if (emitter)
                emitter.Play();
        }
        return true;
    }

    protected override void UseAbility()
    {
        if (charged)
        {
            StartCoroutine(resizeRoutine(cam_.orthographicSize, 3.5f, 1));
            //cam_.orthographicSize = camSize_;            

            if (emitter)
                emitter.Play();

            //Shoot from the server
            CmdUseAbility(gunRot.getGunDir());

            //Set everything false
            base.UseAbility();

            shootBehaviour_.SetBlockShoot(false);

            template.SetActive(false);
            bob.speed *= 2;
        }
    }

    private IEnumerator resizeRoutine(float oldSize, float newSize, float time)
    {
        float elapsed = 0;
        while (elapsed <= time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);

            cam_.orthographicSize = Mathf.Lerp(oldSize, newSize, t);
            yield return null;
        }

        yield return null;
        Debug.Log("Charged");
        charged = !charged;
    }

    [Command]
    private void CmdUseAbility(Vector3 gunDir)
    {
        //Shoot the bullet
        GameObject obj = Instantiate(bullet, spawnPoint.position, transform.rotation);
        obj.GetComponent<Rigidbody>().velocity = gunDir * speedBullet;
        obj.transform.rotation = Quaternion.LookRotation(obj.GetComponent<Rigidbody>().velocity, Vector3.up);
        obj.transform.rotation *= Quaternion.Euler(90, -90, 0);

        obj.layer = gameObject.layer;

        NetworkServer.Spawn(obj);
        RpcChangeAbilityLayer(obj);
    }

    [ClientRpc]
    protected void RpcChangeAbilityLayer(GameObject obj)
    {
        obj.layer = gameObject.layer;
    }
}
