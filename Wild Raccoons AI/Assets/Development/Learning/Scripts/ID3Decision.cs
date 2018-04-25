using System.Collections.Generic;
using Assets.Development.Learning.Scripts;
using UnityEngine;

public class ID3Decision : MonoBehaviour {
  #region Private Functions
  /// <summary>
  /// Function to split the learning attributes and return 
  /// </summary>
  /// <param name="dataExamples"></param>
  /// <param name="learningAttribute"></param>
  /// <returns></returns>
  private Dictionary<float, List<ID3Learning>> SplitByAttribute( ID3Learning[] dataExamples, string learningAttribute)
  {
    var allAttributes = new Dictionary<float, List<ID3Learning>>();
    foreach (ID3Learning e in dataExamples)
    {
      float key = e.GetValue(learningAttribute);
      if (!allAttributes.ContainsKey(key))
        allAttributes.Add(key, new List<ID3Learning>());
      allAttributes[key].Add(e);
    }
    return allAttributes;
  }

  /// <summary>
  /// Function to get the entropy of the data examples
  /// </summary>
  /// <param name="dataExamples"></param>
  /// <returns></returns>
  private float GetEntropy(ID3Learning[] dataExamples)
  {
    if (dataExamples.Length == 0) return 0f;
    int numExamples = dataExamples.Length;
    var dataTallies = new Dictionary<Id3LearningAction, int>();
    //get tallies
    foreach (ID3Learning e in dataExamples)
    {
      if (!dataTallies.ContainsKey(e.Action))
        dataTallies.Add(e.Action, 0);
      dataTallies[e.Action]++;
    }
    int countOfKeys = dataTallies.Keys.Count;
    if (countOfKeys == 0) return 0f;
    float entropy = 0f;
    //calculate entropy of tallies
    foreach (int tally in dataTallies.Values)
    {
      var proportion = tally / (float)numExamples;
      entropy -= proportion * Mathf.Log(proportion, 2);
    }
    return entropy;
  }

  /// <summary>
  /// Function to calculate entropy
  /// </summary>
  /// <param name="sets"></param>
  /// <param name="numberOfExamples"></param>
  /// <returns></returns>
  private float GetEntropy( Dictionary<float, List<ID3Learning>> sets, int numberOfExamples)
  {
    float entropy = 0f;
    foreach (List<ID3Learning> s in sets.Values)
    {
      var proportion = s.Count / (float)numberOfExamples;
      entropy -= proportion * GetEntropy(s.ToArray());
    }
    return entropy;
  }
  #endregion

  #region Public Functions
  /// <summary>
  /// Function to create the decision tree
  /// </summary>
  /// <param name="dataExamples"></param>
  /// <param name="learningAttribute"></param>
  /// <param name="node"></param>
  public void MakeTree( ID3Learning[] dataExamples, List<string> learningAttribute, DecisionNodeClass node)
  {
    float entropy = GetEntropy(dataExamples);
    if (entropy <= 0)
      return;
    int numberOfExamples = dataExamples.Length;
    float gain = 0f;
    string splitAttribute = "";
    var bestSets = new Dictionary<float, List<ID3Learning>>();
    //create sets
    foreach (string a in learningAttribute)
    {
      var sets = SplitByAttribute(dataExamples, a);
      var overallEntropy = GetEntropy(sets, numberOfExamples);
      var infoGain = entropy - overallEntropy;
      if (infoGain > gain)
      {
        gain = infoGain;
        splitAttribute = a;
        bestSets = sets;
      }
    }
    node.Value = splitAttribute;
    List<string> newAttributes = new List<string>(learningAttribute);
    newAttributes.Remove(splitAttribute);
    //create tree
    foreach (List<ID3Learning> set in bestSets.Values)
    {
      float val = set[0].GetValue(splitAttribute);
      DecisionNodeClass child = new DecisionNodeClass();
      node.Children.Add(val, child);
      MakeTree(set.ToArray(), newAttributes, child);
    }
  }
  #endregion
}
