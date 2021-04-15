using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AbilityMolotov : Abilities
{
    [Header("Spawneable Objects")]
    public GameObject molotov;
    public GameObject targetM;

    [Header("Reference Points")]
    public gunRotation gunRotation;

    [Header("Parameters")]
    public float distance = 0;

    Vector3 mPos;
    Vector3 gPos;

    private GameObject target;


    protected override void Update()
    {

        if (abilityUp && Input.GetMouseButton(1))
        {
            PrepareAbility();
            preparing_ = true;
        }

        if (preparing_ && Input.GetMouseButtonUp(1))
        {
            UseAbility();
            abilityUp = false;
            CmdSetCD(0.0f);
            preparing_ = false;
            Invoke("SetAbilityUp", coolDown); //Puede que se necesite el timer para dar el porcentaje
        }

        updateCD();
    }

    protected override void UseAbility()
    {
        if (emitter)
            emitter.Play();

        CmdSpawnMolotov(gameObject.tag, target.transform.position);
        Destroy(target);
    }

    [Command]
    private void CmdSpawnMolotov(string tag, Vector3 targetPos)
    {
        Vector3 pos = transform.position;
        pos.y += 2.0f;
        GameObject obj = Instantiate(molotov, pos, transform.rotation);
        obj.GetComponent<Rigidbody>().velocity = targetPos - obj.transform.position;
        obj.GetComponent<Rigidbody>().velocity += new Vector3(0.0f, distance * 2, 0.0f);
        obj.layer = gameObject.layer;
        obj.tag = tag;
        NetworkServer.Spawn(obj);
        RpcSetTag(obj);
    }

    [ClientRpc]
    private void RpcSetTag(GameObject obj)
    {
        obj.layer = gameObject.layer;
        obj.tag = tag;
    }

    //Show the ability template
    protected override bool PrepareAbility()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var layerMask = 1 << LayerMask.NameToLayer("Ground"); // No se si vale para algo

        if (Physics.Raycast(ray, out hit, 100, layerMask)) //Los dos ultimos parámetros son para solo pillar Ground como capa, 100 arbitrario
        {
            Transform objectHit = hit.transform;

            gPos = new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z);
            
            mPos = new Vector3(hit.point.x, 0.0f, hit.point.z);


            Vector3 diff = mPos - gPos;
            float dist = diff.magnitude;

            if (dist >= distance)
                dist = distance;

            Ray test = new Ray(transform.position, diff.normalized);

            RaycastHit hitTest;


            if (!Physics.Raycast(test, out hitTest, dist) || hitTest.transform.tag != "Wall") {
                if (dist < distance)
                {

                    if (target != null)
                    {
                        target.transform.position = new Vector3(mPos.x, hit.point.y, mPos.z);
                    }

                    else
                    {
                        target = Instantiate(targetM, new Vector3(mPos.x, hit.point.y, mPos.z), new Quaternion(0, 0, 0, 0));
                        target.transform.Rotate(new Vector3(90, 0, 0));
                    }
                }

                else
                {
                    if (target == null)
                    {
                        target = Instantiate(targetM, new Vector3(gPos.x, hit.point.y, gPos.z) + diff.normalized * distance, new Quaternion(0, 0, 0, 0));
                        target.transform.Rotate(new Vector3(90, 0, 0));
                    }

                    else
                    {
                        target.transform.position = new Vector3(gPos.x, hit.point.y, gPos.z) + diff.normalized * distance;
                    }
                }
            }

            else
            {
                //Debug.Log(hitTest.point);
                diff = hitTest.point;
                diff.y = 0.0f;
                diff -= gPos;
                dist = diff.magnitude;

                if (target == null)
                {
                    target = Instantiate(targetM, new Vector3(gPos.x, hit.transform.position.y, gPos.z) + diff.normalized * dist, new Quaternion(0, 0, 0, 0));
                    target.transform.Rotate(new Vector3(90, 0, 0));
                }

                else
                {
                    target.transform.position = new Vector3(gPos.x, hit.transform.position.y, gPos.z) + diff.normalized * dist;
                }
            }
        }

        return true;
    }
}
