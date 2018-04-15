using System.Collections.Generic;
using Assets.Development.Learning.Scripts;
using UnityEngine;

public class ID3Decision : MonoBehaviour {

  public Dictionary<float, List<ID3Learning>> SplitByAttribute( ID3Learning[] dataExamples, string learningAttribute)
  {
    var sets = new Dictionary<float, List<ID3Learning>>();
    foreach (ID3Learning e in dataExamples)
    {
      float key = e.GetValue(learningAttribute);
      if (!sets.ContainsKey(key))
        sets.Add(key, new List<ID3Learning>());
      sets[key].Add(e);
    }
    return sets;
  }

  public float GetEntropy(ID3Learning[] dataExamples)
  {
    if (dataExamples.Length == 0) return 0f;
    int numExamples = dataExamples.Length;
    var tallies = new Dictionary<Id3LearningAction, int>();
    foreach (ID3Learning e in dataExamples)
    {
      if (!tallies.ContainsKey(e.Action))
        tallies.Add(e.Action, 0);
      tallies[e.Action]++;
    }
    int keysCount = tallies.Keys.Count;
    if (keysCount == 0) return 0f;
    float entropy = 0f;
    foreach (int tally in tallies.Values)
    {
      var proportion = tally / (float)numExamples;
      entropy -= proportion * Mathf.Log(proportion, 2);
    }
    return entropy;
  }

  public float GetEntropy( Dictionary<float, List<ID3Learning>> sets, int numberOfExamples)
  {
    float entropy = 0f;
    foreach (List<ID3Learning> s in sets.Values)
    {
      var proportion = s.Count / (float)numberOfExamples;
      entropy -= proportion * GetEntropy(s.ToArray());
    }
    return entropy;
  }


  public void MakeTree( ID3Learning[] dataExamples, List<string> learningAttribute, DecisionNodeClass node)
  {
    float entropy = GetEntropy(dataExamples);
    if (entropy <= 0)
      return;
    int numberOfExamples = dataExamples.Length;
    float gain = 0f;
    string splitAttribute = "";
    var bestSets = new Dictionary<float, List<ID3Learning>>();
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
    foreach (List<ID3Learning> set in bestSets.Values)
    {
      float val = set[0].GetValue(splitAttribute);
      DecisionNodeClass child = new DecisionNodeClass();
      node.Children.Add(val, child);
      MakeTree(set.ToArray(), newAttributes, child);
    }
  }
}
