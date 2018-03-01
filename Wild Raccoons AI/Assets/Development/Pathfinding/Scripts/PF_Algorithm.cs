using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class PF_Algorithm : MonoBehaviour
{
    private PF_AlgorithmManager requestManager;

    private PF_Grid grid;

    private void Awake()
    {
        requestManager = GetComponent<PF_AlgorithmManager>();
        grid = GetComponent<PF_Grid>();
    }

    public void StartFindPath(Vector3 startPos_, Vector3 endPos_)
    {
        StartCoroutine(FindPath(startPos_, endPos_));
    }

    IEnumerator FindPath(Vector3 startPos_, Vector3 endPos_)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        // Get the start and end nodes.
        PF_Node startNode = grid.GetNodeFromWorldPoint(startPos_);
        PF_Node endNode = grid.GetNodeFromWorldPoint(endPos_);

        startNode.parentNode = startNode;

        // Only search if start and end are both walkable nodes.
        if(startNode.walkable && endNode.walkable)
        {
            // Create two lists of nodes.
            // Open list will contain nodes to be evaluated.
            // Closed list will contain nodes that have already been evaluated.
            PF_Heap<PF_Node> openSet = new PF_Heap<PF_Node>(grid.MaxSize);
            HashSet<PF_Node> closedSet = new HashSet<PF_Node>();

            // Add the starting node to our open list.
            openSet.Add(startNode);

            // Loop through the openSet.
            while (openSet.Count > 0)
            {
                // Begin with the first element in the list.
                PF_Node currentNode = openSet.RemoveFirst();

                // Add the starting node to the closed set.
                closedSet.Add(currentNode);

                // Path has been found.
                if (currentNode == endNode)
                {
                    sw.Stop();

                    //UnityEngine.Debug.Log("Path found: " + sw.ElapsedMilliseconds + "ms");

                    pathSuccess = true;

                    break;
                }

                foreach (PF_Node neighbour in grid.GetNeighbourNodes(currentNode))
                {
                    // Check if the neighbour is not walkable, or already in the closed list.
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    // Get the cost to move to the neighbour.
                    int newNeighbourMoveCost = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;

                    // If the new move cost is smaller than the current, or the open list does not contain this neighbour.
                    if (newNeighbourMoveCost < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        // Update the gCost and hCost.
                        neighbour.gCost = newNeighbourMoveCost;
                        neighbour.hCost = GetDistance(neighbour, endNode);

                        // Update the parent node.
                        neighbour.parentNode = currentNode;

                        // Add it to the open set if it is not currently there.
                        if (!openSet.Contains(neighbour))
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

        yield return null;

        if(pathSuccess)
        {
            waypoints = CalculatePath(startNode, endNode);
        }

        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    private Vector3[] CalculatePath(PF_Node startNode_, PF_Node endNode_)
    {
        List<PF_Node> path = new List<PF_Node>();

        // Begin at the end node.
        PF_Node currentNode = endNode_;

        // Loop through from the end node to the start node and construct the path.
        while(currentNode != startNode_)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        // Simplify the path by removing duplicate commands from adjacent nodes when direction does not change.
        Vector3[] waypoints = SimplifyPath(path);

        // Reverse the waypoints to get it from start to end.
        Array.Reverse(waypoints);

        return waypoints;
    }

    Vector3[] SimplifyPath(List<PF_Node> path_)
    {
        List<Vector3> waypoints = new List<Vector3>();

        Vector2 directionOld = Vector2.zero;

        // Loop through the waypoints in the path and ignore duplicate changes in direction.
        for(int i = 1; i < path_.Count; i++)
        {
            Vector2 directionNew = new Vector2(path_[i - 1].gridX - path_[i].gridX, path_[i - 1].gridY - path_[i].gridY);

            // When the direction changes, add the node to the path.
            if(directionNew != directionOld)
            {
                waypoints.Add(path_[i].worldPos);
            }

            // Update the current direction;
            directionOld = directionNew;
        }

        return waypoints.ToArray();
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
