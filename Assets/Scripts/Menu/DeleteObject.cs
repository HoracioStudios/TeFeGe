using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    public GameObject objectToDestroy;

   public void DestroyObject()
    {
        if (objectToDestroy) Destroy(objectToDestroy);
    }
}
