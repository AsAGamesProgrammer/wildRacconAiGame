using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackHit : MonoBehaviour {
  GameObject playerModel;
  PF_Player playerScript;
  public int damage = 20;
  // Use this for initialization
  void Start () {
    GameObject[] playerTagged = GameObject.FindGameObjectsWithTag("Player");
    foreach (var foundObject in playerTagged)
    {
      switch (foundObject.name)
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

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      playerScript.ModifyCurrentHealth(damage * -1);
      var learning = FindObjectOfType<BossAttacks>();
      learning.GetComponent<BossAttacks>().BlockLearn.Data.Add(new NaiveBayesLearning.InformationModel()
      {
        Lable = "Melee",
        Features = new List<string>() { "Hit", "Attack" }
      });
    }
  }
}
