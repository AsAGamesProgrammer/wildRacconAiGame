using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public GameObject UI_Canvas;
    public GameObject defaultScreen;
    public GameObject creditsScreen;
    public GameObject deathScreen;
    public GameObject winScreen;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(UI_Canvas);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        UI_Canvas.SetActive(false);
        cleanAll();

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
        UI_Canvas.SetActive(true);
        cleanAll();
        deathScreen.SetActive(true);
    }

    public void changeScreenWin()
    {
        UI_Canvas.SetActive(true);
        cleanAll();
        winScreen.SetActive(true);
    }
}
