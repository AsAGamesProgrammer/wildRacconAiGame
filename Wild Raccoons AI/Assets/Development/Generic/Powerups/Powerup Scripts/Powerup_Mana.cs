using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Mana : MonoBehaviour
{
    private float manaValue = 50f;

    private PF_Player playerScript;

    private void Awake()
    {
        playerScript = GameObject.FindObjectOfType<PF_Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Mana Powerup");

            ApplyPowerup();

            Destroy(gameObject);
        }
    }

    private void ApplyPowerup()
    {
        playerScript.ModifyCurrentMana(manaValue);
    }
}
