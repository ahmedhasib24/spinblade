using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public GameObject btnMainMenu;
    public GameObject btnNextFight;
    public Text txtReward;

    public int reward = 0;
    private void OnEnable()
    {
        txtReward.text = string.Format("You earned {0} gems", reward);
        if (GameSceneController.Instance.gameMode == GameMode.Arcade)
        {
            btnMainMenu.SetActive(true);
            btnNextFight.SetActive(false);
        }
        else if(GameSceneController.Instance.gameMode == GameMode.Match)
        {
            btnNextFight.SetActive(true);
            btnMainMenu.SetActive(true);
        }
        else
        {
            btnMainMenu.SetActive(false);
            if(GameController.Instance.tournamentRound == 2)
            {
                btnNextFight.SetActive(false);
                btnMainMenu.SetActive(true);
            }
            else
            {
                btnNextFight.SetActive(true);
                btnMainMenu.SetActive(false);
            }
        }
    }

    public void OnNextButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        
        int currentOpponent = GameSceneController.Instance.currentOpponentIndex;
        if(GameSceneController.Instance.gameMode == GameMode.Match)
        {
            if (currentOpponent < GameController.Instance.basicBlades.Count - 1)
            {
                PlayerPrefs.SetInt("CurrentOpponentIndex", currentOpponent + 1);
            }
            else
            {
                PlayerPrefs.SetInt("CurrentOpponentIndex", 0);
            }
            GameSceneController.Instance.LoadBattleScene();
        }
        else if(GameSceneController.Instance.gameMode == GameMode.Tournament)
        {
            GameController.Instance.CompleteTournamentRound();
            GameSceneController.Instance.LoadMenuScene();
        }
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

    public void OnFBShareButtonClick()
    {
    }

    public void OnTweeterShareButtonClick()
    {
    }
}
