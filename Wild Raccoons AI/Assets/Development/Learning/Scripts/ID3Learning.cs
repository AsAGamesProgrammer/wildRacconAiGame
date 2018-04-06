using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ID3LearningAction
{
  STOP, WALK, RUN
}

public class ID3Learning : MonoBehaviour {
  public ID3LearningAction action;
  public Dictionary<string, float> values;

  public float GetValue(string attribute)
  {
    return values[attribute];
  }
}
