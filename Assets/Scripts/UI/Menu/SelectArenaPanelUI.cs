using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectArenaPanelUI : MonoBehaviour
{
    public MenuUIController menuUIController;
    public int panelIndex = 6;

    public List<GameObject> stadiums;
    public int selectedArenaIndex = 0;

    public List<Image> buttonImages;
    public List<Sprite> buttonOnSprites;
    public List<Sprite> buttonOffSprites;

    public Text diamondsText;
    public Image arenaImage;
    public GameObject buyButton;
    public GameObject proceedButton;
    public Text priceText;

    public List<GameObject> buttonLocks;

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        HideAllArenas();
    }

    public void Initialize()
    {
        diamondsText.text = GameController.Instance.Gems.ToString();
        SetupArenaButtons();
        selectedArenaIndex = PlayerPrefs.GetInt("SelectedArena");
        SelectArena(selectedArenaIndex);
    }

    private void HideAllArenas()
    {
        for (int i = 0; i < stadiums.Count; i++)
        {
            if(stadiums[i])
            {
                stadiums[i].SetActive(false);
            }
        }
    }

    private void SetupArenaButtons()
    {
        if (PlayerPrefs.GetInt("Salvation") == 1)
        {
            buttonLocks[0].SetActive(false);
        }
        else
        {
            buttonLocks[0].SetActive(true);
        }
        if (PlayerPrefs.GetInt("BladeFirst") == 1)
        {
            buttonLocks[1].SetActive(false);
        }
        else
        {
            buttonLocks[1].SetActive(true);
        }
        if (PlayerPrefs.GetInt("BladeFuny") == 1)
        {
            buttonLocks[2].SetActive(false);
        }
        else
        {
            buttonLocks[2].SetActive(true);
        }
        if (PlayerPrefs.GetInt("Phantom") == 1)
        {
            buttonLocks[3].SetActive(false);
        }
        else
        {
            buttonLocks[3].SetActive(true);
        }
    }

    void HighlightButton(int index)
    {
        for (int i = 0; i < buttonImages.Count; i++)
        {
            if (i == index)
            {
                buttonImages[i].sprite = buttonOnSprites[i];
            }
            else
            {
                buttonImages[i].sprite = buttonOffSprites[i];
            }
        }
    }

    public void SelectArena(int index)
    {
        selectedArenaIndex = index;
        PlayerPrefs.GetInt("SelectedArena", selectedArenaIndex);
        if (selectedArenaIndex == 1)
        {
            if (PlayerPrefs.GetInt("Salvation") == 0)
            {
                buyButton.SetActive(true);
                priceText.text = GameController.Instance.stadiums[selectedArenaIndex].price.ToString();
                proceedButton.SetActive(false);
            }
            else
            {
                buyButton.SetActive(false);
                proceedButton.SetActive(true);
            }

        }
        else if (selectedArenaIndex == 2)
        {
            if (PlayerPrefs.GetInt("BladeFirst") == 0)
            {
                buyButton.SetActive(true);
                priceText.text = GameController.Instance.stadiums[selectedArenaIndex].price.ToString();
                proceedButton.SetActive(false);
            }
            else
            {
                buyButton.SetActive(false);
                proceedButton.SetActive(true);
            }
        }
        else if (selectedArenaIndex == 3)
        {
            if (PlayerPrefs.GetInt("BladeFuny") == 0)
            {
                buyButton.SetActive(true);
                priceText.text = GameController.Instance.stadiums[selectedArenaIndex].price.ToString();
                proceedButton.SetActive(false);
            }
            else
            {
                buyButton.SetActive(false);
                proceedButton.SetActive(true);
            }
        }
        else if (selectedArenaIndex == 4)
        {
            if (PlayerPrefs.GetInt("Phantom") == 0)
            {
                buyButton.SetActive(true);
                priceText.text = GameController.Instance.stadiums[selectedArenaIndex].price.ToString();
                proceedButton.SetActive(false);
            }
            else
            {
                buyButton.SetActive(false);
                proceedButton.SetActive(true);
            }
        }
        else
        {
            //Debug.LogWarning("Else");
            buyButton.SetActive(false);
            proceedButton.SetActive(true);
        }
        HighlightButton(index);
        ShowArena(index);
        ShowArenaDetails(index);
    }

    void ShowArenaDetails(int index)
    {
        arenaImage.sprite = buttonOnSprites[index];
    }

    void ShowArena(int index)
    {
        if (index >= stadiums.Count)
        {
            Debug.LogWarning("Not enough arena models");
            return;
        }
        for (int i = 0; i < stadiums.Count; i++)
        {
            if (i == index)
            {
                stadiums[i].SetActive(true);
            }
            else
            {
                stadiums[i].SetActive(false);
            }
        }
    }

    public void OnPlusButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        HideAllArenas();
        menuUIController.lastOpenedPanelIndex = panelIndex;
        menuUIController.OpenPanel(5);
    }

    public void OnBackButtonClick()
    {
        //menuUIController.lastOpenedPanelIndex = panelIndex;
        AudioManager.Instance.PlayButtonClip();
        menuUIController.lastOpenedPanelIndex = 3;
        menuUIController.GoBack();
    }

    public void OnPurchaseButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        Stadium stadium = GameController.Instance.stadiums[selectedArenaIndex];
        menuUIController.OpenMapPurchasePanel(selectedArenaIndex);
    }

    public void OnProceedButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        PlayerPrefs.SetInt("SelectedArena", selectedArenaIndex);
        if (GameController.Instance.gameMode == GameMode.Tournament)
        {
            AudioManager.Instance.PlayButtonClip();
            HideAllArenas();
            menuUIController.lastOpenedPanelIndex = panelIndex;
            menuUIController.OpenPanel(8);
        }
        else
        {
            SceneManager.LoadScene("Battle");
        }
    }
}
