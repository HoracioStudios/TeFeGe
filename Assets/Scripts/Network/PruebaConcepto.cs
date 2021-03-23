using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaConcepto : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log( ClientCommunication.LogIn("Posna", "Una contraseña"));
        }
    }
}
