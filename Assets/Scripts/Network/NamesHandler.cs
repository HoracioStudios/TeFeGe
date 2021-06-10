using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class NamesHandler : NetworkBehaviour
{
    // Character name
    public string characterName;

    public TextMeshPro nickName;

    void Start()
    {
        if (isLocalPlayer)
        {
            nickName.text = GameManager.instance.GetNick();
            GameManager.instance.gameData.playerChar = characterName;
        }
        else
        {
            nickName.text = GameManager.instance.gameData.rivalNick;
            GameManager.instance.gameData.rivalChar = characterName;
        }

    }
}
