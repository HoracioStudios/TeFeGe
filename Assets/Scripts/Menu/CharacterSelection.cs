using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public RectTransform toRotate;
    int children;
    float degrees;

    private void Start()
    {



        children = toRotate.gameObject.transform.childCount; //Por si se añadieran más personajes en el futuro
        degrees = 360.0f/children;
    }

    public void RightArrowClick()
    {
        toRotate.Rotate(new Vector3(0, 0, degrees));

        for (int i = 0; i < children; i++)
        {
            toRotate.gameObject.transform.GetChild(i).Rotate(new Vector3(0, 0, -degrees));
        }
    }

    public void LeftArrowClick()
    {
        toRotate.Rotate(new Vector3(0, 0, -degrees));

        for (int i = 0; i < children; i++)
        {
            toRotate.gameObject.transform.GetChild(i).Rotate(new Vector3(0, 0, degrees));
        }
    }
}
