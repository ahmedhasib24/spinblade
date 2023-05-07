using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPurchasePopupPanelUI : MonoBehaviour
{
    public MenuUIController menuUIController;
    public int panelIndex = 4;

    public Text txtNotice;

    public int spinIndex = 0;

    public GameObject btnYes;

    public void Initialize()
    {
        SpinBlade spin = GameController.Instance.basicBlades[spinIndex];
        if (spin.price > GameController.Instance.Gems)
        {
            txtNotice.text = "You do not have enough gem to buy this spin.";
            btnYes.SetActive(false);
        }
        else
        {
            txtNotice.text = "Do you really want to buy this spin for " + spin.price + "?";
            btnYes.SetActive(true);
        }
    }

    public void OnYesButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        GameController.Instance.PurchaseSpin(spinIndex);
        gameObject.SetActive(false);
    }

    public void OnNoButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        gameObject.SetActive(false);
    }
}
