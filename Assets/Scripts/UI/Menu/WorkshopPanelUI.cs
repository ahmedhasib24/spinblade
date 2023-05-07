using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopPanelUI : MonoBehaviour
{
    public MenuUIController menuUIController;
    public int panelIndex = 2;

    public List<GameObject> characterModels;

    public List<GameObject> attackRingModels;
    public int selectedAttackRingIndex = 0;

    public List<GameObject> weightDiskModels;
    public int selectedWeightDiskIndex = 0;

    public List<GameObject> baseModels;
    public int selectedBaseIndex = 0;

    public Slider attackSlider;
    public Slider defenseSlider;
    public Slider staminaSlider;

    public List<AttackRing> attackRings;
    public List<WeightDisk> weightDisks;
    public List<BaseRing> baseRings;

    public Transform attackRingTransform;
    public Transform weightDiskTransform;
    public Transform baseRingTransform;

    private void OnEnable()
    {
        int selectedSpin = PlayerPrefs.GetInt("SelectedSpin");
        //if (GameController.Instance.allBlades[selectedSpin].locked == 0)
        //{
        //    selectedAttackRingIndex = GameController.Instance.allBlades[selectedSpin].attackRing.id;
        //    selectedWeightDiskIndex = GameController.Instance.allBlades[selectedSpin].weightDisk.id;
        //    selectedBaseIndex = GameController.Instance.allBlades[selectedSpin].baseRing.id;
        //}
        //else
        //{
        //    selectedAttackRingIndex = 0;
        //    selectedWeightDiskIndex = 0;
        //    selectedBaseIndex = 0;
        //}
        attackRings = GameController.Instance.workshopAttackRings;
        weightDisks = GameController.Instance.workshopWeightDisks;
        baseRings = GameController.Instance.workshopBaseRings;

        ShowAttackRing(selectedAttackRingIndex);
        ShowWeightDisk(selectedWeightDiskIndex);
        ShowBase(selectedBaseIndex);
    }

    private void OnDisable()
    {
        HideAllModels();
    }

    public void OnAttackRingLeft()
    {
        AudioManager.Instance.PlayButtonClip();
        selectedAttackRingIndex -= 1;
        if (selectedAttackRingIndex < 0)
        {
            selectedAttackRingIndex = attackRings.Count - 1;
        }
        ShowAttackRing(selectedAttackRingIndex);
        //PlayerPrefs.SetInt("SelectedSpin", selectedSpinIndex);
    }

    public void OnAttackRingRight()
    {
        AudioManager.Instance.PlayButtonClip();
        selectedAttackRingIndex += 1;
        if (selectedAttackRingIndex == attackRings.Count)
        {
            selectedAttackRingIndex = 0;
        }
        ShowAttackRing(selectedAttackRingIndex);
        //PlayerPrefs.SetInt("SelectedSpin", selectedSpinIndex);
    }

    public void OnWeightDiskLeft()
    {
        AudioManager.Instance.PlayButtonClip();
        selectedWeightDiskIndex -= 1;
        if (selectedWeightDiskIndex < 0)
        {
            selectedWeightDiskIndex = weightDisks.Count - 1;
        }
        ShowWeightDisk(selectedWeightDiskIndex);
    }

    public void OnWeightDiskRight()
    {
        AudioManager.Instance.PlayButtonClip();
        selectedWeightDiskIndex += 1;
        if (selectedWeightDiskIndex == weightDisks.Count)
        {
            selectedWeightDiskIndex = 0;
        }
        ShowWeightDisk(selectedWeightDiskIndex);
        //PlayerPrefs.SetInt("SelectedSpin", selectedSpinIndex);
    }

    public void OnBaseLeft()
    {
        AudioManager.Instance.PlayButtonClip();
        selectedBaseIndex -= 1;
        if (selectedBaseIndex < 0)
        {
            selectedBaseIndex = baseRings.Count - 1;
        }
        ShowBase(selectedBaseIndex);
        //PlayerPrefs.SetInt("SelectedSpin", selectedSpinIndex);
    }

    public void OnBaseRight()
    {
        AudioManager.Instance.PlayButtonClip();
        selectedBaseIndex += 1;
        if (selectedBaseIndex == baseRings.Count)
        {
            selectedBaseIndex = 0;
        }
        ShowBase(selectedBaseIndex);
        //PlayerPrefs.SetInt("SelectedSpin", selectedSpinIndex);
    }

    void ShowAttackRing(int index)
    {
        AttackRing attackRing = attackRings[index];
        attackSlider.value = attackRing.attack;
        //Debug.Log(attackRing.id);
        for (int i = 0; i < attackRingModels.Count; i++)
        {
            if (i == attackRing.id)
            {
                attackRingModels[i].SetActive(true);
                //attackRingModels[i].transform.position = new Vector3(0, -2.5f, 0);
                attackRingTransform.position = new Vector3(0, 0f, 0);
                characterModels[i].SetActive(true);
            }
            else
            {
                attackRingModels[i].SetActive(false);
                characterModels[i].SetActive(false);
            }
        }
    }

    void ShowWeightDisk(int index)
    {
        WeightDisk weightDisk = weightDisks[index];
        defenseSlider.value = weightDisk.defense;

        for (int i = 0; i < weightDiskModels.Count; i++)
        {
            if (i == weightDisk.id)
            {
                weightDiskModels[i].SetActive(true);
                //weightDiskModels[i].transform.position = new Vector3(0, -3.5f, 0);
                weightDiskTransform.position = new Vector3(0, -1f, 0);
            }
            else
            {
                weightDiskModels[i].SetActive(false);
            }
        }
    }

    void ShowBase(int index)
    {
        BaseRing baseRing = baseRings[index];
        staminaSlider.value = baseRing.stamina;

        for (int i = 0; i < baseModels.Count; i++)
        {
            if (i == baseRing.id)
            {
                if(baseModels[i] != null)
                {
                    baseModels[i].SetActive(true);
                    //baseModels[i].transform.position = new Vector3(0, -4.5f, 0);
                    baseRingTransform.position = new Vector3(0, -2f, 0);
                }
            }
            else
            {
                baseModels[i].SetActive(false);
            }
        }
    }

    void HideAllModels()
    {
        for (int i = 0; i < attackRingModels.Count; i++)
        {
            if(attackRingModels[i] != null)
            {
                attackRingModels[i].SetActive(false);
            }
            
            characterModels[i].SetActive(false);
        }

        for (int i = 0; i < weightDiskModels.Count; i++)
        {
            weightDiskModels[i].SetActive(false);
        }

        for (int i = 0; i < baseModels.Count; i++)
        {
            baseModels[i].SetActive(false);
        }
    }

    public void OnBackButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        menuUIController.GoBack();
    }

    public void OnLoadButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        menuUIController.OpenLoadPanel();
        LoadSpinToShow();
    }

    public void OnSaveButtonClick()
    {
        AudioManager.Instance.PlayButtonClip();
        AttackRing ar = attackRings[selectedAttackRingIndex];
        WeightDisk wd = weightDisks[selectedWeightDiskIndex];
        BaseRing br = baseRings[selectedBaseIndex];
        GameController.Instance.AddCustomBlade(ar.id, wd.id, br.id);
    }

    private void LoadSpinToShow()
    {
        attackRingTransform.position = new Vector3(0, 0f, 0);
        weightDiskTransform.position = new Vector3(0, 0f, 0);
        baseRingTransform.position = new Vector3(0, 0f, 0);
    }

    public void ResetSpinParts()
    {
        attackRingTransform.position = new Vector3(0, 0f, 0);
        weightDiskTransform.position = new Vector3(0, -1f, 0);
        baseRingTransform.position = new Vector3(0, -2f, 0);
    }

    public void AddItems(int index)
    {
        SpinBlade spin = GameController.Instance.basicBlades[index];
        attackRings.Add(spin.attackRing);
        weightDisks.Add(spin.weightDisk);
        baseRings.Add(spin.baseRing);
    }
}
