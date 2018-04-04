using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    //Flag
    bool shieldEnabled = false;

    //Prefabs
    public GameObject pShieldPrefab;
    public GameObject mShieldPrefab;

    //Created object
    GameObject shieldObject;              //actual shield

    //Public variables
    public float maxShieldTime = 5f;
    public float fadePerSecond = 1f;

    //Time
    float currentTime = 0f;

    //Boss
    public GameObject boss;

    //Scripts
    Stats bossStats;
    AttackManager attackManagerScript;

    //Type
    shieldType currentShieldType;

    // Use this for initialization
    void Start()
    {
        //Get scripts
        bossStats = GameObject.FindGameObjectWithTag("BossManager").GetComponent<Stats>();
        attackManagerScript = GameObject.FindGameObjectWithTag("BossManager").GetComponent<AttackManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //If a shield is on
        if (shieldEnabled)
        {
            //Decrease time
            currentTime -= Time.deltaTime;
            var material = shieldObject.GetComponent<Renderer>().material;
            var color = material.color;

            material.color = new Color(color.r, color.g, color.b, color.a - (fadePerSecond * Time.deltaTime));

            //When time is out
            if (currentTime <= 0)
            {
                Destroy(shieldObject);        //Destroy a shield
                shieldEnabled = false;          //Set flag to false
                bossStats.applyShield(currentShieldType, false);    //remove shield

                //Send msg to the state machine
                attackManagerScript.NextAttack = true;
            }
        }


    }

     //Apply physical shield
     public void applyShield(shieldType type)
     {
        currentShieldType = type;

        if (!shieldEnabled)
        {
            //Instantiate
            if(currentShieldType == shieldType.Physycal)
                shieldObject = Instantiate(pShieldPrefab, boss.transform);
            else
                shieldObject = Instantiate(mShieldPrefab, boss.transform);


            //Set flag
            shieldEnabled = true;
            bossStats.applyShield(currentShieldType, true);    //apply shield

            //Set time
            currentTime = maxShieldTime;
        }
     }
}

//ENUM
public enum shieldType
{
    Physycal,
    Magical
}
