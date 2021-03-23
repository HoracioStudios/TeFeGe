using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter emitter;

    private void Start()
    {
        Play();
    }

    public void Play()
    {
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
