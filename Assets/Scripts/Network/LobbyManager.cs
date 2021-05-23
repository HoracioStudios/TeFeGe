using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Text waitText;
    float textTime = 0f;
    public GameObject sprite;
    float time = 0f;

    // Update is called once per frame
    void Update()
    {
        canvasUpdate();

       // searchPlayer();
    }

    void canvasUpdate()
    {
        if (textTime < 3.75)
        {
            textTime += Time.deltaTime;
            if (textTime >= 0.75f && textTime < 1.5f)
            {
                waitText.text = "WAITING FOR PLAYER.";
            }
            else if (textTime >= 1.5f && textTime < 2.25f)
            {
                waitText.text = "WAITING FOR PLAYER..";
            }
            else if (textTime >= 2.25f && textTime < 3f)
            {
                waitText.text = "WAITING FOR PLAYER...";
            }
            else { waitText.text = "WAITING FOR PLAYER"; }
        }
        else { textTime = 0f; }

        sprite.transform.Rotate(Vector3.forward * -50f * Time.deltaTime);
    }

    void searchPlayer()
    {
        try
        {
            Rival found = null;// ClientCommunication.SearchPair(GameManager.instance.player.playerID, time);
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
        }
        catch (RestResponseException e)
        {
            Debug.Log("Error en búsqueda de pareja: " + e.Message);
        }
        //Llamar a el findPair del servidor de Matchmaking
    }
}
