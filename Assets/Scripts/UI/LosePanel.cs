using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    public GameObject btnRestart;

    public Text txtReward;
    public int reward;

    private void OnEnable()
    {
        InitializePanel();
    }

    void InitializePanel()
    {
        txtReward.text = string.Format("You earned {0} gems", reward);
        if (GameSceneController.Instance.gameMode == GameMode.Tournament)
        {
            btnRestart.SetActive(false);
        }
        else
        {
            btnRestart.SetActive(true);
        }
    }

    public void OnRestartButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        int currentOpponent = GameSceneController.Instance.currentOpponentIndex;
        if (GameSceneController.Instance.gameMode == GameMode.Match)
        {
            PlayerPrefs.SetInt("CurrentOpponentIndex", 0);
        }
        else
        {
            if (currentOpponent < GameSceneController.Instance.allBlades.Count - 1)
            {
                PlayerPrefs.SetInt("CurrentOpponentIndex", currentOpponent + 1);
            }
            else
            {
                PlayerPrefs.SetInt("CurrentOpponentIndex", 0);
            }
        }
        GameSceneController.Instance.LoadBattleScene();
    }

    public void OnMainMenuButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        PlayerPrefs.SetInt("CurrentOpponentIndex", 0);
        if (GameSceneController.Instance.gameMode == GameMode.Tournament)
        {
            GameController.Instance.CompleteTournament();
        }
        GameSceneController.Instance.LoadMenuScene();
    }
}
