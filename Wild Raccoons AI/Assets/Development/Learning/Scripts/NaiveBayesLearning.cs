using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class NaiveBayesLearning : MonoBehaviour {
  public List<InformationModel> Data = new List<InformationModel>();
  Classifier data;
  public void BlockLearn()
  {
    data = new Classifier();
    data.Teach(Data);
  }

  public IDictionary<string, double> Classify(List<string> test)
  {
    IDictionary<string, double> dict = data.Classify(test);
    return dict;
  }

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
    private readonly Dictionary<string, List<string>> _allFeaturesOfCategory;
    private List<InformationModel> _rawTrainingData;


    public Classifier()
    {
      _allFeaturesOfCategory = new Dictionary<string, List<string>>();
      _rawTrainingData = new List<InformationModel>();
    }

    public Dictionary<string, double> Classify(List<string> objectFeatures)
    {
      if (objectFeatures == null)
        throw new ArgumentNullException();

      if (objectFeatures.Count <= 0)
        throw new ArgumentException("Classified object does not contain any features.");

      if (_allFeaturesOfCategory.Count <= 0)
        throw new ArgumentException("Classifier has not been trained. First use Teach method.");

      return _allFeaturesOfCategory.ToDictionary(item => item.Key, item => CalculateProbability(item.Key, objectFeatures));
    }

    public void Teach(List<InformationModel> trainingDataSet)
    {
      if (trainingDataSet == null)
        throw new ArgumentNullException();

      _rawTrainingData = trainingDataSet;

      foreach (var model in trainingDataSet)
      {
        if (!_allFeaturesOfCategory.ContainsKey(model.Lable))
        {
          _allFeaturesOfCategory.Add(model.Lable, model.Features);
        }
        else
        {
          _allFeaturesOfCategory[model.Lable].AddRange(model.Features);
        }
      }

    }

    /// <summary>
    /// Calculate probability for current lable.
    /// </summary>
    /// <param name="label">Label</param>
    /// <param name="features">Features of object</param>
    /// <returns></returns>
    private double CalculateProbability(string label, List<string> features)
    {
      if (string.IsNullOrEmpty(label))
        throw new ArgumentException("Empty lable");

      if (features == null)
        throw new ArgumentNullException();


      //P(d) = ilosc_wystapien_danej_kategorii/ilosc_wszystkich_pozycji_na_liscie_treningowej
      //P(v1|d) = ilosc_wystepowania_cechy_v1/ilosc_wystepowania_danej_kategorii_w_danych_treningowych

      var currentLableSetCount = _rawTrainingData.Count(x => x.Lable == label);
      double labelProbability = currentLableSetCount / Convert.ToDouble(_rawTrainingData.Count);

      var objFeaturesProb = new List<double>();

      foreach (var feature in features)
      {
        //takes all features occurency from Dictionary which contain feature from training data.
        var featureOccurency = _allFeaturesOfCategory[label].FindAll(p => p.Equals(feature)).Count;
        //calculate a posteriori probability and add it to collection
        var featurePosterioriProb = featureOccurency / Convert.ToDouble(currentLableSetCount);
        //objFeaturesProp.Add(!featurePosterioriProb.Equals(0) ? featurePosterioriProb : 1);
        if (!featurePosterioriProb.Equals(0)) objFeaturesProb.Add(featurePosterioriProb);
      }

      double result = objFeaturesProb.Aggregate(1.0, (current, item) => current * item) * labelProbability;

      return result;
    }

    
    public void SaveTrainingData(object modelInfos)
    {
      if (modelInfos == null)
        throw new ArgumentNullException();

    }
  }
}
