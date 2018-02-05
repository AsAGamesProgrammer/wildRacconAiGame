using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Unit : MonoBehaviour
{

    public Transform target;

    private float movespeed = 25f;
    private Vector3[] path;
    private int targetIndex;

    private void Start()
    {
        PF_AlgorithmManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath_, bool pathSuccessful_)
    {
        // If a path has been found, start to follow that path.
        if(pathSuccessful_)
        {
            path = newPath_;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while(true)
        {
            // Check if the current waypoint has been reached.
            if(transform.position == currentWaypoint)
            {
                // Increment the target waypoint index.
                targetIndex++;

                // Check if the end of the path has been reached. If true, break out.
                if(targetIndex >= path.Length)
                {
                    yield break;
                }

                // Set the current waypoint to using the new target index.
                currentWaypoint = path[targetIndex];
            }

            // Move the unit towards the current waypoint.
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, movespeed * Time.deltaTime);

            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(path[i], Vector3.one * 1.5f);

                if(i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
