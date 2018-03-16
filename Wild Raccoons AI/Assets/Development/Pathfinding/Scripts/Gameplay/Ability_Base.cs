using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class Ability_Base : MonoBehaviour
{
    [SerializeField]
    protected GameObject player;
    [SerializeField]
    protected GameObject rangeIndicator;
    [SerializeField]
    protected GameObject abilitySpawn;

    protected float cooldownRemaining = 0f;
    [SerializeField]
    protected float cooldown = 0f;
    [SerializeField]
    protected float range = 50f;

    [SerializeField]
    protected Image abilityIcon;
    [SerializeField]
    protected Text cooldownText;

    public GameObject particleEffect;

    [SerializeField]
    protected KeyCode triggerKey;

    private bool debugVisuals = true;

	void Update ()
    {
        cooldownRemaining -= Time.deltaTime;

        if (cooldownRemaining <= 0f)
        {
            if (Input.GetKeyDown(triggerKey))
            {
                KeyDown();
            }

            if (Input.GetKeyUp(triggerKey))
            {
                KeyUp();
            }
        }

        updateUI();
	}

    protected void updateUI()
    {
        if (cooldownRemaining > 0)
        {
            cooldownText.text = Mathf.Ceil(cooldownRemaining).ToString();

            // Darken the icon. (Dark blue)
            abilityIcon.color = new Color(0, 128f / 255f, 1);
        }
        else
        {
            abilityIcon.color = Color.white;

            cooldownText.text = "";
        }
    }

    protected abstract void KeyDown();

    protected abstract void KeyUp();

    protected abstract void UseAbility();
}
