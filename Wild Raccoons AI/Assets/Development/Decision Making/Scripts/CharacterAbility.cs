using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAbility : ScriptableObject
{
  public string abilityName = "New Ability";
  public float coolDownTime = 1f;
  public abstract void Initialize(GameObject obj);
  public abstract void ExecuteAbility();
}
