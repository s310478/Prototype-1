using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{

    Rigidbody Rb;
    public AudioSource engineSource;

    public int gearShiftLength;
    public float pitchBoost;
    public float pitchRange;

    float temp1;
    float temp2; 

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float speed = Rb.velocity.magnitude;
        temp1 = speed / gearShiftLength;
        temp2 = (int)temp1;

        float Difference = temp1 - temp2;

        engineSource.pitch = Mathf.Lerp(engineSource.pitch, (pitchRange * Difference) * pitchBoost, 0.01f);
    }
}
