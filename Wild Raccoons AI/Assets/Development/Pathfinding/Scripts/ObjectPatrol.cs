using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPatrol : MonoBehaviour
{
    private Vector3 startingPosition;
    private Vector3 movementScalar = new Vector3(0f, 5f, 0f);

    private void Awake()
    {
        startingPosition = transform.position;
    }

    void Update ()
    {
        transform.position = startingPosition + movementScalar * Mathf.Sin(Time.time);
	}
}
