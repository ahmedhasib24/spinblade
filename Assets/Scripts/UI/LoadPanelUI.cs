using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPanelUI : MonoBehaviour
{
    public MenuUIController menuUIController;
    public int panelIndex = 9;

    public WorkshopPanelUI workshopPanelUI;

    public void OnCloseButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        gameObject.SetActive(false);
        workshopPanelUI.ResetSpinParts();
    }
}
