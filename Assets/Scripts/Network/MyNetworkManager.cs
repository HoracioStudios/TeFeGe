using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    [Tooltip("Players to spawn")]
    GameObject[] players;

    [Tooltip("Specific player")]
    public int player;

    int layer = 9;

    public void SetLayer(int layer_)
    {
        //playerPrefab.layer = layer_;
        //layer = layer_;
        Debug.Log("Layer al settearlo: " + layer);
    }
    
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        //player.layer = layer;
        //Debug.Log("Layer de spawn del server" + player.layer);
        //Debug.Log("Layer de prefab del server" + layer);
        //layer++;

        NetworkServer.AddPlayerForConnection(conn, player);

        // Para cuando esten los dos jugadores, hace lo que sea
        if(numPlayers == 2)
        {

        }
    }
}