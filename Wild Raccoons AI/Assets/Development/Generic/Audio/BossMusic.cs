using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : MonoBehaviour
{
  public AudioSource Player;

  public AudioClip[] BossTheme;
  // Use this for initialization
  private void Start () {
    int randomIndex = Random.Range(0, BossTheme.Length);
    Player.clip = BossTheme[randomIndex];
    Player.Play();
    Player.loop = true;
  }
	
	// Update is called once per frame
  private void Update () {
		
	}
}
