using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPattern : MonoBehaviour {

    public GameObject bulletPrefab;

    //Boss
    GameObject Boss;

    //Counts
    public int destroyedBullets = 0;
    int bulletsTotal = 1;

    //Script
    AttackManager attackManagerScript;

    // Use this for initialization
    void Start ()
    {
        //Scripts
        attackManagerScript = GameObject.FindGameObjectWithTag("BossManager").GetComponent<AttackManager>();

        //Boss
        Boss = GameObject.FindGameObjectWithTag("Boss");
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(destroyedBullets >= bulletsTotal)
        {
            attackManagerScript.NextAttack = true;

            //Reset variables
            bulletsTotal = 1;
            destroyedBullets = 0;
        }
	}

    public void ShootThreeBullets()
    {
        shootAt(new Vector3(1f, 0, 0.5f));
        shootAt(new Vector3(1f, 0, 0));
        shootAt(new Vector3(1f, 0, -0.5f));

        bulletsTotal = 3;
    }


    public void ShootFiveBullets()
    {
        shootAt(new Vector3(1f, 0, 0.2f));
        shootAt(new Vector3(1f, 0, 0.3f));
        shootAt(new Vector3(1f, 0, 0.0f));
        shootAt(new Vector3(1f, 0, -0.2f));
        shootAt(new Vector3(1f, 0, -0.3f));

        bulletsTotal = 5;
    }

    //Creates a bullet with given direction, speed and maximum distance from the boss
    public void shootAt(Vector3 direction)
    {
        GameObject newBullet = Instantiate(bulletPrefab, Boss.transform);
        newBullet.GetComponent<BulletScript>().direction = direction;
    }
}
