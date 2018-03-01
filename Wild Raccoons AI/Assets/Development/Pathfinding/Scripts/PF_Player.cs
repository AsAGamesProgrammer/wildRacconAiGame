using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Player : MonoBehaviour
{
    private float movespeed = 20f;
    private Vector3[] path;
    private int targetIndex;

    public Camera playerCam;

    private float clickCooldown = 0.5f;
    private float clickCooldownRemaining = 0f;

    private float newPathCooldown = 0.1f;
    private float newPathCooldownRemaining;

    private Vector3 targetPosition = Vector3.zero;
    private bool targetSet = false;
    private bool targetReached = false;

    public GameObject targetIndicator;

    private int debugEnumCount = 0;

    private void Update()
    {
        if(clickCooldown > 0f)
        {
            clickCooldown -= Time.deltaTime;
        }

        if(newPathCooldown > 0f)
        {
            newPathCooldownRemaining -= Time.deltaTime;
        }

        if(clickCooldownRemaining <= 0f)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow, 2f);

                RaycastHit hit;
                
                // 
                if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("Grass")))
                {
                    targetPosition = hit.point;

                    targetIndicator.transform.position = targetPosition;
                    targetIndicator.SetActive(true);

                    PF_AlgorithmManager.RequestPath(transform.position, targetPosition, OnPathFound);

                    targetSet = true;
                    targetReached = false;
                }

                clickCooldownRemaining = clickCooldown;
            }
        }

        if (newPathCooldownRemaining <= 0f)
        {
            if (targetSet && !targetReached)
            {
                PF_AlgorithmManager.RequestPath(transform.position, targetPosition, OnPathFound);

                newPathCooldownRemaining = newPathCooldown;
            }
        }
    }

    public void OnPathFound(Vector3[] newPath_, bool pathSuccessful_)
    {
        // If a path has been found, start to follow that path.
        if (pathSuccessful_)
        {
            path = newPath_;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        debugEnumCount++;

        // Make sure we aren't trying to path to our current position. (Clicking the same spot multiple times.)
        if(path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];

            while (true)
            {
                // Check if the current waypoint has been reached.
                if (transform.position == currentWaypoint)
                {
                    // Increment the target waypoint index.
                    targetIndex++;

                    // Check if the end of the path has been reached. If true, break out.
                    if (targetIndex >= path.Length)
                    {
                        targetSet = false;
                        targetReached = true;

                        targetIndicator.SetActive(false);

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
        else
        {
            targetSet = false;
            targetReached = true;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(path[i], Vector3.one * 1.5f);

                if (i == targetIndex)
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
