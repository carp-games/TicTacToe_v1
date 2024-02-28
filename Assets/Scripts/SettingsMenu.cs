using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    private bool musicPlaying = true;
    private bool sfxPlaying = true;
    public GameObject settings;
    public GameObject mainUI;


    public void MusicOff()
    {
        if (audioMixer != null && musicPlaying == true)
        {
            audioMixer.SetFloat("Music", 1f);
            musicPlaying = false;
        }
    }

    public void MusicOn()
    {
        if(audioMixer != null && musicPlaying == false)
        {
            audioMixer.SetFloat("Music", 1f);
            musicPlaying = true;
        }
    }

    public void SfxOff()
    {
        if (audioMixer != null && sfxPlaying == true)
        {
            audioMixer.SetFloat("SFX", 1f);
            sfxPlaying = false;
        }
    }

    public void SfxOn()
    {
        if (audioMixer != null && sfxPlaying == false)
        {
            audioMixer.SetFloat("SFX", 1f);
            sfxPlaying = true;
        }
    }

    public void BackToMain()
    {
        mainUI.SetActive(true);
        settings.SetActive(false);
        
    }
}
