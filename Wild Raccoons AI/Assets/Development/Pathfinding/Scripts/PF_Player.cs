using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PF_Player : MonoBehaviour
{
    private float currentHealth = 1000f;
    private float maxHealth = 1000f;
    private float currentHealthRegen = 0f;

    private float currentMana = 100f;
    private float maxMana = 100f;
    private float currentManaRegen = 5f;

    private int Invulnerability = 2;
    private bool invulnerable;

    private float baseMovespeed = 20f;
    private float currentMovespeed = 20f;

    private bool canMove = true;

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

    public Slider healthSlider;
    public Text healthText;

    public Slider manaSlider;
    public Text manaText;

    public PlayerAbilities ActivePower = PlayerAbilities.Arcane;

    public enum PlayerAbilities
    {
        Physical,
        Arcane,
        Teleport,
        Shield
    };

  private void Awake()
    {
        rangeIndicator = GameObject.FindGameObjectWithTag("Range Indicator");

        rangeIndicator.SetActive(false);

        currentHealth = 550f;
        currentMana = 40f;
    }

    private void ManaUpdate()
    {
        if (currentMana < 0)
            ModifyCurrentMana(0);
        if (currentMana < maxMana)
            ModifyCurrentMana(currentManaRegen);
    }

    private void Update()
    {
        clickCooldown -= Time.deltaTime;

        RefreshUI();

        ManaUpdate();

        newPathCooldownRemaining -= Time.deltaTime;

        if(clickCooldownRemaining <= 0f)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow, 2f);

                RaycastHit hit;
                
                // Make sure the click was somewhere on the map.
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
                    // If the player is not incapacitated.
                    if(canMove)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);

                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

                        Vector3 temp = transform.rotation.eulerAngles;

                        temp.z = 0;

                        transform.rotation = Quaternion.Euler(temp);

                        transform.Translate(Vector3.forward * Time.deltaTime * currentMovespeed, Space.Self);
                    }
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

    private void RefreshUI()
    {
        healthSlider.value = currentHealth;
        healthSlider.maxValue = maxHealth;
        healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

        manaSlider.value = currentMana;
        manaSlider.maxValue = maxMana;
        manaText.text = currentMana.ToString() + " / " + maxMana.ToString();
    }

    // Current Health
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public void SetCurrentHealth(float newHealthValue_)
    {
        currentHealth = newHealthValue_;
    }

    bool CheckIfNegitive(float modifyValue_)
    {
        return modifyValue_ < 0;
    }

    public void ModifyCurrentHealth(float modifyValue_)
    {
        if (CheckIfNegitive(modifyValue_) && invulnerable)
            return;
        currentHealth += modifyValue_;
        if (CheckIfNegitive(modifyValue_))
            StartCoroutine(InvulnerabilityTimer());

        // Check if the value is greater than the max health value.
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private IEnumerator InvulnerabilityTimer()
    {
        invulnerable = true;
        yield return new WaitForSeconds(Invulnerability);
        invulnerable = false;
    }

  // Max Health
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public void SetMaxHealth(float newHealthValue_)
    {
        maxHealth = newHealthValue_;
    }

    // Health Regen
    public float GetHealthRegen()
    {
        return currentHealthRegen;
    }
    public void SetHealthRegen(float newHealthValue_)
    {
        currentHealthRegen = newHealthValue_;
    }

    // Current Mana
    public float GetCurrentMana()
    {
        return currentMana;
    }
    public void SetCurrentMana(float newManaValue_)
    {
        currentMana = newManaValue_;
    }
    public void ModifyCurrentMana(float modifyValue_)
    {
        currentMana += modifyValue_;

        if(currentMana > maxMana)
        {
            currentMana = maxMana;
        }
    }

    // Max Mana
    public float GetMaxMana()
    {
        return maxMana;
    }
    public void SetMaxMana(float newManaValue_)
    {
        maxMana = newManaValue_;
    }

    // Mana Regen
    public float GetManaRegen()
    {
        return currentManaRegen;
    }
    public void SetManaRegen(float newManaValue_)
    {
        currentHealthRegen = newManaValue_;
    }

    // Base Movespeed
    public float GetBaseMovespeed()
    {
        return baseMovespeed;
    }
    public void SetBaseMovespeed(float newBaseMovespeed_)
    {
        baseMovespeed = newBaseMovespeed_;
    }

    // Current Movespeed
    public float GetCurrentMovespeed()
    {
        return currentManaRegen;
    }
    public void SetCurrentMovespeed(float newMovespeed_)
    {
        currentManaRegen = newMovespeed_;
    }

    // Movement Enabled?
    public bool GetCanMove()
    {
        return canMove;
    }
    public void SetCanMove(bool flag)
    {
        canMove = flag;
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
}
