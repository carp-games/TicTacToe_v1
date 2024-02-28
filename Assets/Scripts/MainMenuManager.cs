using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public GameObject settings;
    public GameObject mainMenu;

    public void StartGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void SettingsMenuOn()
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
