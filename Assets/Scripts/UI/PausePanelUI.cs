using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanelUI : MonoBehaviour
{
    public void OnContinueButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        GameSceneController.Instance.TogglePause();
    }

    public void OnRestartButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        GameSceneController.Instance.TogglePause();
        GameSceneController.Instance.LoadBattleScene();
    }

    public void OnMainMenuButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        GameSceneController.Instance.TogglePause();
        PlayerPrefs.SetInt("CurrentOpponentIndex", 0);
        if (GameSceneController.Instance.gameMode == GameMode.Tournament)
        {
            GameController.Instance.CompleteTournament();
        }
        GameSceneController.Instance.LoadMenuScene();
    }
}
