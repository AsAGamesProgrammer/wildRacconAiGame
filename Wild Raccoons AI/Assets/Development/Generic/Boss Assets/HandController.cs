using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public GameObject spawnIndicator;

    public void CreateIndicator(Vector3 pos_)
    {
        pos_.y = 0.007f;

        Instantiate(spawnIndicator, pos_, Quaternion.identity);

        // Play some hand animation.
    }
}
