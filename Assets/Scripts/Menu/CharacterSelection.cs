using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public CharacterWheel pivot;

    public bool leftDir;

    public void Click()
    {
        if (!pivot.rotating) pivot.startLerp(leftDir);
    }
}
