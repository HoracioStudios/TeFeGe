using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitCounter : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "BTeam")
            GameManager.instance.gameData.accuracy++;
    }
}
