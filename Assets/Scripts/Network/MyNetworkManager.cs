using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    [Tooltip("Players to spawn")]
    [SerializeField]GameObject[] players;

    [Tooltip("Prefab para probar disparos o habilidades")]
    public int playerPrueba;

    [Tooltip("Comprobar si esta activo para desactivar este manager")]
    public NetworkManager networkManager;

    int layer = 9;

    public void SetLayer(int layer_)
    {
        //playerPrefab.layer = layer_;
        //layer = layer_;
        Debug.Log("Layer al settearlo: " + layer);
    }

    public override void Awake()
    {
        if (!networkManager.enabled)
            gameObject.SetActive(false);
        base.Awake();
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        GameObject player1 = startPos != null
            ? Instantiate(players[playerPrueba%players.Length], startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        player1.layer = 10;
        player1.tag = "BTeam";
        //Debug.Log("Layer de spawn del server" + player.layer);
        //Debug.Log("Layer de prefab del server" + layer);
        //layer++;

        NetworkServer.AddPlayerForConnection(conn, player);
        NetworkServer.AddPlayerForConnection(conn, player1);

        // Para cuando esten los dos jugadores, hace lo que sea
        //if(numPlayers == 2)
        //{

        //}
    }
}