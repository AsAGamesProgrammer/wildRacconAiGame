using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {
  public float secondsInFullDay = 120f;
  [Range(0, 1)]
  public float currentTimeOfDay = 0;
  [HideInInspector]
  public float timeMultiplier = 1f;

  float sunInitialIntensity;

  void Start()
  {
    sunInitialIntensity = GetComponent<Light>().intensity;
  }

  void Update()
  {
    UpdateSun();

    currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

    if (currentTimeOfDay >= 1)
    {
      currentTimeOfDay = 0;
    }
  }

  void UpdateSun()
  {
    transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);

    float intensityMultiplier = 1;
    if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)
    {
      intensityMultiplier = 0;
    }
    else if (currentTimeOfDay <= 0.25f)
    {
      intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
    }
    else if (currentTimeOfDay >= 0.73f)
    {
      intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
    }

    GetComponent<Light>().intensity = sunInitialIntensity * intensityMultiplier;
  }

  void OnGUI()
  {
    float currentHour = 24 * currentTimeOfDay;
    float currentHourFix = (int)(24 * currentTimeOfDay);
    float currentMinute = (int)(60 * (currentHour - Mathf.Floor(currentHour)));
    Rect rect = new Rect(10, 10, 120, 20);
    GUI.Label(rect, text: "time: " + currentHourFix + ":" + currentMinute);
    rect = new Rect(120, 10, 200, 10);
  }
}
