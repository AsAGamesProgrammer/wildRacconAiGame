using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    //Script
    BossAttacks attackList;
    Stats bossStats;

    //Player
    public GameObject playerModel;

    //Current zone
    public Zones currentPlayerZone;

    //Next attack
    public bool NextAttack = true;

	// Use this for initialization
	void Start ()
    {
        //Getting script information
        attackList = GetComponent<BossAttacks>();
        bossStats = GetComponent<Stats>();

        //Shoot at the beginning of the round
        //attackList.shootAt(new Vector3(1f, 0, 0.3f));
        //attackList.shootAt(new Vector3(1f, 0, -0.3f));
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log("Is ready to attack " + NextAttack + " aiming for " + currentPlayerZone);

        //STATE MACHINE
        if (NextAttack)
        {
            if (currentPlayerZone == Zones.Melee)
            {
                Debug.Log("Attacking melee");
                NextAttack = false;         //Attack performed
                attackList.AttackMelee();
            }
        }




        //attackList.applyPhysicalShield();           //shield

        //if (!bossStats.pShieldEnabled)              //if shield is not up
        //{
        //    //Perform melee attack
        //    if (attackList.performMeleeAttack())    //if aura ttack started
        //    {
        //        //Shoot three times at the end of the attack
        //        attackList.shootAt(new Vector3(1f, 0, 0.5f));
        //        attackList.shootAt(new Vector3(1f, 0, 0));
        //        attackList.shootAt(new Vector3(1f, 0, -0.5f));

        //        //Teleport
        //        switch (bossStats.getOrientation())
        //        {
        //            case Orientation.Left:
        //                attackList.teleport(Orientation.Top);
        //                break;

        //            case Orientation.Top:
        //                attackList.teleport(Orientation.Right);
        //                break;

        //            case Orientation.Right:
        //                attackList.teleport(Orientation.Left);

        //                //TEST
        //                bossStats.TakePDamage(100);
        //                break;
        //        }
        //    }
        //}
    }

}

//ENUM
public enum Zones
{
    Melee,
    Left,
    Right,
    Middle,
    Back
}
