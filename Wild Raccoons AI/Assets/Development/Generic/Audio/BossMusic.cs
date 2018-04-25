using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusic : MonoBehaviour
{
  #region Properties
  public AudioSource Player;
  public AudioClip[] BossTheme;
  #endregion

  #region Private Functions
  /// <summary>
  /// On Start play a random piece of music
  /// </summary>
  private void Start () {
    int randomIndex = Random.Range(0, BossTheme.Length);
    Player.clip = BossTheme[randomIndex];
    Player.Play();
    Player.loop = true;
  }
  #endregion
}
