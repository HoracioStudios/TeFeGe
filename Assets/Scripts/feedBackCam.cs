using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feedBackCam : MonoBehaviour
{

    public float time_ = 1.0f; //Time it moves
    public float intensity_ = 1.0f;

    private Vector3 vectorPos_;
    private float t_;
    private float cadence_;
    private bool shake_ = false;

    void Start()
    {
        vectorPos_ = transform.position - transform.parent.position;
        t_ = time_;
    }

    void Update()
    {
        if (shake_)
            Shake();
    }

    //Call this method if you want the camera shakes
    public void startShaking() { shake_ = true; }

    private void Shake()
    {
        if (t_ > 0)
        {
            transform.position = (vectorPos_ + transform.parent.position) + new Vector3(Random.insideUnitCircle.x, 0,Random.insideUnitCircle.y) * intensity_;
            t_ -= Time.deltaTime;
            if (t_ <= 0)
            {
                shake_ = false;
                t_ = time_;
                transform.position = vectorPos_ + transform.parent.position;
            }
        }
    }
}