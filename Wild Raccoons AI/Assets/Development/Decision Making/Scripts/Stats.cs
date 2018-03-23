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
    public Image fillColour;
    public int phase = 0;
    int firstPhaseHealth = 600;
    int secondPhaseHealth = 200;

    //Death
    public bool isDead = false;

    //START
    private void Start()
    {
        fillColour.color = Color.green;
    }

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
            //Reduce health
            health -= amount;

            //Display on the health bar
            healthBar.value = health;

            //Check phases
            CheckPhases();

        }

        if (health <=0)
        {
            //************TODO***********
            //Game over
            isDead = true;
        }
    }

    //PHASES
    public void CheckPhases()
    {
        //Phases
        if (health < firstPhaseHealth)
        {
            phase = 1;
            fillColour.color = Color.yellow;
        }

        if (health < secondPhaseHealth)
        {
            phase = 2;
            fillColour.color = Color.red;
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
