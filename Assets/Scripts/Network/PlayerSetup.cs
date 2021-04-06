using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [Tooltip("Components to disable")]
    [SerializeField]
    Behaviour[] components;

    public override void OnStartClient()
    {
        base.OnStartClient();

        // Si es localPlayer, sera siempre del equipo A
        int layer = isLocalPlayer ? 9: 10;
        gameObject.layer = layer;

        // El enemigo es inamovible. Evita que haya desincronizaciones de posiciones
        if (!isLocalPlayer)
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
        }
    }

    public bool IsOurLocalPlayer() { return isLocalPlayer; }

    void DisableComponents()
    {
        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }
    }

}
