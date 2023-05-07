using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanelUI : MonoBehaviour
{
    public MenuUIController menuUIController;
    public int panelIndex = 0;

    public List<GameObject> subPanels;

    public List<Image> buttonImages;
    public List<Sprite> buttonOnSprites;
    public List<Sprite> buttonOffSprites;

    //private void OnEnable()
    //{
        //OpenSubPanel(0);
    //}

    public void OpenSubPanel(int index)
    {
        for (int i = 0; i < subPanels.Count; i++)
        {
            if (i == index)
            {
                subPanels[i].SetActive(true);
                buttonImages[i].sprite = buttonOnSprites[i];
            }
            else
            {
                subPanels[i].SetActive(false);
                buttonImages[i].sprite = buttonOffSprites[i];
            }
        }
    }

    public void StartArcadeMode()
    {
        AudioManager.Instance.PlayButtonClip();
        PlayerPrefs.SetInt("GameMode", 0); // Arcade mode is 0
        GameController.Instance.gameMode = GameMode.Arcade;
        OpenSelectSpinPanel();
    }

    public void StartMatchMode()
    {
        AudioManager.Instance.PlayButtonClip();
        PlayerPrefs.SetInt("GameMode", 1); // Match mode is 1
        GameController.Instance.gameMode = GameMode.Match;
        OpenSelectSpinPanel();
    }

    public void StartTournamentMode()
    {
        AudioManager.Instance.PlayButtonClip();
        //PlayerPrefs.SetInt("GameMode", 2); // Tournament mode is 2
        GameController.Instance.gameMode = GameMode.Tournament;
        OpenSelectSpinPanel();
    }

    public void OpenSelectSpinPanel()
    {
        menuUIController.lastOpenedPanelIndex = panelIndex;
        menuUIController.OpenPanel(3);
    }

    public void OpenWorkshopPanel()
    {
        AudioManager.Instance.PlayButtonClip();
        menuUIController.lastOpenedPanelIndex = panelIndex;
        menuUIController.OpenPanel(2);
    }

    public void OpenShopPanel()
    {
        AudioManager.Instance.PlayButtonClip();
        menuUIController.lastOpenedPanelIndex = panelIndex;
        menuUIController.OpenPanel(5);
    }

    public void OpenOptionPanel()
    {
        AudioManager.Instance.PlayButtonClip();
        //menuUIController.lastOpenedPanelIndex = panelIndex;
        menuUIController.OpenOptionPanel();
    }
}
