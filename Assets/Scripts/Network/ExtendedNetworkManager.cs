using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ExtendedNetworkManager : NetworkManager
{
    [Header("Extended Features")]
    [Tooltip("RoundManager prefab")]
    [SerializeField] GameObject roundManagerPrefab;

    [Tooltip("Min num of players that determines when the game should start")]
    [SerializeField] int minPlayers = 2;

    GameObject roundManager;


    public override void OnServerChangeScene(string newSceneName)
    {
        NetworkServer.Destroy(roundManager);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
    }

    public override void OnStartServer()
    {
        Time.timeScale = 0;
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        
        if(!roundManager) roundManager = Instantiate(roundManagerPrefab);

        NetworkServer.Spawn(roundManager, conn);

        roundManager.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);

        if (numPlayers >= minPlayers)
            Time.timeScale = 1;
    }
}