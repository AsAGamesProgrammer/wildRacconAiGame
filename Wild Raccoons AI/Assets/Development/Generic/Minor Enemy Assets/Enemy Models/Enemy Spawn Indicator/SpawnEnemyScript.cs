using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyScript : MonoBehaviour
{
    public GameObject enemy;

    private void Awake()
    {
        Invoke("SpawnEnemy", 1f);
    }

    private void SpawnEnemy()
    {
        Instantiate(enemy, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
