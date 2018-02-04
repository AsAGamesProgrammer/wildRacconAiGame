using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Node : IHeapItem<PF_Node>
{
    public bool walkable = false;
    public Vector3 worldPos = Vector3.zero;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    private int fCost;

    public PF_Node parentNode;

    int heapIndex;

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

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }

        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(PF_Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        // If the two fCosts are equal, use the hCost as a tiebreaker.
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        // Return 1 if lower rather than if higher.
        return -compare;
    }
}
