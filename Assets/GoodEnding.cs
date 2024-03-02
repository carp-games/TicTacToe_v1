using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoodEnding : MonoBehaviour
{
    public GameObject endingVideo;
    
    void Start()
    {
        StartCoroutine(GoodEndingGif());
    }

    IEnumerator GoodEndingGif()
    {
        yield return new WaitForSeconds(2f);
        AudioManager.instance.GoodEndingAudio();
        endingVideo.SetActive(true);

        yield return new WaitForSeconds(AudioManager.instance.GifAudio.clip.length);
        AudioManager.instance.GoodEndingAudioStop();
        SceneManager.LoadScene("Menu");

    }

    public void BackToMenuButton()
    {
        AudioManager.instance.GoodEndingAudioStop();
        SceneManager.LoadScene("Menu");
    }
    
}
