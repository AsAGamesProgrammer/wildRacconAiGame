using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Unit : MonoBehaviour
{
    public Transform target;

    public float movespeed = 25f;
    public float turnSpeed = 3f;
    public float turnDistance = 5f;

    public PF_Path path;

    private float newPathCooldown = 0.5f;
    private float newPathCooldownRemaining;

    private bool targetSet = false;
    private bool targetReached = false;

    private void Update()
    {
        if (newPathCooldown > 0f)
        {
            newPathCooldownRemaining -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            PF_AlgorithmManager.RequestPath(transform.position, target.position, OnPathFound);

            targetSet = true;
            targetReached = false;
        }

        if (newPathCooldownRemaining <= 0f)
        {
            if (targetSet && !targetReached)
            {
                PF_AlgorithmManager.RequestPath(transform.position, target.position, OnPathFound);

                newPathCooldownRemaining = newPathCooldown;
            }
        }
    }

    // Callback for the path request.
    public void OnPathFound(Vector3[] waypoints_, bool pathSuccessful_)
    {
        // If a path has been found, start to follow that path.
        if(pathSuccessful_)
        {
            path = new PF_Path(waypoints_, transform.position, turnDistance);

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        if(path.lookPoints.Length > 0)
        {
            bool followingPath = true;
            int pathIndex = 0;

            // Look at the first path node.
            transform.LookAt(path.lookPoints[0]);

            while (followingPath)
            {
                Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);

                // 
                while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
                {
                    // Break out of the while loop if the path is finished.
                    if (pathIndex == path.finishLineIndex)
                    {
                        followingPath = false;

                        targetSet = false;
                        targetReached = true;

                        break;
                    }
                    else
                    {
                        pathIndex++;
                    }
                }

                if (followingPath)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);

                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

                    Vector3 temp = transform.rotation.eulerAngles;

                    temp.z = 0;

                    transform.rotation = Quaternion.Euler(temp);

                    transform.Translate(Vector3.forward * Time.deltaTime * movespeed, Space.Self);
                }

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
        if(path != null)
        {
            path.DrawWithGizmos();
        }
    }
}
