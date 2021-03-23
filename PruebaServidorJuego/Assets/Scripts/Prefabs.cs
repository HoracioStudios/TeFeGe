using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefabs : NetworkManager
{

    public GameObject[] prefabs;

    
    
    override public void Start()
    {
        int rnd = Random.Range(0, 2);
        playerPrefab = prefabs[rnd];
        base.Start();
        
    }

    
    void Update()
    {
        
    }
}
