using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    //public ToggleGroup toggle;
    public Toggle soundOnToggle;
    public Toggle soundOffToggle;
    public Toggle musicOnToggle;
    public Toggle musicOffToggle;
    public Toggle fightModeEasyToggle;
    public Toggle fightModeMediumToggle;
    public Toggle fightModeHardToggle;

    private void OnEnable()
    {
        SetupSoundToggles();
        SetupMusicToggles();
        SetupFightModeToggles();
    }

    private void SetupSoundToggles()
    {
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            soundOnToggle.isOn = true;
            soundOffToggle.isOn = false;
        }
        else
        {
            soundOnToggle.isOn = false;
            soundOffToggle.isOn = true;
        }
    }

    private void SetupMusicToggles()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            musicOnToggle.isOn = true;
            musicOffToggle.isOn = false;
        }
        else
        {
            musicOnToggle.isOn = false;
            musicOffToggle.isOn = true;
        }
    }

    private void SetupFightModeToggles()
    {
        int value = PlayerPrefs.GetInt("FightMode");
        switch (value)
        {
            case 0:
                fightModeEasyToggle.isOn = true;
                fightModeMediumToggle.isOn = false;
                fightModeHardToggle.isOn = false;
                break;
            case 1:
                fightModeEasyToggle.isOn = false;
                fightModeMediumToggle.isOn = true;
                fightModeHardToggle.isOn = false;
                break;
            case 2:
                fightModeEasyToggle.isOn = false;
                fightModeMediumToggle.isOn = false;
                fightModeHardToggle.isOn = true;
                break;
        }
    }

    public void OnSoundToogle(int index)
    {
        //toggle.ActiveToggles()[]
        AudioManager.Instance.SoundSettings(index);
        AudioManager.Instance.PlayButtonClip();
    }

    public void OnMusicToogle(int index)
    {
        AudioManager.Instance.MusicSettings(index);
        AudioManager.Instance.PlayButtonClip();
    }

    public void OnFightModeChanged(int index)
    {
        AudioManager.Instance.PlayButtonClip();
        PlayerPrefs.SetInt("FightMode", index);
    }
}
