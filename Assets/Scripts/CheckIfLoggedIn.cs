using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfLoggedIn : MonoBehaviour
{
    void Start()
    {
        if (GameManager.instance.loggedIn)
            Destroy(this.gameObject);
    }
}
