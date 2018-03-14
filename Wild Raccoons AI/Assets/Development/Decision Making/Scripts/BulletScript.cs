using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TARGET: Buller prefab
/// Moves the bullet forward
/// </summary>

public class BulletScript : MonoBehaviour {

    public Vector3 direction;
    public float speed = 5;
    public GameObject Boss;
    public float maxBossDistance = 20;

	// Use this for initialization
	void Start ()
    {
        Boss = GameObject.FindGameObjectWithTag("Boss");
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (Vector3.Distance(this.transform.position, Boss.transform.position) > maxBossDistance)
            Destroy(this.gameObject);
	}
}
