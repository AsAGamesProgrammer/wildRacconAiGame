using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_PhysicalShot : MonoBehaviour {
  #region Properties
  public float movespeed = 0f;
  public float lifeSpan = 999f;
  private GameObject boss;
  #endregion

  #region Private Functions
  /// <summary>
  /// Set boss on start
  /// </summary>
  private void Start()
  {
    boss = GameObject.FindWithTag("BossManager");
  }

  /// <summary>
  /// Check collision with boss then do damage
  /// </summary>
  /// <param name="collision"></param>
  private void OnTriggerEnter(Collider collision)
  {
    if (collision.gameObject.tag != "Boss")
      return;
    boss.GetComponent<Stats>().TakePDamage(20);
    Destroy(gameObject);
  }

  /// <summary>
  /// Function to check the life span of bullet is greater than 0
  /// </summary>
  private void Update()
  {
    lifeSpan -= Time.deltaTime;

    if (lifeSpan > 0f)
    {
      transform.position += transform.forward * movespeed;
    }
    else
    {
      Destroy(gameObject);
    }
  }
  #endregion
}
