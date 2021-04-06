using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public StateMachine states;
    public Animator effectAnimator;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((int)states.GetState().state);
        effectAnimator.SetInteger("State", (int)states.GetState().state);
    }
}
