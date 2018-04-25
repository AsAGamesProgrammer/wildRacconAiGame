using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TARGET: Bullet prefab
/// Moves the bullet forward
/// </summary>

public class BulletScript : MonoBehaviour {

    #region Properties
    //Script
    BulletPattern bulletPattern;

    //Boss
    GameObject Boss;

    //Movement
    public Vector3 direction;
    public float speed = 5;
    public float maxBossDistance = 20;

    //Damage
    public int damage = 20;

    //Player
    GameObject playerModel;
    PF_Player playerScript;
    #endregion

    #region Private Functions
    //START
    void Start ()
    {
        //Boss
        Boss = GameObject.FindGameObjectWithTag("Boss");

        //Bullet pattern
        bulletPattern = GameObject.FindGameObjectWithTag("BossAbilities").GetComponent<BulletPattern>();

        //Player
        GameObject[] playerTagged = GameObject.FindGameObjectsWithTag("Player");
        foreach(var foundObject in playerTagged)
        {
            switch(foundObject.name)
            {
                case "Player":
                    playerScript = foundObject.GetComponent<PF_Player>();
                    break;

                case "Player Model":
                    playerModel = foundObject;
                    break;

                default:
                    break;
            }
        }
	  }
	
	  //UPDATE
	  void Update ()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (Vector3.Distance(this.transform.position, Boss.transform.position) > maxBossDistance)
        {
            //Send msg to bullet pattern
            bulletPattern.destroyedBullets++;

            Destroy(this.gameObject);
        }
	  }

    //DO DAMAGE
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == playerModel)
        {
            playerScript.ModifyCurrentHealth(damage * -1);
            //Craig Added Learning
            var learning = FindObjectOfType<BossAttacks>();
            learning.GetComponent<BossAttacks>().BlockLearn.Data.Add(new NaiveBayesLearning.InformationModel()
            {
              Lable = "Bullet",
              Features = new List<string>() { "Hit", "Attack" }
            });
            //Send msg to bullet pattern
            bulletPattern.destroyedBullets++;

            Destroy(this.gameObject);
        }
    }
    #endregion
}
