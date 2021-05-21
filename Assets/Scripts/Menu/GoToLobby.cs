using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLobby : MonoBehaviour
{
    float time = 0;
    private void Update()
    {
        try
        {
            Rival found = ClientCommunication.SearchPair(GameManager.instance.player.playerID, time);
            if (!found.found)
            {
                time += Time.deltaTime;
            }
            else
            {
                //Ir a partida
                if (Mirror.NetworkManager.singleton)
                    Mirror.NetworkManager.singleton.StartClient();
            }
        } catch (RestResponseException e)
        {
            Debug.Log("Error en búsqueda de pareja: " + e.Message);
        }
        //Llamar a el findPair del servidor de Matchmaking
        
    }
    public void Go()
    {
        //ESTO HAY QUE CAMBIARLO CUANDO HAGAMOS EL LOBBY
        if(Mirror.NetworkManager.singleton)
            Mirror.NetworkManager.singleton.StartClient();
    }
}
