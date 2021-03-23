using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlModeChecker : MonoBehaviour
{

    private GameManager gm = null;
    private Text txt;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        txt = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //sometimes gamemanager doesn't get picked up during Start
        if (gm == null)
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (gm != null)
        {
            if (gm.isControllerMode)
                txt.text = "Controller";
            else
                txt.text = "Mouse";
        }
    }
}
