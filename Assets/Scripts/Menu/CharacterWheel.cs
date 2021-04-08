using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWheel : MonoBehaviour
{
    int children;
    float degrees;

    //Posicion inicial
    float startAngle = -90;
    float degreeToRad = Mathf.PI / 180;
    float radius = 3.25f;

    //Control
    int characterSelected = 1;

    //Rotacion
    public float lerpTime = 0.1f;
    float t = 0.0f;
    public bool rotating = false;

    Vector3 startRot;
    Vector3 endRot;

    private void Start()
    {
        children = transform.childCount;
        degrees = 360.0f / children;

        for (int i = 0; i < children; i++)
        {
            transform.GetChild(i).position = transform.position + (new Vector3(radius * Mathf.Cos((startAngle + degrees * i) * degreeToRad), radius * Mathf.Sin((startAngle + degrees * i) * degreeToRad), 0));
        }
    }

    private void Update()
    {
        if (rotating)
        {
            if (t < lerpTime)
            {
                t += Time.deltaTime;

                transform.eulerAngles = Vector3.Lerp(startRot, endRot, t / lerpTime);

                int pivotChildren = transform.childCount;

                for (int i = 0; i < pivotChildren; i++)
                {
                    transform.GetChild(i).SetPositionAndRotation(transform.GetChild(i).position, Quaternion.identity);
                }
            }
            else
            {
                rotating = false;
                t = 0.0f;
                Debug.Log("eee");

                if (characterSelected > 0)
                    characterSelected++;
                else
                    characterSelected = transform.childCount;
            }
        }
    }

    public void startLerp(bool leftDir)
    {
        rotating = true;

        Vector3 mod = new Vector3(0, 0, 360.0f / (float)transform.childCount);

        if (!leftDir) mod = -mod;

        startRot = transform.eulerAngles;
        endRot = startRot + mod;
    }

    public void UpdateCharacterSelected()
    {
        switch (characterSelected)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }
}
