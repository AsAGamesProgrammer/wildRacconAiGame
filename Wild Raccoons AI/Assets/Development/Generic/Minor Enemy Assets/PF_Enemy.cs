using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Enemy : MonoBehaviour
{
    private GameObject target;

    public float movespeed = 25f;
    public float turnSpeed = 3f;
    public float turnDistance = 5f;

    public float chaseTimer = 3f;

    public PF_Path path;

    private float newPathCooldown = 0.5f;
    private float newPathCooldownRemaining;

    private bool targetSet = false;
    private bool targetReached = false;

    public GameObject healthPickup;
    public GameObject manaPickup;
    public GameObject speedPickup;

    public GameObject explosionHitbox;
    public ParticleSystem particleSystem;

    private bool hasExploded = false;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");

        Invoke("SelfDestruct", chaseTimer);

        PF_AlgorithmManager.RequestPath(transform.position, target.transform.position, OnPathFound);
    }

    private void Update()
    {
        if (newPathCooldown > 0f)
        {
            newPathCooldownRemaining -= Time.deltaTime;
        }

        // Update the path. (Dynamic)
        if (newPathCooldownRemaining <= 0f)
        {
            PF_AlgorithmManager.RequestPath(transform.position, target.transform.position, OnPathFound);

            newPathCooldownRemaining = newPathCooldown;
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

                while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
                {
                    // Break out of the while loop if the path is finished.
                    if (pathIndex == path.finishLineIndex)
                    {
                        followingPath = false;

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
    }

    private void SelfDestruct()
    {
        if(!hasExploded)
        {
            particleSystem.Play();

            explosionHitbox.SetActive(true);

            StopCoroutine("FollowPath");
            newPathCooldownRemaining = 999f;

            Invoke("DestroySelf", 1f);

            hasExploded = true;
        }
    }

    private void DestroySelf()
    {
        float pickupType = Random.value;

        if(pickupType <= 0.25f)
        {
            Instantiate(healthPickup, transform.position, Quaternion.identity);
        }
        else if (pickupType > 0.25f && pickupType <= 0.5f)
        {
            Instantiate(manaPickup, transform.position, Quaternion.identity);
        }
        else if (pickupType > 0.5f && pickupType <= 0.75f)
        {
            Instantiate(speedPickup, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            path.DrawWithGizmos();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("wew");

            SelfDestruct();
        }
    }
}
