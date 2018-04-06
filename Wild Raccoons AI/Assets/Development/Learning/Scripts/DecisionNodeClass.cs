using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionNodeClass : MonoBehaviour {

	public string Value;
  public Dictionary<float, DecisionNodeClass> Children;

  public DecisionNodeClass(string value = "")
  {
    this.Value = value;
    Children = new Dictionary<float, DecisionNodeClass>();
  }
}
