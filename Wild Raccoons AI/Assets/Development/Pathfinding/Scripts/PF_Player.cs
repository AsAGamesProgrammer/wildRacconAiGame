using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Player : MonoBehaviour
{
    private float movespeed = 20f;
    public float turnSpeed = 3f;
    public float turnDistance = 5f;

    private PF_Path path;

    private float clickCooldown = 0.5f;
    private float clickCooldownRemaining = 0f;

    private float newPathCooldown = 0.1f;
    private float newPathCooldownRemaining;

    private Vector3 targetPosition = Vector3.zero;
    private bool targetSet = false;
    private bool targetReached = false;

    public GameObject targetIndicator;

    private GameObject rangeIndicator;

    private void Awake()
    {
        rangeIndicator = GameObject.FindGameObjectWithTag("Range Indicator");

        rangeIndicator.SetActive(false);
    }

    private void Update()
    {
        clickCooldown -= Time.deltaTime;

        newPathCooldownRemaining -= Time.deltaTime;

        if(clickCooldownRemaining <= 0f)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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

    public void OnPathFound(Vector3[] waypoints_, bool pathSuccessful_)
    {
        // If a path has been found, start to follow that path.
        if (pathSuccessful_)
        {
            path = new PF_Path(waypoints_, transform.position, turnDistance);

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        if (path.lookPoints.Length > 0)
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

    public void CancelMovement()
    {
        StopCoroutine("FollowPath");

        targetSet = false;
        targetReached = true;
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
}
