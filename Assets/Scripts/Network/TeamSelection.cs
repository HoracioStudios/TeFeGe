using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelection : MonoBehaviour
{
    public MyNetworkManager networkManager;

    public void JoinLocalA()
    {
        networkManager.networkAddress = "localhost";
        Debug.Log("Seleccion equipo A (9)");
        networkManager.SetLayer(9);
        networkManager.StartClient();
    }

    public void JoinLocalB()
    {
        networkManager.networkAddress = "localhost";
        Debug.Log("Seleccion equipo B (10)");
        networkManager.SetLayer(10);
        networkManager.StartClient();
    }
}
