using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentagono : MonoBehaviour
{
    float centerX = 0.0f;
    float centerY = -75.0f;
    float radius = 125.0f;

    // Start is called before the first frame update
    void Start()
    {
        float total = gameObject.transform.childCount;
        for (int i = 0; i < total; i++)
        {
            Vector3 pos = new Vector3(centerX, centerY, 0);

            pos.x = pos.x + Mathf.Cos(i * 360.0f /total) * radius;
            pos.y = pos.y + Mathf.Sin(i * 360.0f /total) * radius;

            gameObject.transform.GetChild(i).GetComponent<RectTransform>().position = pos;

            Debug.Log(i + " " + gameObject.transform.GetChild(i).GetComponent<RectTransform>().position + " ");
        }
    }    
}
