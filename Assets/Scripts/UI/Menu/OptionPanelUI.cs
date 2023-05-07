using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanelUI : MonoBehaviour
{
    public MenuUIController menuUIController;
    public int panelIndex = 10;

    public void ClosePanel()
    {
        AudioManager.Instance.PlayButtonClip();
        gameObject.SetActive(false);
    }
}
