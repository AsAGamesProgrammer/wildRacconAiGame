using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PF_Algorithm : MonoBehaviour
{

    public Transform seeker;
    public Transform target;

    private PF_Grid grid;

    private void Awake()
    {
        grid = GetComponent<PF_Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindPath(seeker.position, target.position);
        }
    }

    private void FindPath(Vector3 startPos_, Vector3 endPos_)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        // Get the start and end nodes.
        PF_Node startNode = grid.GetNodeFromWorldPoint(startPos_);
        PF_Node endNode = grid.GetNodeFromWorldPoint(endPos_);

        // Create two lists of nodes.
        // Open list will contain nodes to be evaluated.
        // Closed list will contain nodes that have already been evaluated.
        PF_Heap<PF_Node> openSet = new PF_Heap<PF_Node>(grid.MaxSize);
        HashSet<PF_Node> closedSet = new HashSet<PF_Node>();

        // Add the starting node to our open list.
        openSet.Add(startNode);

        // Loop through the openSet.
        while(openSet.Count > 0)
        {
            UnityEngine.Debug.Log("Wewlads");

            // Begin with the first element in the list.
            PF_Node currentNode = openSet.RemoveFirst();

            closedSet.Add(currentNode);

            // Path has been found.
            if(currentNode == endNode)
            {
                sw.Stop();

                UnityEngine.Debug.Log("Path found: " + sw.ElapsedMilliseconds + "ms");

                CalculatePath(startNode, endNode);

                return;
            }

            foreach(PF_Node neighbour in grid.GetNeighbourNodes(currentNode))
            {
                // Check if the neighbour is not walkable, or already in the closed list.
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                // Get the cost to move to the neighbour.
                int newNeighbourMoveCost = currentNode.gCost + GetDistance(currentNode, neighbour);

                // If the new move cost is smaller than the current, or the open list does not contain this neighbour.
                if(newNeighbourMoveCost < neighbour.gCost ||
                    !openSet.Contains(neighbour))
                {
                    // Update the gCost and hCost.
                    neighbour.gCost = newNeighbourMoveCost;
                    neighbour.hCost = GetDistance(neighbour, endNode);

                    // Update the parent node.
                    neighbour.parentNode = currentNode;

                    // Add it to the open set if it is not currently there.
                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    // Otherwise update it.
                    else
                    {
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
    }

    private void CalculatePath(PF_Node startNode_, PF_Node endNode_)
    {
        List<PF_Node> path = new List<PF_Node>();

        PF_Node currentNode = endNode_;

        // Loop through from the end node to the start node and construct the path.
        while(currentNode != startNode_)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        // Reverse the path to get it from start to end.
        path.Reverse();

        grid.path = path;
    }

    private int GetDistance(PF_Node nodeA, PF_Node nodeB)
    {
        // Get the x and y distances between the two nodes.
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        // Return the distance of the shortest path.
        if(distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        else
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }
}
