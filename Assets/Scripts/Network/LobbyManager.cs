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

    float petitionTimer = 0f;
    float petitionWait = 0.5f;

    bool looking = false;

    private void Start()
    {

        try
        {
            Startup();
        }
        catch (System.Exception)
        {
            GameManager.instance.ThrowErrorScreen(-2);
        }
    }

    void Startup()
    {
        ServerMessage msg = ClientCommunication.AddToQueue();

        if (msg.code == 200)
        {
            looking = true;
        }
        else if (msg.code == 403)
        {
            msg = ClientCommunication.Refresh();

            if (msg.code != 200)
            {
                looking = false;

                GameManager.instance.ThrowErrorScreen(msg.code, ((REST_Error)msg).message);
            }
            else
                Startup();
        }
        else
        {
            GameManager.instance.ThrowErrorScreen(msg.code, ((REST_Error)msg).message);
        }

        GameManager.instance.inQueue = looking;
    }

    // Update is called once per frame
    void Update()
    {
        canvasUpdate();
        if (looking)
        {

            if (petitionTimer > petitionWait)
            {
                time += petitionTimer;
                searchPlayer();

                petitionTimer = 0;
            }
            else
                petitionTimer += Time.deltaTime;
        }
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
        ServerMessage msg = ClientCommunication.SearchPair(time);
        Debug.Log("Tiempo buscando: " + time);

        if (msg.code == 200)
        {
            PairSearch found = (PairSearch)msg;

            if (!found.finished)
            {
                time += Time.deltaTime;
            }
            else
            {
                //Obtener puesto del servidor de juego
                if (Mirror.NetworkManager.singleton && getGameServerInfo(found))
                {
                    looking = false;
                    Mirror.NetworkManager.singleton.StartClient();
                }
            }
        }
        else if (msg.code == 403)
        {
            msg = ClientCommunication.Refresh();

            if (msg.code != 200)
            {
                looking = false;

                GameManager.instance.ThrowErrorScreen(msg.code, ((REST_Error)msg).message);
            }
        }
        else
        {
            looking = false;

            GameManager.instance.ThrowErrorScreen(msg.code, ((REST_Error)msg).message);
        }
    }

    bool getGameServerInfo(PairSearch foundRival)
    {
        ServerMessage serverMsg = ClientCommunication.FindServerInfo(GameManager.instance.ID, foundRival.rivalID);
        if (serverMsg.code == 200)
        {
            ServerMatchInfo serverInfo = (ServerMatchInfo)serverMsg;
            Mirror.NetworkManager.singleton.gameObject.GetComponent<Mirror.TelepathyTransport>().port = ushort.Parse(serverInfo.port);

            Debug.Log("Datos de partida guardados");

            //Guardado de datos de la partida
            GameManager.instance.gameData = new GameData();
            GameManager.instance.gameData.matchID = serverInfo.matchID;

            GameManager.instance.gameData.rivalID = foundRival.rivalID;
            GameManager.instance.gameData.rivalNick = foundRival.rivalNick;
            GameManager.instance.gameData.rivalRating = foundRival.bestRivalRating;
            GameManager.instance.gameData.rivalRD = foundRival.bestRivalRD;
            GameManager.instance.gameData.myRating = foundRival.myRating;
            GameManager.instance.gameData.myRD = foundRival.myRD;

            return true;
        }

        return false;
    }

    public void Close()
    {
        ServerMessage msg = ClientCommunication.LeaveQueue();

        if (msg.code == 200)
        {
            looking = false;
        }
        else if (msg.code == 403)
        {
            msg = ClientCommunication.Refresh();

            if (msg.code != 200)
            {
                looking = true;

                GameManager.instance.ThrowErrorScreen(msg.code, ((REST_Error)msg).message);
            }
            else
                Close();
        }
        else
        {
            GameManager.instance.ThrowErrorScreen(msg.code, ((REST_Error)msg).message);
        }

        GameManager.instance.inQueue = false;
    }
}
