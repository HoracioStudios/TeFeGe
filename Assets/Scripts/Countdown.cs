using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Countdown : MonoBehaviour
{
    public Sprite[] numbers;
    public Image numberScreen;

    public void updateCountdown(float time)
    {
        if (time < 1)
            numberScreen.sprite = numbers[2];
        else if (time < 2)
            numberScreen.sprite = numbers[1];
        else if (time < 3)
            numberScreen.sprite = numbers[0];
    }

}
