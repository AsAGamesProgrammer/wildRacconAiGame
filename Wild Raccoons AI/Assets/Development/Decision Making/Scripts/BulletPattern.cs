using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPattern : MonoBehaviour {

    public GameObject bulletPrefab;

    //Counts
    int destroyedBullets = -1;
    int bulletsTotal = 0;

    //Script
    AttackManager attackManagerScript;

    // Use this for initialization
    void Start ()
    {
        attackManagerScript = GameObject.FindGameObjectWithTag("BossManager").GetComponent<AttackManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(destroyedBullets >= bulletsTotal)
        {
            attackManagerScript.NextAttack = true;
        }
	}

    public void ShootThreeBullets()
    {
        //TODO
        //CONNECT ULLET SCRIPT

        //attackList.shootAt(new Vector3(1f, 0, 0.5f));
        //attackList.shootAt(new Vector3(1f, 0, 0));
        //attackList.shootAt(new Vector3(1f, 0, -0.5f));
    }
}
