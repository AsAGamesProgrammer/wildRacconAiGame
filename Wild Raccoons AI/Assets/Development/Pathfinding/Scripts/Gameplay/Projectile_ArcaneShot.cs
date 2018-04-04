using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_ArcaneShot : MonoBehaviour
{
    public float movespeed = 0f;
    public float lifeSpan = 999f;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        lifeSpan -= Time.deltaTime;

        if(lifeSpan > 0f)
        {
            transform.position += transform.forward * movespeed;
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
