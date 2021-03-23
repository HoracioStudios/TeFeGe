using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePauseMenu : MonoBehaviour
{
    public Canvas canvas;

    // Update is called once per frame
    public void Close()
    {
        Time.timeScale = 1f;
        canvas.enabled = false;
    }
}
