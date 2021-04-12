using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToLobby : MonoBehaviour
{
    public void Go()
    {
        //ESTO HAY QUE CAMBIARLO CUANDO HAGAMOS EL LOBBY
        if(Mirror.NetworkManager.singleton)
            Mirror.NetworkManager.singleton.StartClient();
    }
}
