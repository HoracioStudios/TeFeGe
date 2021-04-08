using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ExtendedNetworkManager : NetworkManager
{
    [Header("Extended Features")]
    [Tooltip("Objects to spawn server-side, only once, when the game starts")]
    [SerializeField] GameObject[] serverObjects;

    [Tooltip("Min num of players that determines when the game should start")]
    [SerializeField] int minPlayers = 2;

    bool spawnedObjects = false;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        if (!spawnedObjects && singleton.numPlayers == minPlayers)
            StartGame(conn);
    }

    public void StartGame(NetworkConnection conn)
    {
        spawnedObjects = true;

        foreach(GameObject o in serverObjects)
        {
            GameObject i = Instantiate(o);

            //NetworkServer.AddPlayerForConnection(conn, i);

            NetworkServer.Spawn(i, conn);
        }
    }
}