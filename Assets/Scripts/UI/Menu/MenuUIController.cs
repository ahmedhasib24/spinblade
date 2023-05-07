using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    public List<GameObject> allPanels;

    public int lastOpenedPanelIndex = 0;
    public int currentOpenedPanelIndex = 0;

    private void Start()
    {
        if(GameController.Instance.isTournamentEnabled == true)
        {
            OpenPanel(8);
        }
        else
        {
            OpenPanel(0);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            switch(currentOpenedPanelIndex)
            {
                case 0:
                    OpenQuitPanel();
                    break;
                case 1:
                    allPanels[1].GetComponent<QuitPanelUI>().OnNoButtonClick();
                    break;
                case 2:
                    allPanels[2].GetComponent<WorkshopPanelUI>().OnBackButtonClick();
                    break;
                case 3:
                    allPanels[3].GetComponent<SelectSpinPanelUI>().OnBackButtonClick();
                    break;
                case 4:
                    allPanels[4].GetComponent<ItemPurchasePopupPanelUI>().OnNoButtonClick();
                    break;
                case 5:
                    allPanels[5].GetComponent<ShopPanelUI>().OnBackButtonClick();
                    break;
                case 6:
                    allPanels[6].GetComponent<SelectArenaPanelUI>().OnBackButtonClick();
                    break;
                case 7:
                    allPanels[7].GetComponent<MapPurchasePopupPanelUI>().OnNoButtonClick();
                    break;
                case 8:
                    allPanels[8].GetComponent<TournamentPanelUI>().OnBackButtonClick();
                    break;
                case 9:
                    currentOpenedPanelIndex = 2;
                    allPanels[9].GetComponent<LoadPanelUI>().OnCloseButtonClick();
                    break;
                case 10:
                    allPanels[10].GetComponent<OptionPanelUI>().ClosePanel();
                    break;
            }
        }
    }

    public void OpenPanel(int index)
    {
        for (int i = 0; i < allPanels.Count; i++)
        {
            if(i == index)
            {
                allPanels[i].SetActive(true);
                currentOpenedPanelIndex = i;
            }
            else
            {
                allPanels[i].SetActive(false);
            }
        }
    }

    public void OpenQuitPanel()
    {
        allPanels[1].SetActive(true);
        currentOpenedPanelIndex = 1;
    }

    public void OpenOptionPanel()
    {
        allPanels[10].SetActive(true);
        currentOpenedPanelIndex = 10;
    }

    public void OpenLoadPanel()
    {
        allPanels[9].SetActive(true);
        currentOpenedPanelIndex = 9;
    }

    public void OpenItemPurchasePanel(int spinIndex)
    {
        allPanels[4].SetActive(true);
        currentOpenedPanelIndex = 3;
        ItemPurchasePopupPanelUI item = allPanels[4].GetComponent<ItemPurchasePopupPanelUI>();
        item.spinIndex = spinIndex;
        item.Initialize();
    }

    public void OpenMapPurchasePanel(int stadiumIndex)
    {
        allPanels[7].SetActive(true);
        currentOpenedPanelIndex = 6;
        MapPurchasePopupPanelUI purchasePanel = allPanels[7].GetComponent<MapPurchasePopupPanelUI>();
        purchasePanel.stadiumIndex = stadiumIndex;
        purchasePanel.Initialize();
    }

    public void UpdateArenaPanel(int stadiumIndex)
    {
        SelectArenaPanelUI arenaPanel = allPanels[6].GetComponent<SelectArenaPanelUI>();
        switch(stadiumIndex - 1)
        {
            case 0:
                PlayerPrefs.SetInt("Salvation", 1);
                break;
            case 1:
                PlayerPrefs.SetInt("BladeFirst", 1);
                break;
            case 2:
                PlayerPrefs.SetInt("BladeFuny", 1);
                break;
            case 3:
                PlayerPrefs.SetInt("Phantom", 1);
                break;
        }
        arenaPanel.Initialize();
    }

    public void UpdateSelectSpinPanel(int index)
    {
        SelectSpinPanelUI spinPanel = allPanels[3].GetComponent<SelectSpinPanelUI>();
        spinPanel.Initialize();
    }

    public void AddItemToworkshop(int index)
    {
        WorkshopPanelUI workshopPanel = allPanels[2].GetComponent<WorkshopPanelUI>();
        workshopPanel.AddItems(index);
    }

    public void GoBack()
    {
        OpenPanel(lastOpenedPanelIndex);
    }
}
