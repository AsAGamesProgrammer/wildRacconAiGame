using System.Collections.Generic;
using UnityEngine;

namespace Assets.Development.Learning.Scripts
{
  public enum Id3LearningAction
  {
    Stop, Walk, Run
  }

  public class ID3Learning : MonoBehaviour {
    public Id3LearningAction Action;
    public Dictionary<string, float> Values;

    public float GetValue(string attribute)
    {
      return Values[attribute];
    }
  }
}