using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAuthority : NetworkBehaviour
{
    public GameObject obj;
    public bool IsOurLocalPlayer()
    {
        return isLocalPlayer;
    }

    
    public void SpawnObjectServer()
    {
        //obj = Instantiate(_obj);
        //Spawn();
    }

    [Command]
    public void Spawn(GameObject go, Vector3 pos, Quaternion rot)
    {
        Debug.Log($"Obj spawned");
        try
        {
            NetworkServer.Spawn(Instantiate(go, pos, rot));
        }
        catch (System.Exception)
        {
            Debug.Log("Obj is probably null");
            throw;
        }
    }
}
