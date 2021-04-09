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

    [Tooltip("Names of online scenes")]
    [SerializeField] List<string> onlineScenes;

    bool spawnedObjects = false;

    GameObject roundManager;


    public override void OnServerChangeScene(string newSceneName)
    {
        NetworkServer.UnSpawn(roundManager);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
    }

    public override void OnStartServer()
    {
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        
        if(!roundManager) roundManager = Instantiate(roundManagerPrefab);

        NetworkServer.Spawn(roundManager, conn);
    }
}