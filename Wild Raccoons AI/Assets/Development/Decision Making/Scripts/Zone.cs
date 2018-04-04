using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 TARGET: Each zone 
 ACTION REQUIRED: Set thisZone in the inspector
     
     */

public class Zone : MonoBehaviour {

    public Zones thisZone;

    //Script
    AttackManager bossScript;

	// Use this for initialization
	void Start ()
    {
        bossScript = GameObject.FindGameObjectWithTag("BossManager").GetComponent<AttackManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log(thisZone);
            bossScript.currentPlayerZone = thisZone;
        }
    }
}
