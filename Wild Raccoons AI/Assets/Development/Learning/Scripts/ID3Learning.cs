using System.Collections.Generic;
using UnityEngine;

namespace Assets.Development.Learning.Scripts
{
  #region Enum
  /// <summary>
  /// Public enum of actions
  /// </summary>
  public enum Id3LearningAction
  {
    Attack, Defend, Stand
  }
  #endregion

  public class ID3Learning : MonoBehaviour
  {
    #region Properties
    public Id3LearningAction Action;
    public Dictionary<string, float> Values;
    #endregion

    #region Public Functions
    /// <summary>
    /// Function to return value from string key
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public float GetValue(string attribute)
    {
      return Values[attribute];
    }
    #endregion
  }
}