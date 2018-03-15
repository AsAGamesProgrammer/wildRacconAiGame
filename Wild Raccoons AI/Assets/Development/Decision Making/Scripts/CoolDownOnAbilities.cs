using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownOnAbilities : MonoBehaviour {

  public string abilityButtonAxisName = "Fire1";

  [SerializeField] private CharacterAbility ability;
  [SerializeField] private GameObject weaponHolder;
  private float coolDownDuration;
  private float nextReadyTime;
  private float coolDownTimeLeft;


  void Start()
  {
    Initialize(ability, weaponHolder);
  }

  public void Initialize(CharacterAbility selectedAbility, GameObject weaponHolder)
  {
    ability = selectedAbility;
    coolDownDuration = ability.coolDownTime;
    ability.Initialize(weaponHolder);
    //AbilityReady();
  }

  // Update is called once per frame
  void Update()
  {
    bool coolDownComplete = (Time.time > nextReadyTime);
    if (coolDownComplete)
    {
      //AbilityReady();
      if (Input.GetButtonDown(abilityButtonAxisName))
      {
        ButtonTriggered();
      }
    }
    else
    {
      CoolDown();
    }
  }

  //private void AbilityReady()
  //{
  //}

  private void CoolDown()
  {
    coolDownTimeLeft -= Time.deltaTime;
  }

  private void ButtonTriggered()
  {
    nextReadyTime = coolDownDuration + Time.time;
    coolDownTimeLeft = coolDownDuration;
    ability.ExecuteAbility();
  }
}
