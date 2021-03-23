using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Canvas canvas;

    public bool pauseOn = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseOn = Time.timeScale != 0;

            if (pauseOn)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;

            canvas.enabled = pauseOn;
        }
    }
}
