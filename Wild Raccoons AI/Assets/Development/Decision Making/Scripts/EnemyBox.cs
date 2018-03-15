using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBox : MonoBehaviour
{
  public int enemyHealth = 100;
  public void Damage(int damage)
  {
    enemyHealth -= damage;
  }

  void Update()
  {
    if (enemyHealth <= 0)
    {
      Destroy(gameObject);
    }
  }
}
