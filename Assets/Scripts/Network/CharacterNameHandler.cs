using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class CharacterNameHandler : NetworkBehaviour
{
    // Character name
    public string Name;
    void Start()
    {
        if (isLocalPlayer)
            GameManager.instance.gameData.playerChar = name;
        else
            GameManager.instance.gameData.rivalChar = name;

    }
}
