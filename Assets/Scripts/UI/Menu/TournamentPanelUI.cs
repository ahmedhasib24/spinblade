using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TournamentPanelUI : MonoBehaviour
{
    public MenuUIController menuUIController;
    public int panelIndex = 8;

    public List<Text> playerNameTexts;
    public GameObject btnBack;

    private void OnEnable()
    {
        InitializePanel();
    }

    private void InitializePanel()
    {
        //Initialize tournament
        InitializeTournament();
        if (GameController.Instance.tournamentRound == 0)
        {
            btnBack.SetActive(true);
        }
        else
        {
            btnBack.SetActive(false);
        }
        for (int i = 0; i < GameController.Instance.tournamentBlades.Count; i++)
        {
            if(i == 0)
            {
                playerNameTexts[i].color = Color.green;
            }
            else if(GameController.Instance.eliminatedIds.Contains(i))
            {
                playerNameTexts[i].color = Color.red;
            }
            playerNameTexts[i].text = GameController.Instance.tournamentBlades[i].name;
        }
    }

    private void InitializeTournament()
    {
        
        GameController.Instance.InitializeTournament();
    }

    public void OnBackButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        menuUIController.lastOpenedPanelIndex = 3;
        menuUIController.OpenPanel(6);
    }

    public void OnProceedButtonClick()
    {
        SetupCurrentMatchData();
        SceneManager.LoadScene("Battle");
    }

    void SetupCurrentMatchData()
    {
        GameController.Instance.SetupTournamentRound();
    }
}
