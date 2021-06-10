using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Mirror
{

    public struct CharacterMessage : IMessageBase
    {
        public int playerSelection;

        public CharacterMessage(int playerSel)
        {
            playerSelection = playerSel;
        }

        public void Deserialize(NetworkReader reader)
        {
            playerSelection = reader.ReadInt32();
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.WriteInt32(playerSelection);
        }
    }
}

public class ExtendedNetworkManager : NetworkManager
{
    [Header("Round Manager")]
    [Tooltip("RoundManager prefab")]
    [SerializeField] GameObject roundManagerPrefab;

    [Tooltip("Min num of players that determines when the game should start")]
    [SerializeField] int minPlayers = 2;

    GameObject roundManager;


    [Header("Player Selection")]
    [Tooltip("Player prefabs")]
    [SerializeField] GameObject[] playerPrefabs;

    [Tooltip("Player selection")]
    public int playerSelection = 0;

    public override void Awake()
    {
        base.Awake();
        string arg = GetArg("-port");
        if (arg != null)
            GetComponent<TelepathyTransport>().port = ushort.Parse(arg);

        //ClientCommunication.Init(networkAddress, GetComponent<TelepathyTransport>().port.ToString());
    }

    public static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }

    public override void OnServerChangeScene(string newSceneName)
    {
        NetworkServer.Destroy(roundManager);
        //Application.Quit();
    }

    public override void OnServerSceneChanged(string sceneName)
    {
    }

    public override void OnStartServer()
    {
        //Cambio de puerto
        base.OnStartServer();
        NetworkServer.RegisterHandler<CharacterMessage>(OnAddCharacter);
        Time.timeScale = 0;        
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);

        //send the message here
        //the message should be defined above this class in a NetworkMessage
        CharacterMessage characterMessage = new CharacterMessage(playerSelection);

        conn.Send(characterMessage);
    }

    public void OnAddCharacter(NetworkConnection conn, CharacterMessage msg)
    {
        int playerSel = msg.playerSelection;

        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefabs[playerSel], startPos.position, startPos.rotation)
            : Instantiate(playerPrefabs[playerSel]);

        NetworkServer.AddPlayerForConnection(conn, player);


        if (!roundManager) roundManager = Instantiate(roundManagerPrefab);

        NetworkServer.Spawn(roundManager, conn);

        roundManager.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);


        if (numPlayers >= minPlayers)
            Time.timeScale = 1;
    }

    public override void OnStopClient()
    {
        if (RoundManager.instance.gameStarted)
            RoundManager.instance.FinishGameOnDisconnect();
        base.OnStopClient();
    }
}