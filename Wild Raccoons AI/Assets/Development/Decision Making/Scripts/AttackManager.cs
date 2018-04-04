using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    //Script
    BossAttacks attackList;
    Stats bossStats;
    PF_Player playerScript;

    //Player
    public GameObject playerModel;

    //Current zone
    public Zones currentPlayerZone;
    private Zones previouslyAttackedZone;   //To check if player moves

    //Next attack
    public bool NextAttack = true;

    //List of attacks
    List<BossActions> pastBossActions = new List<BossActions>();

    //Other
    public int averageManaCost = 20; //Average price for mana ability, used to select shields

	// Use this for initialization
	void Start ()
    {
        //Getting script information
        attackList = GetComponent<BossAttacks>();
        bossStats = GetComponent<Stats>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PF_Player>();

        //Adding initial past action
        pastBossActions.Add(BossActions.None);

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
            NextAttack = false;         //Attack performed
            previouslyAttackedZone = currentPlayerZone; //Remember location

            switch(currentPlayerZone)
            {
                //Player in Melee zone
                case (Zones.Melee):
                    //If the last attack was NOT melee attack melee
                    if (pastBossActions[pastBossActions.Count-1] != BossActions.Melee)
                    {
                        Debug.Log("Attacking melee");
                        attackList.AttackMelee();
                        pastBossActions.Add(BossActions.Melee);
                    }
                    else //else put shield
                        if(playerScript.GetCurrentMana() < averageManaCost) 
                        {
                            //Physycal shield if player has no mana 
                            Debug.Log("Physycal shield");
                            attackList.applyPhysicalShield();
                            pastBossActions.Add(BossActions.PShield);
                        }
                        else
                        {
                            //Magical shield if player has mana
                            Debug.Log("Magical shield");
                            attackList.applyMagicalShield();
                            pastBossActions.Add(BossActions.MShield);
                        }
                    break;


                default:
                    NextAttack = true;
                    break;
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

//------------------------ENUM--------------------------
//Zones
public enum Zones
{
    Melee,
    Left,
    Right,
    Middle,
    Back
}


//Attacks
public enum BossActions
{
    Melee,
    Minions,
    PShield,
    MShield,
    Shoot,
    None
}

