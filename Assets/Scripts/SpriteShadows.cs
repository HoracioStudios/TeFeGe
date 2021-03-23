using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadows : MonoBehaviour
{
    private SpriteRenderer sprite;

    public UnityEngine.Rendering.ShadowCastingMode cast = UnityEngine.Rendering.ShadowCastingMode.On;
    public bool receive = true;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();

        sprite.receiveShadows = receive;
        sprite.shadowCastingMode = cast;
    }
}
