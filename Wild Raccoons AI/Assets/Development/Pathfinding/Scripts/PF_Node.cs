using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Node
{
    public bool walkable = false;
    public Vector3 worldPos = Vector3.zero;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    private int fCost;

    public PF_Node parentNode;

    public PF_Node(bool walkable_, Vector3 worldPos_, int gridX_, int gridY_)
    {
        walkable = walkable_;
        worldPos = worldPos_;
        gridX = gridX_;
        gridY = gridY_;
    }

    public int GetfCost()
    {
        return gCost + hCost;
    }
}
