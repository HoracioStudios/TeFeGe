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
        effectAnimator.SetInteger("State", (int)states.GetState().state);
    }
}
