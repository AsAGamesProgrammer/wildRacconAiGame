using System.Collections.Generic;
using UnityEngine;

public class DecisionNodeClass : MonoBehaviour {

	public string Value;
  public Dictionary<float, DecisionNodeClass> Children;

  public DecisionNodeClass(string value = "")
  {
    Value = value;
    Children = new Dictionary<float, DecisionNodeClass>();
  }
}
