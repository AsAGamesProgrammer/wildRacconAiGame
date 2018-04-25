using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    #region Properties

    private PF_Player playerScript;

    public float explosionDamage = 150f;

    private bool damageDone = false;
    #endregion

    #region Private Functions
    private void Awake()
    {
        playerScript = FindObjectOfType<PF_Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(!damageDone)
            {
                playerScript.ModifyCurrentHealth(-explosionDamage);
                //Craig Added Learning
                var learning = FindObjectOfType<BossAttacks>();
                damageDone = true;
                learning.BlockLearn.Data.Add(new NaiveBayesLearning.InformationModel()
                {
                  Lable = "Spawn",
                  Features = new List<string>() { "Hit", "Attack" }
                });
            }
        }
    }
    #endregion
}
