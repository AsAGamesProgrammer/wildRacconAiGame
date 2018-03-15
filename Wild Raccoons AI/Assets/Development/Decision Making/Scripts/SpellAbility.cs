using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAbility : CharacterAbility{

  public int spellDamage = 1;
  public float spellRange = 50f;
  public float spellForce = 100f;
  public Color laserColor = Color.white;

  private MagicShotTrigger rcShoot;

  public override void Initialize(GameObject obj)
  {
    rcShoot = obj.GetComponent<MagicShotTrigger>();
    rcShoot.Initialize();

    rcShoot.spellDamage = spellDamage;
    rcShoot.spellRange = spellRange;
    rcShoot.spellForce = spellForce;
    rcShoot.castingLine.material = new Material(Shader.Find("Unlit/Color"));
    rcShoot.castingLine.material.color = laserColor;

  }

  public override void ExecuteAbility()
  {
    rcShoot.Fire();
  }

}
