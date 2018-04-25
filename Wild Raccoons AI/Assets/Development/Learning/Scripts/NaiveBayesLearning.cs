using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class NaiveBayesLearning : MonoBehaviour {
  #region Properties
  public List<InformationModel> Data = new List<InformationModel>();
  Classifier data;
  #endregion

  #region Public Functions
  /// <summary>
  /// Function to help with learning data
  /// </summary>
  public void BlockLearn()
  {
    data = new Classifier();
    data.Teach(Data);
  }

  /// <summary>
  /// Function to get all probabilities from a list of strings 
  /// </summary>
  /// <param name="test"></param>
  /// <returns></returns>
  public IDictionary<string, double> Classify(List<string> test)
  {
    IDictionary<string, double> dict = data.Classify(test);
    return dict;
  }
  #endregion

  public class InformationModel
  {
    /// <summary>
    /// Object category
    /// </summary>
    public string Lable { get; set; }

    /// <summary>
    /// List of object features 
    /// </summary>
    public List<string> Features { get; set; }
  }

  public class Classifier
  {
    #region Properties
    private readonly Dictionary<string, List<string>> _allCategories;
    private List<InformationModel> _rawDataToTrain;
    #endregion

    #region Constructor
    public Classifier()
    {
      _allCategories = new Dictionary<string, List<string>>();
      _rawDataToTrain = new List<InformationModel>();
    }
    #endregion

    #region Public Functions

    /// <summary>
    /// Function to find the probability that a combination will match an attribute
    /// </summary>
    /// <param name="objectFeatures"></param>
    /// <returns></returns>
    public Dictionary<string, double> Classify(List<string> objectFeatures)
    {
      if (objectFeatures == null)
        throw new ArgumentNullException();

      if (objectFeatures.Count <= 0)
        throw new ArgumentException("Classified object does not contain any features.");

      if (_allCategories.Count <= 0)
        throw new ArgumentException("Classifier has not been trained. First use Teach method.");

      return _allCategories.ToDictionary(item => item.Key, item => CalculateProbability(item.Key, objectFeatures));
    }

    /// <summary>
    /// Function to get the data to be used by the classify function
    /// </summary>
    /// <param name="trainingDataSet"></param>
    public void Teach(List<InformationModel> trainingDataSet)
    {
      if (trainingDataSet == null)
        throw new ArgumentNullException();

      _rawDataToTrain = trainingDataSet;

      foreach (var model in trainingDataSet)
      {
        if (!_allCategories.ContainsKey(model.Lable))
        {
          _allCategories.Add(model.Lable, model.Features);
        }
        else
        {
          _allCategories[model.Lable].AddRange(model.Features);
        }
      }

    }

    #endregion

    #region Private Functions
    /// <summary>
    /// Calculate probability for current label.
    /// </summary>
    /// <param name="label">Label</param>
    /// <param name="features">Features of object</param>
    /// <returns></returns>
    private double CalculateProbability(string label, List<string> features)
    {
      if (string.IsNullOrEmpty(label))
        throw new ArgumentException("Empty label");

      if (features == null)
        throw new ArgumentNullException();

      var labelSetCount = _rawDataToTrain.Count(x => x.Lable == label);
      double probabilityOfLabel = labelSetCount / Convert.ToDouble(_rawDataToTrain.Count);

      var objectProbabilityOfFeature = new List<double>();

      foreach (var feature in features)
      {
        //takes all features occurrence from Dictionary which contain feature from training data.
        var occurency = _allCategories[label].FindAll(p => p.Equals(feature)).Count;
        //calculate a posteriori probability and add it to collection
        var featurePosterioriProb = occurency / Convert.ToDouble(labelSetCount);
        //objFeaturesProp.Add(!featurePosterioriProb.Equals(0) ? featurePosterioriProb : 1);
        if (!featurePosterioriProb.Equals(0)) objectProbabilityOfFeature.Add(featurePosterioriProb);
      }

      double result = objectProbabilityOfFeature.Aggregate(1.0, (current, item) => current * item) * probabilityOfLabel;

      return result;
    }
    #endregion
  }
}
