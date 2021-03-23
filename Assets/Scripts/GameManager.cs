using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //[HideInInspector]
    public bool isControllerMode = false;


    private void Start()
    {
        //CursorController.instance.ActivateYellowCursor();
    }

    void Update()
    {
        if (isControllerMode)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Fire") != 0)
            {
                isControllerMode = false;
            }
        }
        else
        {
            if (Input.GetAxis("Horizontal_Joy") != 0 || Input.GetAxis("Vertical_Joy") != 0 || Input.GetAxis("Aim_X") != 0 || Input.GetAxis("Aim_Y") != 0 || Input.GetAxis("Fire_Joy") != 0)
            {
                isControllerMode = true;
            }
        }
    }
}
