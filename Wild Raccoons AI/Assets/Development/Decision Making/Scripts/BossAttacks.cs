﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This scripts contains an implementation of boss attacks

public class BossAttacks : MonoBehaviour {

    //Boss
    public GameObject boss;

    //Melee attack
    public float meleeAttackRadius = 10;
    public Vector3 sphereGrowthVector = new Vector3(0.01f, 0.01f, 0.01f);
    public GameObject meleeAttackSphere;    //prefab
    GameObject meleeAttack;     //Object

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
        performMeleeAttack();
	}

    //MELEE ATTACK 1
    //Create a circle around the boss
    //Grow a circle until reaches maximum diameter
    //Do damage inside the circle

    void prepareMeleeAttack()
    {
        if (!attackPrepared)
        {
            meleeAttack = Instantiate(meleeAttackSphere, boss.transform);
            attackPrepared = true;
        }
    }

    void performMeleeAttack()
    {
        prepareMeleeAttack();

        Debug.Log("Instantiate");

        if (meleeAttack.transform.localScale.x < meleeAttackRadius)
        {
            Debug.Log(meleeAttack.transform.localScale.x);
            meleeAttack.transform.localScale += sphereGrowthVector;
        }
        else
        {
            Destroy(meleeAttack);
            Debug.Log("Destoryed");
            attackPrepared = false;
        }
    }
}
