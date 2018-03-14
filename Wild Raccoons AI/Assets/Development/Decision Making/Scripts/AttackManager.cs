using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    //Script
    BossAttacks attackList;

	// Use this for initialization
	void Start ()
    {
        //Getting script information
        attackList = GetComponent<BossAttacks>();

        //Shoot at the beginning of the round
        attackList.shootAt(new Vector3(-0.3f, 0, -1));
        attackList.shootAt(new Vector3(0.3f, 0, -1));
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Perform melee attack
        if(attackList.performMeleeAttack())
        {
            //Shoot three times at the end of the attack
            attackList.shootAt(new Vector3(0f, 0, -1));
            attackList.shootAt(new Vector3(0.5f, 0, -1));
            attackList.shootAt(new Vector3(-0.5f, 0, -1));
        }
	}
}
