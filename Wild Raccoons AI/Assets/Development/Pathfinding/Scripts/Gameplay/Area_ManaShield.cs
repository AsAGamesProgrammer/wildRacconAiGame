using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_ManaShield : MonoBehaviour {
  private GameObject player;

  private float spellDrain = 5f;
  // Use this for initialization

  public void SetPlayer(GameObject p)
  {
    player = p;
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.tag != "Bullet")
      return;
    var collisionScript = other.gameObject.GetComponent<BulletScript>();
    collisionScript.direction = -collisionScript.direction;
  }

  // Update is called once per frame
  void Update () {
	  player.GetComponent<PF_Player>().ModifyCurrentMana(-spellDrain);
    if(player.GetComponent<PF_Player>().GetCurrentMana()<=0)
      Destroy(gameObject);
  }
}
