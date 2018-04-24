using System.Collections.Generic;
using UnityEngine;

public class DecisionNodeClass : MonoBehaviour {
  #region properties
  public string Value;
  public Dictionary<float, DecisionNodeClass> Children;
  #endregion

  #region Constructor
  /// <summary>
  /// Function to create a decision node
  /// </summary>
  /// <param name="value"></param>
  public DecisionNodeClass(string value = "")
  {
    Value = value;
    Children = new Dictionary<float, DecisionNodeClass>();
  }
  #endregion
}
