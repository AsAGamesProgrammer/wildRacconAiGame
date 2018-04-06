using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    private PF_Player playerScript;

    public float explosionDamage = 150f;

    private bool damageDone = false;

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

                damageDone = true;
            }
        }
    }
}
