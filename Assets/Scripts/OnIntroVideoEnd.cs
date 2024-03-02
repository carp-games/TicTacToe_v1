using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class OnIntroVideoEnd : MonoBehaviour
{
    public VideoPlayer videoPlayer;    

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (vp == videoPlayer)
        {
            AudioManager.instance.StartMainGameAudio();
            SceneManager.LoadScene("MainGameScene");
        }

        
    }
}
