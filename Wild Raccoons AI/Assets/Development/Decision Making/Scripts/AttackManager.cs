using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    //Script
    BossAttacks attackList;
    Stats bossStats;

	// Use this for initialization
	void Start ()
    {
        //Getting script information
        attackList = GetComponent<BossAttacks>();
        bossStats = GetComponent<Stats>();

        //Shoot at the beginning of the round
        attackList.shootAt(new Vector3(1f, 0, 0.3f));
        attackList.shootAt(new Vector3(1f, 0, -0.3f));
    }
	
	// Update is called once per frame
	void Update ()
    {
        //attackList.applyPhysicalShield();           //shield

        if (!bossStats.pShieldEnabled)              //if shield is not up
        {
            //Perform melee attack
            if (attackList.performMeleeAttack())    //if aura ttack started
            {
                //Shoot three times at the end of the attack
                attackList.shootAt(new Vector3(1f, 0, 0.5f));
                attackList.shootAt(new Vector3(1f, 0, 0));
                attackList.shootAt(new Vector3(1f, 0, -0.5f));

                //Teleport
                switch (bossStats.getOrientation())
                {
                    case Orientation.Left:
                        attackList.teleport(Orientation.Top);
                        break;

                    case Orientation.Top:
                        attackList.teleport(Orientation.Right);
                        break;

                    case Orientation.Right:
                        attackList.teleport(Orientation.Left);

                        //TEST
                        bossStats.TakePDamage(100);
                        break;
                }
            }
        }
    }
}
