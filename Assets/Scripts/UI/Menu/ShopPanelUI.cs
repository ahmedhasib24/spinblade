using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelUI : MonoBehaviour
{
    public MenuUIController menuUIController;
    public int panelIndex = 5;

    public Text diamondsText;

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        diamondsText.text = GameController.Instance.Gems.ToString();
    }

    public void OnBackButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        //menuUIController.lastOpenedPanelIndex = panelIndex;
        menuUIController.GoBack();
    }
}
