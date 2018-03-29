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

    // Player Reference
    public GameObject player;

    //Scripts
    Stats bossStats;

    //Melee attack
    public float meleeAttackRadius = 10;
    public Vector3 sphereGrowthVector = new Vector3(0.01f, 0.01f, 0.01f);
    public GameObject explosionSpherePrefab;    //prefab
    GameObject meleeAttack;     //Object

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

        leftHandController.CreateIndicator(player.transform.position);
    }

    //UPDATE
    private void Update()
    {
        //If a shield is on
        if(shieldEnabled)
        {
            //Decrease time
            currentTime -= Time.deltaTime;
            var material = physicalShield.GetComponent<Renderer>().material;
            var color = material.color;

            material.color = new Color(color.r, color.g, color.b, color.a - (fadePerSecond * Time.deltaTime));

            //When time is out
            if (currentTime <=0)
            {
                Destroy(physicalShield);        //Destroy a shield
                shieldEnabled = false;          //Set flag to false
                bossStats.pShieldEnabled = false;
            }
        }
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

        if (meleeAttack.transform.localScale.x < meleeAttackRadius)
        {
            meleeAttack.transform.localScale += sphereGrowthVector;
            return false;
        }
        else
        {
            Destroy(meleeAttack);
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
        if(!shieldEnabled)
        {
            //Instantiate
            physicalShield = Instantiate(physicalShieldPrefab, boss.transform);

            //Set flag
            shieldEnabled = true;
            bossStats.pShieldEnabled = true;

            //Set time
            currentTime = maxShieldTime;
        }
    }
}
