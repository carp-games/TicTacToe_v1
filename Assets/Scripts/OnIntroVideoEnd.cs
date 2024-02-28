using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class OnIntroVideoEnd : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject VideoPanel;
    

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (vp == videoPlayer)
        {
            gameObject.SetActive(false);
            VideoPanel.SetActive(false);
        }
    }
}
