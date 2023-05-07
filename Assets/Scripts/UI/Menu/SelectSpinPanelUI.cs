using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSpinPanelUI : MonoBehaviour
{
    public MenuUIController menuUIController;
    public int panelIndex = 3;

    //public List<GameObject> spinBladesModels;
    public int selectedSpinIndex;

    public Text diamondsText;
    public Text spinNameText;
    public Text spinLevelText;
    public Slider spinAttackSlider;
    public Slider spinDefenseSlider;
    public Slider spinStaminaSlider;
    public GameObject imgLock;
    //public GameObject btnPurchase;
    public Text txtPrice;
    public GameObject btnNextStep;

    public List<GameObject> attackRingModels;
    //public int selectedAttackRingIndex = 0;

    public List<GameObject> weightDiskModels;
    //public int selectedWeightDiskIndex = 0;

    public List<GameObject> baseModels;
    //public int selectedBaseIndex = 0;
    public List<GameObject> characterModels;

    public Transform attackRingTransform;
    public Transform weightDiskTransform;
    public Transform baseRingTransform;

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        //HideSpins();
        imgLock.SetActive(false);
    }

    public void Initialize()
    {
        imgLock.SetActive(false);
        //btnPurchase.SetActive(false);
        selectedSpinIndex = PlayerPrefs.GetInt("SelectedSpin");
        diamondsText.text = GameController.Instance.Gems.ToString();
        ShowSpin(selectedSpinIndex);
    }

    public void OnRightArrowClick()
    {
        //if(selectedSpinIndex < spinBladesModels.Count - 1)
        //{
        AudioManager.Instance.PlayButtonClip();
        selectedSpinIndex += 1;
        if (selectedSpinIndex == GameController.Instance.allBlades.Count)
        {
            selectedSpinIndex = 0;
        }
        ShowSpin(selectedSpinIndex);
        
        //}
    }

    public void OnLeftArrowClick()
    {
        //if (selectedSpinIndex > 0)
        //{
        AudioManager.Instance.PlayButtonClip();
        selectedSpinIndex -= 1;
        if (selectedSpinIndex < 0)
        {
            selectedSpinIndex = GameController.Instance.allBlades.Count - 1;
        }
        ShowSpin(selectedSpinIndex);
        //}
    }

    void ShowSpin(int index)
    {
        SpinBlade spinBlade = GameController.Instance.allBlades[index];
        spinNameText.text = spinBlade.name;
        spinLevelText.text = spinBlade.level.ToString();
        spinAttackSlider.value = spinBlade.attackRing.attack;
        spinDefenseSlider.value = spinBlade.weightDisk.defense;
        spinStaminaSlider.value = spinBlade.baseRing.stamina;

        if (spinBlade.locked == 0)
        {
            imgLock.SetActive(false);
            btnNextStep.SetActive(true);
            //tnPurchase.SetActive(false);
        }
        else
        {
            imgLock.SetActive(true);
            btnNextStep.SetActive(false);
            //btnPurchase.SetActive(true);
            txtPrice.text = spinBlade.price.ToString();
        }

        for (int i = 0; i < attackRingModels.Count; i++)
        {
            if (i == spinBlade.attackRing.id)
            {
                attackRingModels[i].SetActive(true);
                //attackRingModels[i].transform.position = new Vector3(0, -2.5f, 0);
                attackRingTransform.position = Vector3.zero;
                //Showing characters depending on attack ring
                characterModels[i].SetActive(true);
            }
            else
            {
                attackRingModels[i].SetActive(false);
                characterModels[i].SetActive(false);
            }
            if (i == spinBlade.weightDisk.id)
            {
                weightDiskModels[i].SetActive(true);
                //weightDiskModels[i].transform.position = new Vector3(0, -2.5f, 0);
                weightDiskTransform.position = Vector3.zero;
            }
            else
            {
                weightDiskModels[i].SetActive(false);
            }
            if (i == spinBlade.baseRing.id)
            {
                baseModels[i].SetActive(true);
                //baseModels[i].transform.position = new Vector3(0, -2.5f, 0);
                baseRingTransform.position = Vector3.zero;
            }
            else
            {
                baseModels[i].SetActive(false);
            }
        }

    }

    void HideSpins()
    {
        for (int i = 0; i < GameController.Instance.basicBlades.Count; i++)
        {
            attackRingModels[i].SetActive(false);
            characterModels[i].SetActive(false);
            weightDiskModels[i].SetActive(false);
            baseModels[i].SetActive(false);
        }
    }

    public void OpenWorkshopPanel()
    {
        AudioManager.Instance.PlayButtonClip();
        HideSpins();
        menuUIController.lastOpenedPanelIndex = panelIndex;
        menuUIController.OpenPanel(2);
    }

    public void OpenMapPurchasePopupPanel()
    {
        AudioManager.Instance.PlayButtonClip();
        HideSpins();
        PlayerPrefs.SetInt("SelectedSpin", selectedSpinIndex);
        menuUIController.lastOpenedPanelIndex = panelIndex;
        menuUIController.OpenPanel(6);
    }

    public void OnBackButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        HideSpins();
        menuUIController.lastOpenedPanelIndex = panelIndex;
        menuUIController.OpenPanel(0);
    }

    public void OnPlusButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        HideSpins();
        menuUIController.lastOpenedPanelIndex = panelIndex;
        menuUIController.OpenPanel(5);
    }

    public void OnPurchaseButtonClick()
    {
        //imgLock.SetActive(false);
        AudioManager.Instance.PlayButtonClip();
        menuUIController.OpenItemPurchasePanel(selectedSpinIndex);
    }
}
