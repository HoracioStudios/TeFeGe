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
        Startup();
    }

    void Startup()
    {
        Message msg = ClientCommunication.AddToQueue();

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

                GameManager.instance.ThrowErrorScreen(msg.code);
            }
            else
                Startup();
        }
        else
        {
            GameManager.instance.ThrowErrorScreen(msg.code);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (looking)
        {
            if (petitionTimer > petitionWait)
            {
                canvasUpdate();

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
        Message msg = ClientCommunication.SearchPair(time);

        if (msg.code == 200)
        {
            PairSearch found = (PairSearch)msg;

            if (!found.finished)
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
        else if (msg.code == 403)
        {
            msg = ClientCommunication.Refresh();

            if (msg.code != 200)
            {
                looking = false;

                GameManager.instance.ThrowErrorScreen(msg.code);
            }
        }
        else
        {
            looking = false;

            GameManager.instance.ThrowErrorScreen(msg.code);
        }
    }

    public void Close()
    {
        Message msg = ClientCommunication.LeaveQueue();

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

                GameManager.instance.ThrowErrorScreen(msg.code);
            }
            else
                Close();
        }
        else
        {
            GameManager.instance.ThrowErrorScreen(msg.code);
        }
    }
}
