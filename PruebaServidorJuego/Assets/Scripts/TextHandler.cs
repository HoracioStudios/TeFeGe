using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHandler : MonoBehaviour
{
    public Text texto;
    private int count = 0;

    private void Awake()
    {
        Player.NewBall += NewBall;
    }

    public void NewBall()
    {
        Debug.Log("Nueva Bola");
        count++;
        texto.text = count.ToString();
    }

}
