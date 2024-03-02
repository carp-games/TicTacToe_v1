using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public AudioSource GifAudio;

    public GameObject Music;
    public GameObject SFX;

    private bool musicOn = true;
    private bool sfxOn = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }



    public void StartOpeningSceneMusic()
    {
        mainMenuMusic.Stop();
        glitch.Play();
        openingTrack.Play();
    }

    public void StartMainGameAudio()
    {
        openingTrack.Stop();
        glitch.Stop();
        mainMusic.Play();
    }

    public void PlayMainMenuMusic()
    {
        if (mainMenuMusic != null)
        {
            mainMenuMusic.Play();
            openingTrack.Stop();
        }
        if (glitch != null)
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
        if (computerHum != null)
        {
            computerHum.Stop();
        }
    }

    public void TryAgainAudio()
    {
        loseMusic.Stop();
        glitch.Stop();
        computerHum.Play();
        mainMusic.Play();
    }

    public void ExitToMenu()
    {
        mainMusic.Stop();
        loseMusic.Stop();
        glitch.Stop();
        computerHum.Play();
        mainMenuMusic.Play();
    }

    public void GoodEndingAudio()
    {
        winMusic.Stop();
        virusLostSFX.Stop();
        GifAudio.Play();
    }
    public void GoodEndingAudioStop()
    {
        GifAudio.Stop();
        mainMenuMusic.Play();
        computerHum.Play();
    }


    public void ButtonSFX()
    {
        buttonSFX.Play();
    }

    public void TurnMusicOn()
    {
        if(musicOn == false)
        {
            Music.SetActive(true);
            musicOn = true;
        }
    }

    public void TurnMusicOff()
    {
        if (musicOn == true)
        {
            Music.SetActive(false);
            musicOn = false;
        }
    }

    public void TurnSFXOn()
    {
        if (sfxOn == false)
        {
            SFX.SetActive(true);
            sfxOn = true;
        }
    }

    public void TurnSFXOff()
    {
        if (sfxOn == true)
        {
            SFX.SetActive(false);
            sfxOn = false;
        }
    }

}
