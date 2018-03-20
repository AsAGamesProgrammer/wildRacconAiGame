using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup_Speed : MonoBehaviour
{
    private float healthValue = 400f;

    private PF_Player playerScript;

    private void Awake()
    {
        playerScript = GameObject.FindObjectOfType<PF_Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Speed Powerup");

            ApplyPowerup();

            Destroy(gameObject);
        }
    }

    private void ApplyPowerup()
    {
        
    }
}
