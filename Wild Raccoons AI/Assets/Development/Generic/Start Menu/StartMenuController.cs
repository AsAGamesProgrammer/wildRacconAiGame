using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject defaultScreen;
    public GameObject creditsScreen;
    public GameObject deathScreen;
    public GameObject winScreen;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void cleanAll()
    {
        defaultScreen.SetActive(false);
        creditsScreen.SetActive(false);
        deathScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    public void changeScreenDefault()
    {
        cleanAll();
        defaultScreen.SetActive(true);
    }

    public void changeScreenCredits()
    {
        cleanAll();
        creditsScreen.SetActive(true);
    }

    public void changeScreenDeath()
    {
        cleanAll();
        deathScreen.SetActive(true);
    }

    public void changeScreenWin()
    {
        cleanAll();
        winScreen.SetActive(true);
    }
}
