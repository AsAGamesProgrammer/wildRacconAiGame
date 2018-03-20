using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Animation : MonoBehaviour
{
    public Vector3 rotation = new Vector3(5f, 5f, 5f);

	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(rotation);
	}
}
