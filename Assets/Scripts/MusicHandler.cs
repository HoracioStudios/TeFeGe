using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MusicHandler : NetworkBehaviour
{
    public FMODUnity.StudioEventEmitter emitter;

    private void Start()
    {
        if (isLocalPlayer)
            Play();
    }

    public void Play()
    {
        if (isLocalPlayer)
            emitter.Play();
    }

    public void Stop()
    {
        emitter.Stop();
    }

    public bool IsPlaying()
    {
       return emitter.IsPlaying();
    }
}
