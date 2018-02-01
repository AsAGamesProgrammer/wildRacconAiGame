using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Grid : MonoBehaviour
{
    public Transform player;

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize = Vector2.zero;
    public float nodeRadius = 0f;
    private PF_Node[,] grid;

    private float nodeDiameter = 1f;
    private int gridSizeX = 0;
    private int gridSizeY = 0;

    public List<PF_Node> path;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2f;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        // Generate the nodes which will comprise the grid.
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new PF_Node[gridSizeX, gridSizeY];

        // Get the world position for the bottom left of the grid.
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                // Get the world position for the current node.
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                // Determine if the node is walkable or should be considered obstructed.
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                // Update the node information within the grid for this particular node.
                grid[x, y] = new PF_Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<PF_Node> GetNeighbourNodes(PF_Node node_)
    {
        List<PF_Node> neighbourNodes = new List<PF_Node>();

        // Loop through and get all neighbouring nodes.
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node_.gridX + x;
                int checkY = node_.gridY + y;

                if (checkX >= 0 &&
                    checkX < gridSizeX &&
                    checkY >= 0 &&
                    checkY < gridSizeY)
                {
                    neighbourNodes.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbourNodes;
    }

    public PF_Node GetNodeFromWorldPoint(Vector3 _worldPos)
    {
        // Get the world position as a percentage of the entire grid.
        float percentX = (_worldPos.x - transform.position.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (_worldPos.z - transform.position.z + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }
    
    private void OnDrawGizmos()
    {
        // Wireframe to show grid boundaries in editor.
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if(grid != null)
        {
            PF_Node playerNode = GetNodeFromWorldPoint(player.position);

            foreach(PF_Node n in grid)
            {
                if(n.walkable)
                {
                    // Walkable.
                    Gizmos.color = Color.white;
                }
                else
                {
                    // Obstacle.
                    Gizmos.color = Color.red;
                }

                if(playerNode == n)
                {
                    // Player
                    Gizmos.color = Color.cyan;
                }

                if(path != null)
                {
                    if(path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }

                // Cubes to represent node positions.
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}


/*public APNode NodeFromWorldPosition(Vector3 worldPos)
{ // Move coordinates in right position
    float linearPosX = worldPos.x - worldBottomLeftCorner.x;
    float linearPosY = worldPos.z - worldBottomLeftCorner.z;

    // Get float value of positions in new coordinates 
    float floatPosX = linearPosX / nodeDiameter;
    float floatPosY = linearPosY / nodeDiameter;
    int x = (int)floatPosX; int y = (int)floatPosY;

    // Make sure that index values don't get out of grid bounds and take closest node to worldPos 
    x = Mathf.Clamp(x, 0, xNodeAmount - 1);
    y = Mathf.Clamp(y, 0, yNodeAmount - 1);
    return grid[x, y];
}﻿*/