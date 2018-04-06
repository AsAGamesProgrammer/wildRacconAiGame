﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    //Script
    BossAttacks attackList;
    Stats bossStats;
    PF_Player playerScript;

    //Player
    //public GameObject playerModel;

    //Current zone
    public Zones currentPlayerZone;
    private Zones previouslyAttackedZone;   //To check if player moves

    //Next attack
    public bool NextAttack = true;

    //List of attacks
    List<BossActions> pastBossActions = new List<BossActions>();

    //Hands
    public GameObject leftHand;
    public GameObject rightHand;

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
                //MIDDLE
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

                //LEFT
                case (Zones.Left):
                    //If the last attack was not spawning enemies
                    if (pastBossActions[pastBossActions.Count - 1] != BossActions.SpawnEnemies)
                    {
                        Debug.Log("Spawn Enemies Left");
                        attackList.spawnEnemiesLeft();
                        pastBossActions.Add(BossActions.SpawnEnemies);

                    }
                    else //otherwise teleport
                    {
                        Debug.Log("Teleport");
                        TeleportHelper();
                        pastBossActions.Add(BossActions.Teleport);
                    }
                    break;

                //RIGHT
                case (Zones.Right):
                    //If the last attack was not spawning enemies
                    if (pastBossActions[pastBossActions.Count - 1] != BossActions.SpawnEnemies)
                    {
                        Debug.Log("Spawn Enemies Left");
                        attackList.spawnEnemiesRight();
                        pastBossActions.Add(BossActions.SpawnEnemies);

                    }
                    else //otherwise teleport
                    {
                        Debug.Log("Teleport");
                        TeleportHelper();
                        pastBossActions.Add(BossActions.Teleport);
                    }
                    break;

                //MIDDLE
                case (Zones.Middle):
                    //Shoot
                    if (pastBossActions[pastBossActions.Count - 1] != BossActions.ShootThree)
                    {
                        Debug.Log("Three shots");
                        attackList.shootThree();
                        pastBossActions.Add(BossActions.ShootThree);

                    }
                    else
                    {
                        Debug.Log("Five shots");
                        attackList.shootFive();
                        pastBossActions.Add(BossActions.ShootFive);
                    }
                    break;

                //BACK
                case (Zones.Back):
                    //Grab with the closest hand
                    if (Vector3.Distance(playerScript.gameObject.transform.position, leftHand.transform.position) >
                        Vector3.Distance(playerScript.gameObject.transform.position, rightHand.transform.position))
                    {
                        Debug.Log("Right hand grab");
                        attackList.shootRightHand();
                        pastBossActions.Add(BossActions.RightHandGrab);

                    }
                    else
                    {
                        Debug.Log("Left hand grab");
                        attackList.shootLeftHand();
                        pastBossActions.Add(BossActions.LeftHandGrab);
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
            //    }
            //}
        }

    //TELEPORT HELPER
    void TeleportHelper()
    {
        switch (bossStats.getOrientation())
        {
            case Orientation.Left:
                attackList.teleport(Orientation.Top);
                NextAttack = true;
                break;

            case Orientation.Top:
                attackList.teleport(Orientation.Right);
                NextAttack = true;
                break;

            case Orientation.Right:
                attackList.teleport(Orientation.Left);
                NextAttack = true;
                break;
        }
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
    ShootThree,
    ShootFive,
    None,
    SpawnEnemies,
    RightHandGrab,
    LeftHandGrab,
    Teleport
}

