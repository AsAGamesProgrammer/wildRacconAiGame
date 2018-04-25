using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {

    //Melee attack
    public float meleeAttackRadius = 10;
    public Vector3 sphereGrowthVector = new Vector3(0.01f, 0.01f, 0.01f);
    public GameObject explosionSpherePrefab;    //prefab
    GameObject meleeAttack;     //Object
    
    int damage = 20;
    //Flag
    bool attackPrepared = false;
    bool attackOnGoing = false;

    //Boss
    public GameObject boss;

    //Script
    public AttackManager attackManager;

    // Use this for initialization
    void Start () {
    
  }
	
	// Update is called once per frame
	void Update ()
    {
		if(attackPrepared && attackOnGoing)
        {
            if (meleeAttack.transform.localScale.x < meleeAttackRadius)
            {
                meleeAttack.transform.localScale += sphereGrowthVector;
            }
            else
            {
                Destroy(meleeAttack);
                attackPrepared = false;
                attackOnGoing = false;

                //Send note to attack manager that melee is finished
                attackManager.NextAttack = true;
            }
        }
	}


    void prepareMeleeAttack()
    {
        if (!attackPrepared)
        {
            meleeAttack = Instantiate(explosionSpherePrefab, boss.transform);
            attackPrepared = true;

            attackOnGoing = true;
        }
    }


    public void performMeleeAttack()
    {
        prepareMeleeAttack();
    }

  
}
