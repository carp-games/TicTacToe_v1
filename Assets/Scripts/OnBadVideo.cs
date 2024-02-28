using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OnBadVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject buttons;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (vp == videoPlayer)
        {
            buttons.SetActive(true);
        }
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("MainGameScene");
    }
    public void ExitGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
