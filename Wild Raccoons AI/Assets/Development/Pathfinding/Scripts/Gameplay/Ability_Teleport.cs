using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability_Teleport : Ability_Base
{
    // cooldown
    // cooldownRemaining
    // range

    private Vector3 targetPosition;

    protected override void KeyDown()
    {
        rangeIndicator.transform.localScale = new Vector3(range, 1, range);

        rangeIndicator.SetActive(true);
    }

    protected override void KeyUp()
    {
        rangeIndicator.SetActive(false);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow, 2f);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("Grass")))
        {
            targetPosition = hit.point;

            UseAbility();
        }
    }

    protected override void UseAbility()
    {
        // Ignore the y values and work on the xz plane.
        targetPosition.y = 0;

        Vector3 playerPosition = player.transform.position;
        playerPosition.y = 0;

        particleEffect.transform.position = player.transform.position;
        particleEffect.GetComponent<ParticleSystem>().Play();

        // Check if casting within range.
        if (Vector3.Distance(playerPosition, targetPosition) <= range)
        {
            player.transform.position = targetPosition;
        }
        // If not, use the closest valid location.
        else if(Vector3.Distance(playerPosition, targetPosition) > range)
        {

        }

        cooldownRemaining = cooldown;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawSphere(player.transform.position, range);
    }
}
