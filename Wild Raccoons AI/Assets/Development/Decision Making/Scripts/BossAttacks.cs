using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This scripts contains an implementation of boss attacks

public class BossAttacks : MonoBehaviour {

    //Boss
    public GameObject boss;

    // Boss Hand Scripts
    public HandController leftHandController;
    public HandController rightHandController;

    //ATTACKS
    public MeleeAttack meleeAttackScript;      //Melee
    public Shield shieldScript;                //Shield


    // Player Reference
    public GameObject player;

    //Scripts
    Stats bossStats;

    ////Melee attack
    //public float meleeAttackRadius = 10;
    //public Vector3 sphereGrowthVector = new Vector3(0.01f, 0.01f, 0.01f);
    //public GameObject explosionSpherePrefab;    //prefab
    //GameObject meleeAttack;     //Object

    //Shooting
    public GameObject bulletPrefab;

    //Orientation
    Dictionary<Orientation, Transform> orientationDictionary = new Dictionary<Orientation, Transform>();

    //Shields
    bool shieldEnabled = false;
    public GameObject physicalShieldPrefab;
    GameObject physicalShield;              //actual shield
    public float maxShieldTime = 5f;
    public float fadePerSecond = 1f;

    //Time
    float currentTime = 0f;

    //Flags
    bool attackPrepared = false;
    bool attackExecuting = false;
    bool attackFinished = false;


    //START
    private void Start()
    {
        //Get scripts
        bossStats = GetComponent<Stats>();

        //Populate dictionary
        GameObject[] positions = GameObject.FindGameObjectsWithTag("Orientation");
        foreach (var position in positions)
        {
            switch(position.name)
            {
                case "Top":
                    orientationDictionary.Add(Orientation.Top, position.transform);
                    break;

                case "Left":
                    orientationDictionary.Add(Orientation.Left, position.transform);
                    break;

                case "Right":
                    orientationDictionary.Add(Orientation.Right, position.transform);
                    break;
            }
        }

        //leftHandController.CreateIndicator(player.transform.position);
    }

    //UPDATE
    private void Update()
    {

    }

    //MELEE
    public void AttackMelee()
    {
        meleeAttackScript.performMeleeAttack();
    }

    //SHOOT
    //Creates a bullet with given direction, speed and maximum distance from the boss
    public void shootAt(Vector3 direction)
    {
        GameObject newBullet = Instantiate(bulletPrefab, boss.transform);
        newBullet.GetComponent<BulletScript>().direction = direction;
    }


    //SWITCH SIDES
    //Relocates the boss to one out of three predetrmined position
    public void teleport(Orientation orientation)
    {
        boss.transform.position = orientationDictionary[orientation].position;  //Position
        boss.transform.rotation = orientationDictionary[orientation].rotation;  //Rotation

        //Update stats
        bossStats.setOrientation(orientation);
    }

    //PHYSICAL SHIELD
    //Apply physical shield
    public void applyPhysicalShield()
    {
        shieldScript.applyShield(shieldType.Physycal);
    }

    //Magical
    public void applyMagicalShield()
    {
        shieldScript.applyShield(shieldType.Magical);
    }

    //*********TODO**********
    //HANDS
    //Left hand
    public void shootLeftHand()
    {
        //shoot
    }

    //Right hand
    public void shootRightHand()
    {
        //shoot
    }
    //***********************

    //SPAWN ENEMIES
    public void spawnEnemiesLeft()
    {
        leftHandController.CreateIndicator(player.transform.position);
    }

    public void spawnEnemiesRight()
    {
        rightHandController.CreateIndicator(player.transform.position);
    }

}
