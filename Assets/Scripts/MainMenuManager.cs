using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public GameObject settings;
    public GameObject mainMenu;
    public GameObject creditsMenu;

    public void StartGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void SettingsMenuOn()
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CreditsMenu()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void BackToMain()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
