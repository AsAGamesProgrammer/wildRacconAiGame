using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour {

    public float health = 1000;
    public float maxHealth = 1000;
    public Orientation orientation = Orientation.Top;

    bool pShieldEnabled = false;
    bool mShieldEnabled = false;

    private StartMenuController startMenuScript;

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
        startMenuScript = FindObjectOfType<StartMenuController>();

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

    //PHYSYCAL DMG
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
            Debug.Log("I AM DEAD!!!");
        }
    }

    //MAGICAL DMG
    public void TakeMDamage(float amount)
    {
        if (!mShieldEnabled)
        {
            //Reduce health
            health -= amount;

            //Display on the health bar
            healthBar.value = health;

            //Check phases
            CheckPhases();

        }

        if (health <= 0)
        {
            //************TODO***********
            //Game over
            isDead = true;

            startMenuScript.changeScreenWin();
        }

        Debug.Log("I AM DEAD!!!");
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

    //APPLY SHIELD
    public void applyShield(shieldType type, bool toApply)
    {
        switch (type)
        {
            case shieldType.Magical:
                mShieldEnabled = toApply;
                break;

            case shieldType.Physycal:
                pShieldEnabled = toApply;
                break;

            default:
                break;
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
