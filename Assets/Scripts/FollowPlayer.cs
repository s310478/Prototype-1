using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 6.5f, -7.75f);

    // LateUpdate --> so that camera moves 'after' vehicle moves, this smothes out the camera jitters.
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
