using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUIController : MonoBehaviour
{
    public VSPanel vsPanel;
    public GamePanelUI gamePanel;
    public WinPanel winPanel;
    public LosePanel losePanel;
    public PausePanelUI pausePanel;
    public Button btnSkill;
    public Button btnAttack;
    public Button btnDefense;

    public List<Sprite> bladeIconImages;

   

    public void ShowVSPanel()
    {
        vsPanel.InitializePanel();
        vsPanel.gameObject.SetActive(true);
    }

    public void CloseVSPanel()
    {
        vsPanel.gameObject.SetActive(false);
    }

    public void ShowGamePanel()
    {
        gamePanel.InitializePanel();
        gamePanel.gameObject.SetActive(true);
    }

    public void CloseGamePanel()
    {
        gamePanel.gameObject.SetActive(false);
    }

    public void ShowGameOverPanel(bool win, int reward)
    {
        if(win == true)
        {
            winPanel.reward = reward;
            winPanel.gameObject.SetActive(true);
        }
        else
        {
            losePanel.reward = reward;
            losePanel.gameObject.SetActive(true);
        }
    }

    public void CloseGameOverPanel()
    {
        winPanel.gameObject.SetActive(false);
    }

    public void OpenPausePanel()
    {
        pausePanel.gameObject.SetActive(true);
    }

    public void ClosePausePanel()
    {
        pausePanel.gameObject.SetActive(false);
    }

    public void UpdateEnergyBars(SpinController player, List<SpinController> opponents)
    {
        gamePanel.UpdateEnergySliders(player, opponents);
    }

    public void UpdatePlayerHealthBar(float playerHealth)
    {
        gamePanel.UpdatePlayerHealthSlider(playerHealth);
    }

    public void UpdateOpponentHealthBar(int id, float opponentHealth)
    {
        gamePanel.UpdateOpponentHealthSlider(id, opponentHealth);
    }

    public void UpdateSkillButton(bool isInteractable)
    {
        btnSkill.interactable = isInteractable;
    }

    public void UpdateAttackButton(bool isInteractable)
    {
        btnAttack.interactable = isInteractable;
    }

    public void UpdateDefenseButton(bool isInteractable)
    {
        btnDefense.interactable = isInteractable;
    }
}
