using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_ArcaneShot : MonoBehaviour
{
    public float movespeed = 0f;
    public float lifeSpan = 999f;
    private GameObject boss;
    private void Start()
    {
        boss = GameObject.FindWithTag("BossManager");
    }

    private void OnTriggerEnter(Collider collision)
    {
      if (collision.gameObject.tag != "Boss")
          return;
      boss.GetComponent<Stats>().TakeMDamage(60);
      Destroy(gameObject);
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
