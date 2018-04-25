using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_MagicShield : Ability_Base
{
  #region Properties
  private bool isActive;
  public GameObject shield;
  private GameObject spawnedShield;
  #endregion

  // Use this for initialization
  #region Protected Functions
  protected override void KeyDown()
  {

  }

  /// <summary>
  /// Function to control ability when key is released
  /// </summary>
  protected override void KeyUp()
  {
    if (player.GetComponent<PF_Player>().GetCurrentMana() < 10) return;
    UseAbility();
  }

  /// <summary>
  /// Function to activate ability
  /// </summary>
  protected override void UseAbility()
  {
    player.GetComponent<PF_Player>().ActivePower = PF_Player.PlayerAbilities.Shield;
    if (isActive && spawnedShield == null)
      isActive = false;
    if (!isActive && spawnedShield == null)
    {
      spawnedShield = Instantiate(shield, abilitySpawn.transform.position, player.transform.rotation);
      Area_ManaShield Shield = spawnedShield.AddComponent<Area_ManaShield>();
      Shield.SetPlayer(player);
      spawnedShield.transform.parent = player.transform;
      isActive = true;
    }
    else if (isActive)
    {
      Destroy(spawnedShield);
      spawnedShield = null;
      isActive = false;
    }
  }
  #endregion
}
