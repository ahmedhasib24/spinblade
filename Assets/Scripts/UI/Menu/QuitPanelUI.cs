using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitPanelUI : MonoBehaviour
{
    public MenuUIController menuUIController;
    public int panelIndex = 1;

    public void OnYesButtonClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OnNoButtonClick()
    {
        gameObject.SetActive(false);
    }
}
