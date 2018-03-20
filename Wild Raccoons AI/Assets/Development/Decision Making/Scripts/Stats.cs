using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    float health = 100;
    public Orientation orientation = Orientation.Top;
    public bool pShieldEnabled = false;
    
    //ORIENTATION
    public Orientation getOrientation()
    {
        return orientation;
    }

    public void setOrientation(Orientation newOrientation)
    {
        orientation = newOrientation;
    }

}

//Enum for orientations
public enum Orientation
{
    Top,
    Left,
    Right
}
