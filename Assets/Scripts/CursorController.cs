using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;

    public Texture2D yellowCursor, greenCursor;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void ActivateYellowCursor()
    {
        Cursor.SetCursor(yellowCursor, new Vector2(yellowCursor.width/2, yellowCursor.height / 2), CursorMode.Auto);
    }

    public void ActivateGreenCursor()
    {
        Cursor.SetCursor(greenCursor, new Vector2(greenCursor.width / 2, greenCursor.height / 2), CursorMode.Auto);
    }
}
