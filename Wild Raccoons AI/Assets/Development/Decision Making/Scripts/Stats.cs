using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour {

    float health = 1000;
    public Orientation orientation = Orientation.Top;
    public bool pShieldEnabled = false;

    //Phases
    public Slider healthBar;
    public int phase = 0;
    int firstPhaseHealth = 600;
    int secondPhaseHealth = 200;
    
    //ORIENTATION
    public Orientation getOrientation()
    {
        return orientation;
    }

    public void setOrientation(Orientation newOrientation)
    {
        orientation = newOrientation;
    }

    //HEALTH
    public void TakePDamage (float amount)
    {
        if (!pShieldEnabled)
        {
            health -= amount;
            healthBar.value = health;

            //Phases
            if (health < firstPhaseHealth)
                phase = 1;

            if (health < secondPhaseHealth)
                phase = 2;

        }

        if (health <=0)
        {
            //************TODO***********
            //Game over
        }


    }

}

//Enum for orientations
public enum Orientation
{
    Top,
    Left,
    Right
}
