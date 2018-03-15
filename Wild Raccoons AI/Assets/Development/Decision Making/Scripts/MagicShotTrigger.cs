using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShotTrigger : MonoBehaviour {

  [HideInInspector] public int spellDamage = 1;                         // Set the number of hitpoints that this gun will take away from shot objects with a health script.
  [HideInInspector] public float spellRange = 50f;                   // Distance in unity units over which the player can fire.
  [HideInInspector] public float spellForce = 100f;                     // Amount of force which will be added to objects with a rigidbody shot by the player.
  public Transform characterHitbox;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun.
  [HideInInspector] public LineRenderer castingLine;                    // Reference to the LineRenderer component which will display our castingLine.

  private Camera fpsCam;                                              // Holds a reference to the first person camera.
  private readonly WaitForSeconds spellDuration = new WaitForSeconds(.07f);     // WaitForSeconds object used by our SpellEffect coroutine, determines time laser line will remain visible.


  public void Initialize()
  {
    //Get and store a reference to our LineRenderer component
    castingLine = GetComponent<LineRenderer>();

    //Get and store a reference to our Camera
    fpsCam = GetComponentInParent<Camera>();
  }

  public void Fire()
  {

    //Create a vector at the center of our camera's near clip plane.
    Vector3 startOfRay = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));

    //Draw a debug line which will show where our ray will eventually be
    Debug.DrawRay(startOfRay, fpsCam.transform.forward * spellRange, Color.green);

    //Declare a raycast hit to store information about what our raycast has hit.
    RaycastHit hit;

    //Start our SpellEffect coroutine to turn our laser line on and off
    StartCoroutine(SpellEffect());

    //Set the start position for our visual effect for our laser to the position of gunEnd
    castingLine.SetPosition(0, characterHitbox.position);

    //Check if our raycast has hit anything
    if (Physics.Raycast(startOfRay, fpsCam.transform.forward, out hit, spellRange))
    {
      //Set the end position for our laser line 
      castingLine.SetPosition(1, hit.point);

      //Get a reference to a health script attached to the collider we hit
      EnemyBox health = hit.collider.GetComponent<EnemyBox>();

      //If there was a health script attached
      if (health != null)
      {
        //Call the damage function of that script, passing in our gunDamage variable
        health.Damage(spellDamage);
      }

      //Check if the object we hit has a rigidbody attached
      if (hit.rigidbody != null)
      {
        //Add force to the rigidbody we hit, in the direction it was hit from
        hit.rigidbody.AddForce(-hit.normal * spellForce);
      }
    }
    else
    {
      //if we did not hit anything, set the end of the line to a position directly away from
      castingLine.SetPosition(1, fpsCam.transform.forward * spellRange);
    }
  }

  private IEnumerator SpellEffect()
  {

    //Turn on our line renderer
    castingLine.enabled = true;
    //Wait for .07 seconds
    yield return spellDuration;

    //Deactivate our line renderer after waiting
    castingLine.enabled = false;
  }
}
