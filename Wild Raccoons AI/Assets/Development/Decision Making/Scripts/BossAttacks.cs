using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This scripts contains an implementation of boss attacks

public class BossAttacks : MonoBehaviour {

    //Boss
    public GameObject boss;

    //Melee attack
    public float meleeAttackRadius = 10;
    public Vector3 sphereGrowthVector = new Vector3(0.01f, 0.01f, 0.01f);
    public GameObject explosionSpherePrefab;    //prefab
    GameObject meleeAttack;     //Object

    //Shooting
    public GameObject bulletPrefab;

    //Flags
    bool attackPrepared = false;
    bool attackExecuting = false;
    bool attackFinished = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {

    }

    //MELEE ATTACK 1
    //Create a circle around the boss
    //Grow a circle until reaches maximum diameter
    //Do damage inside the circle

    void prepareMeleeAttack()
    {
        if (!attackPrepared)
        {
            meleeAttack = Instantiate(explosionSpherePrefab, boss.transform);
            attackPrepared = true;
        }
    }

    //Returns true when finished
    public bool performMeleeAttack()
    {
        prepareMeleeAttack();

        Debug.Log("Instantiate");

        if (meleeAttack.transform.localScale.x < meleeAttackRadius)
        {
            Debug.Log(meleeAttack.transform.localScale.x);
            meleeAttack.transform.localScale += sphereGrowthVector;
            return false;
        }
        else
        {
            Destroy(meleeAttack);
            Debug.Log("Destoryed");
            attackPrepared = false;
            return true;
        }
    }

    //SHOOT
    //Creates a bullet with given direction, speed and maximum distance from the boss
    public void shootAt(Vector3 direction)
    {
        GameObject newBullet = Instantiate(bulletPrefab, boss.transform);
        newBullet.GetComponent<BulletScript>().direction = direction;
    }

    //BOMB ATTACK
    //Launch bombs at ransom/semi random position
    //Melee attack for each circle with delay



}
