using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_ManaShield : MonoBehaviour {
  #region Properties
  private GameObject player;
  private float spellDrain = 5f;
  #endregion

  #region Public Functions
  /// <summary>
  /// Function to set the player
  /// </summary>
  /// <param name="p"></param>
  public void SetPlayer(GameObject p)
  {
    player = p;
  }
  #endregion

  #region Private Functions
  /// <summary>
  /// Function to check collision  with bullets
  /// </summary>
  /// <param name="other"></param>
  void OnTriggerEnter(Collider other)
  {
    if (other.tag != "Bullet")
      return;
    var collisionScript = other.gameObject.GetComponent<BulletScript>();
    collisionScript.direction = -collisionScript.direction;
  }

  /// <summary>
  /// Function to destroy shield when mana is empty
  /// </summary>
  void Update ()
  {
	  player.GetComponent<PF_Player>().ModifyCurrentMana(-spellDrain);
    if(player.GetComponent<PF_Player>().GetCurrentMana()<=0)
      Destroy(gameObject);
  }
  #endregion
}
