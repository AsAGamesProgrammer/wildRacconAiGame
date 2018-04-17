using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Development.Learning.Scripts;
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
    public BulletPattern bulletPattern;

    // Player Reference
    public GameObject player;

    //Scripts
    Stats bossStats;

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

    //Learning
    public NaiveBayesLearning BlockLearn;
    private bool phase1;
    private bool phase2;
    private bool phase3;


    //START
    private void Start()
    {
        BlockLearn = new NaiveBayesLearning();
        //Get scripts
        bossStats = GetComponent<Stats>();
        //Bullet pattern
        bulletPattern = GameObject.FindGameObjectWithTag("BossAbilities").GetComponent<BulletPattern>();

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
    }

    private void IncreaseAbilityDamage(string key)
    {
        switch (key)
        {
            case "Bullet":
                var masterBullet = GameObject.FindObjectOfType<BulletPattern>();
                var updateBullet = masterBullet.bulletPrefab.GetComponent<BulletScript>();
                updateBullet.damage *= 2;
                break;
            case "Melee":
                var masterMelee = GameObject.FindObjectOfType<MeleeAttack>();
                var updateMelee = masterMelee.explosionSpherePrefab.GetComponent<MeleeAttackHit>();
                updateMelee.damage *= 2;
                break;
            case "Hand":
                leftHandController.grabDamage *= 2; 
                rightHandController.grabDamage *= 2;
                break;
            case "Spawn":
                var child = leftHandController.spawnIndicatorPrefab.GetComponent<SpawnEnemyScript>();
                var updateChild = child.enemy.GetComponentInChildren<EnemyExplosion>();
                updateChild.explosionDamage *= 2;
                break;
        }
    }

    //UPDATE
    private void Update()
    {
        if(bossStats.health< (bossStats.maxHealth/4)* 3 && !phase1)
        {
            BlockLearn.BlockLearn();
            var results = BlockLearn.Classify(new List<string>() { "Hit", "Attack" });
            var max = results.Keys.Min();
            IncreaseAbilityDamage(max);
            phase1 = true;
        }
        else if (bossStats.health < (bossStats.maxHealth / 4) * 2 && !phase2)
        {
            BlockLearn.BlockLearn();
            var results = BlockLearn.Classify(new List<string>() { "Hit", "Attack" });
            var max = results.Keys.Min();
            IncreaseAbilityDamage(max);
            phase2 = true;
        }
        else if (bossStats.health < (bossStats.maxHealth / 4) && !phase3)
        {
            BlockLearn.BlockLearn();
            var results = BlockLearn.Classify(new List<string>() { "Hit", "Attack" });
            var max = results.Keys.Min();
            IncreaseAbilityDamage(max);
            phase3 = true;
        }
    }

    //MELEE
    public void AttackMelee()
    {
        meleeAttackScript.performMeleeAttack();
        BlockLearn.Data.Add(new NaiveBayesLearning.InformationModel()
        {
          Lable = "Melee",
          Features = new List<string>() { "Initiated", "Attack"}
        });
    }

    //SHOOT
    //Creates a bullet with given direction, speed and maximum distance from the boss
    public void shootThree()
    {
        bulletPattern.ShootThreeBullets();
        BlockLearn.Data.Add(new NaiveBayesLearning.InformationModel()
        {
          Lable = "Bullet",
          Features = new List<string>() { "Initiated", "Attack" }
        });
    }

    public void shootFive()
    {
        bulletPattern.ShootFiveBullets();
        BlockLearn.Data.Add(new NaiveBayesLearning.InformationModel()
        {
          Lable = "Bullet",
          Features = new List<string>() { "Initiated", "Attack" }
        });
    }


    //SWITCH SIDES
    //Relocates the boss to one out of three predetrmined position
    public void teleport(Orientation orientation)
    {
        boss.transform.position = orientationDictionary[orientation].position;  //Position
        boss.transform.rotation = orientationDictionary[orientation].rotation;  //Rotation
        BlockLearn.Data.Add(new NaiveBayesLearning.InformationModel()
        {
          Lable = "Teleport",
          Features = new List<string>() { "Initiated", "Travel" }
        });
        //Update stats
        bossStats.setOrientation(orientation);
    }

    //PHYSICAL SHIELD
    //Apply physical shield
    public void applyPhysicalShield()
    {
        shieldScript.applyShield(shieldType.Physycal);
        BlockLearn.Data.Add(new NaiveBayesLearning.InformationModel()
        {
          Lable = "Physical Shield",
          Features = new List<string>() { "Initiated", "Shield" }
        });
    }

    //Magical
    public void applyMagicalShield()
    {
        shieldScript.applyShield(shieldType.Magical);
        BlockLearn.Data.Add(new NaiveBayesLearning.InformationModel()
        {
          Lable = "Magic Shield",
          Features = new List<string>() { "Initiated", "Shield" }
        });
    }

    //HANDS
    //Left hand
    public void shootLeftHand()
    {
        leftHandController.InitiateGrab(player.transform.position);
        BlockLearn.Data.Add(new NaiveBayesLearning.InformationModel()
        {
          Lable = "Left Hand",
          Features = new List<string>() { "Initiated", "Pull" }
        });
    }

    //Right hand
    public void shootRightHand()
    {
        rightHandController.InitiateGrab(player.transform.position);
        BlockLearn.Data.Add(new NaiveBayesLearning.InformationModel()
        {
          Lable = "Right Hand",
          Features = new List<string>() { "Initiated", "Pull" }
        });
    }

    //SPAWN ENEMIES
    public void spawnEnemiesLeft()
    {
        leftHandController.CreateIndicator(player.transform.position);
        BlockLearn.Data.Add(new NaiveBayesLearning.InformationModel()
        {
          Lable = "Spawn",
          Features = new List<string>() { "Initiated", "Attack" }
        });
    }

    public void spawnEnemiesRight()
    {
        rightHandController.CreateIndicator(player.transform.position);
        BlockLearn.Data.Add(new NaiveBayesLearning.InformationModel()
        {
          Lable = "Spawn",
          Features = new List<string>() { "Initiated", "Attack" }
        });
    }

}
