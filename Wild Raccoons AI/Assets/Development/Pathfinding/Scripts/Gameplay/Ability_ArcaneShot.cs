using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_ArcaneShot : Ability_Base
{
    // cooldown
    // cooldownRemaining
    // range
    private Vector3 targetPosition;

    public GameObject projectile;

    protected override void KeyDown()
    {
        rangeIndicator.SetActive(true);
    }

    protected override void KeyUp()
    {
        rangeIndicator.SetActive(false);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow, 2f);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, 1 << LayerMask.NameToLayer("Click Plane")))
        {
            targetPosition = hit.point;

            UseAbility();
        }
    }

    protected override void UseAbility()
    {
        // Cancel movement for the player.
        player.GetComponent<PF_Player>().CancelMovement();

        // Calculate the direction of the shot.
        Vector3 shotDirection = targetPosition - player.transform.position;

        // Rotate the player towards the target, ignoring x and z components.
        player.transform.LookAt(targetPosition);

        Vector3 temp = player.transform.rotation.eulerAngles;

        temp.x = 0f;
        temp.z = 0f;

        player.transform.rotation = Quaternion.Euler(temp);

        // Spawn the projectile.
        GameObject spawnedProjectile = Instantiate(projectile, abilitySpawn.transform.position, player.transform.rotation);

        // Give the projectile the correct script.
        Projectile_ArcaneShot projectileScript = spawnedProjectile.AddComponent<Projectile_ArcaneShot>();

        // Modify initial script values.
        projectileScript.movespeed = 3f;
        projectileScript.lifeSpan = 1f;

        // Initiate the cooldown for the ability.
        cooldownRemaining = cooldown;
    }
}
