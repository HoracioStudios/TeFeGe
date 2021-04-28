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
            ClientCommunication.SignIn("pass", email: "HOLA");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log(ClientCommunication.OnlineUsers());
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log(ClientCommunication.SearchPair(2, Time.time));
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Debug.Log(ClientCommunication.DeleteAccount("Posna", "nosejaja@gmail.com", "Una contraseña"));
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log(ClientCommunication.GetEmailAvailable("JOSEJOSEJOSE"));
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            Debug.Log(ClientCommunication.LeaveQueue(2));
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            Debug.Log(ClientCommunication.GetInfo(3, "posna"));
        }
    }
}
