using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Node
{
    public bool walkable = false;
    public Vector3 worldPos = Vector3.zero;

    public PF_Node(bool _walkable, Vector3 _worldPos)
    {
        walkable = _walkable;
        worldPos = _worldPos;
    }
}
