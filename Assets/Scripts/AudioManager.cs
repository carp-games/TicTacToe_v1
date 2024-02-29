using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource openingTrack;
    public AudioSource mainMenuMusic;
    public AudioSource winMusic;
    public AudioSource loseMusic;
    public AudioSource mainMusic;

    public AudioSource buttonSFX;
    public AudioSource virusLostSFX;
    public AudioSource computerHum;
    public AudioSource virusLaugh;
    public AudioSource glitch;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    void Start()
    {
        if (openingTrack != null)
        {
            openingTrack.Play();
        }

    }

    public void PlayMainMenuMusic()
    {
        if (mainMenuMusic != null)
        {
            mainMenuMusic.Play();
            openingTrack.Stop();
        }
        if(glitch != null)
        {
            glitch.Stop();
        }

    }

    public void PlayWinMusic()
    {
        if (mainMusic != null)
        {
            mainMusic.Stop();
        }

        if (winMusic != null)
        {
            winMusic.Play();
        }
         
        if (virusLostSFX != null)
        {
            virusLostSFX.Play();
        }
        if (computerHum != null)
        {
            computerHum.Stop();
        }
    }

    public void PlayLoseMusic()
    {
        if (mainMusic != null)
        {
            mainMusic.Stop();
        }

        if (loseMusic != null)
        {
            loseMusic.Play();
        }

        if (virusLaugh != null)
        {
            virusLaugh.Play();
        }
        if(computerHum != null)
        {
            computerHum.Stop();
        }
    }

    public void ButtonSFX()
    {
        buttonSFX.Play();
    }

}
