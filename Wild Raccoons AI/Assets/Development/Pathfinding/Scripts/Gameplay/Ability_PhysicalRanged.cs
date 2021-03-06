﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_PhysicalRanged : Ability_Base
{
  #region Properties
  // cooldown
  // cooldownRemaining
  // range
  private Vector3 targetPosition;
  public GameObject projectile;
  #endregion

  #region Protected Functions
  /// <summary>
  /// Function to control indicator on key pressed
  /// </summary>
  protected override void KeyDown()
  {
    rangeIndicator.SetActive(true);
  }

  /// <summary>
  /// Function to fire ability on key release
  /// </summary>
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

  /// <summary>
  /// Function to activate the ability
  /// </summary>
  protected override void UseAbility()
  {
    player.GetComponent<PF_Player>().ActivePower = PF_Player.PlayerAbilities.Physical;
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
    Projectile_PhysicalShot projectileScript = spawnedProjectile.AddComponent<Projectile_PhysicalShot>();

    // Modify initial script values.
    projectileScript.movespeed = 3f;
    projectileScript.lifeSpan = 1f;

    // Initiate the cooldown for the ability.
    cooldownRemaining = cooldown;
  }
  #endregion
}