using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OnGoodVideo : MonoBehaviour
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
            AudioManager.instance.winMusic.Stop();
            AudioManager.instance.computerHum.Stop();
            SceneManager.LoadScene("FileLoaded");   

        }
    }
}
