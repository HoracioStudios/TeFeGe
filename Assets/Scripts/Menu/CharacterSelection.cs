using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterSelection : MonoBehaviour
{
    public GameObject toRotate;
    int children;
    int degrees;
    Button left;
    Button right;

    //Posicion inicial
    int startAngle = -90;
    float startPosX = -4.25f;
    float startPosY = -0.0f;
    float degreeToRad = Mathf.PI / 180;
    float radius = 3.25f;

    //Control
    int characterSelected = 1;

    //Rotacion
    float speed = 0.1f;
    float t = 0.0f;
    bool rotating = false;

    private void Start()
    {
        children = toRotate.transform.childCount;
        degrees = 360 / children;

        for (int i = 0; i < children; i++)
        {
            toRotate.transform.GetChild(i).position = new Vector3(startPosX + radius * Mathf.Cos((startAngle + degrees * i) * degreeToRad), startPosY + radius * Mathf.Sin((startAngle + degrees * i) * degreeToRad), 0);
        }
    }

    private void Update()
    {
        if (rotating)
        {
            if (t < 1.0f)
                t += Time.deltaTime;
            else
            {
                rotating = false;
                t = 0.0f;
                Debug.Log("eee");
            }
        }
    }

    public void RightArrowClick()
    {
        rotating = true;

        Quaternion aux = new Quaternion();
        aux.eulerAngles = new Vector3(toRotate.transform.rotation.x, toRotate.transform.rotation.y, toRotate.transform.rotation.z - degrees);
        toRotate.transform.rotation = Quaternion.Slerp(toRotate.transform.rotation, aux, t);

        //.Set(0, 0, Mathf.Lerp(toRotate.transform.rotation.z, toRotate.transform.rotation.z - degrees, Time.deltaTime * speed));

        //toRotate.transform.Rotate(0, 0, -degrees);

        for (int i = 0; i < children; i++)
        {
            toRotate.transform.GetChild(i).rotation.eulerAngles.Set(0, 0, Mathf.Lerp(toRotate.transform.GetChild(i).rotation.z, toRotate.transform.GetChild(i).rotation.z + degrees, t * speed));
        }

        if (characterSelected > 0)
            characterSelected++;
        else
            characterSelected = children;
    }

    public void LeftArrowClick()
    {
        rotating = true;

        toRotate.transform.rotation.eulerAngles.Set(0, 0, Mathf.Lerp(toRotate.transform.rotation.z, toRotate.transform.rotation.z + degrees, Time.deltaTime * speed));

        //toRotate.transform.Rotate(0, 0, degrees);

        for (int i = 0; i < children; i++)
        {
            toRotate.transform.GetChild(i).rotation.eulerAngles.Set(0, 0, Mathf.Lerp(toRotate.transform.GetChild(i).rotation.z, toRotate.transform.GetChild(i).rotation.z - degrees, t * speed));
        }

        if (characterSelected < children)
            characterSelected++;
        else
            characterSelected = 0;
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
