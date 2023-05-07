using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPurchasePopupPanelUI : MonoBehaviour
{
    public MenuUIController menuUIController;
    public int panelIndex = 7;

    public Text txtNotice;

    public int stadiumIndex = 0;

    public GameObject btnYes;

    public void Initialize()
    {
        Stadium stadium = GameController.Instance.stadiums[stadiumIndex];
        if (stadium.price > GameController.Instance.Gems)
        {
            txtNotice.text = "You do not have enough gem to buy this stadium.";
            btnYes.SetActive(false);
        }
        else
        {
            txtNotice.text = "Do you really want to buy this stadium for " + stadium.price + "?";
            btnYes.SetActive(true);
        }
    }

    public void OnYesButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        GameController.Instance.PurchaseStadium(stadiumIndex);
        gameObject.SetActive(false);
    }

    public void OnNoButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        gameObject.SetActive(false);
    }
}
