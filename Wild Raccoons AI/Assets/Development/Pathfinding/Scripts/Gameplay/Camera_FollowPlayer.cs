using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_FollowPlayer : MonoBehaviour {

    public GameObject player;

    private Vector3 startingPosition;

    private Vector3 offset;

    private void Awake()
    {
        startingPosition = transform.position;

        offset = startingPosition - player.transform.position;
    }

    private void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
