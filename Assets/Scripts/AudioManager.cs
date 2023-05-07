using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }


    public AudioClip background;
    public AudioClip button;

    private AudioSource audioSource;

    private bool isSoundOn = false;
    private bool isMusicOn = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
            audioSource = GetComponent<AudioSource>();
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                isSoundOn = true;
            }
            else
            {
                isSoundOn = false;
            }
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                isMusicOn = true;
            }
            else
            {
                isMusicOn = false;
            }
            PlayBackgroundClip();
        }
    }

    public void SoundSettings(int index)
    {
        switch (index)
        {
            case 0:
                PlayerPrefs.SetInt("Sound", 0);
                isSoundOn = true;
                break;
            case 1:
                PlayerPrefs.SetInt("Sound", 1);
                isSoundOn = false;
                break;
        }
        //Debug.Log("id: " + index + ", " + isSoundOn);
        //PlayButtonClip();
    }

    public void MusicSettings(int index)
    {
        switch (index)
        {
            case 0:
                PlayerPrefs.SetInt("Music", 0);
                isMusicOn = true;
                if(audioSource.isPlaying == false)
                {
                    audioSource.Play();
                }
                break;
            case 1:
                PlayerPrefs.SetInt("Music", 1);
                isMusicOn = false;
                audioSource.Stop();
                break;
        }
    }

    public void PlayButtonClip()
    {
        if (isSoundOn)
        {
            audioSource.PlayOneShot(button);
        }
    }

    public void PlayBackgroundClip()
    {
        if (isMusicOn)
        {
            audioSource.Play();
        }
    }
}
