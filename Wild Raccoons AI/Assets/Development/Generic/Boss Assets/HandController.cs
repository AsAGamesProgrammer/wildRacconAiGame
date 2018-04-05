using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandState
{
    Idle,
    Fired,
    Retract
}

public class HandController : MonoBehaviour
{
    public GameObject handIndicatorPrefab;
    private GameObject activeHandIndicator;

    public GameObject spawnIndicatorPrefab;

    private PF_Player playerScript;

    private HandState currentHandState = HandState.Idle;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private Vector3 targetPosition;
    private Vector3 targetDirection;

    private float firePower = 0.035f;

    private float modifierDistance = 0f;
    private float currentDistance = 0f;
    private float maxDistance = 110f;

    public float grabDamage = 250f;

    private void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        if(currentHandState == HandState.Fired)
        {
            if(currentDistance < maxDistance)
            {
                modifierDistance = targetDirection.magnitude * firePower;

                currentDistance += modifierDistance;

                transform.position += targetDirection * firePower;
            }
            else
            {
                currentHandState = HandState.Retract;
            }
        }

        if(currentHandState == HandState.Retract)
        {
            if(currentDistance > 0)
            {
                modifierDistance = targetDirection.magnitude * firePower;

                currentDistance -= modifierDistance;

                transform.position += -targetDirection * firePower;
            }
            else
            {
                transform.position = originalPosition;
                transform.rotation = originalRotation;

                if(playerScript != null)
                {
                    playerScript.gameObject.transform.parent = null;

                    playerScript.SetCanMove(true);
                }

                currentDistance = 0f;

                currentHandState = HandState.Idle;
            }
        }
    }

    public void CreateIndicator(Vector3 pos_)
    {
        pos_.y = 0.007f;

        Instantiate(spawnIndicatorPrefab, pos_, Quaternion.identity);
    }

    public void InitiateGrab(Vector3 pos_)
    {
        targetPosition = pos_;

        activeHandIndicator = Instantiate(handIndicatorPrefab, pos_, Quaternion.identity);

        Invoke("FireGrab", 1f);
    }

    private void FireGrab()
    {
        if(currentHandState == HandState.Idle)
        {
            Destroy(activeHandIndicator);

            targetDirection = targetPosition - transform.position;

            Vector3.Normalize(targetDirection);

            transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            currentHandState = HandState.Fired;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(currentHandState == HandState.Fired)
        {
            if (other.tag == "Player")
            {
                playerScript = other.transform.parent.GetComponent<PF_Player>();

                playerScript.SetCanMove(false);

                playerScript.ModifyCurrentHealth(-grabDamage);

                other.transform.parent.transform.parent = transform;

                currentHandState = HandState.Retract;
            }
        }
    }
}
