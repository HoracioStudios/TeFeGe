using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMolotov : Abilities
{
    public GameObject molotov;
    public GameObject targetM;
    public float distance = 0;

    Vector3 mPos;
    Vector3 gPos;

    private GameObject target;


    // Update is called once per frame
    void Update()
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
            currentCD_ = 0.0f;
            preparing_ = false;
            Invoke("SetAbilityUp", coolDown); //Puede que se necesite el timer para dar el porcentaje
        }

        updateCD();
    }

    protected void UseAbility()
    {
        if (emitter)
            emitter.Play();

        GameObject gun = gameObject.GetComponentInChildren<gunRotation>().gameObject;

        GameObject obj = Instantiate(molotov, gun.transform.position, transform.rotation);
        obj.layer = gameObject.layer;
        obj.GetComponent<Rigidbody>().velocity = target.transform.position - obj.transform.position;
        obj.GetComponent<Rigidbody>().velocity += new Vector3(0.0f, distance * 2, 0.0f);
        obj.tag = tag;

        //Debug.Log(obj.GetComponent<Rigidbody>().velocity);

        Destroy(target);
    }

    protected void SetAbilityUp()
    {
        abilityUp = true;
    }

    //Show the ability template
    protected bool PrepareAbility()
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
