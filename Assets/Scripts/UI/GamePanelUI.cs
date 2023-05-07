using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelUI : MonoBehaviour
{
    public GameSceneUIController uiController;
    public SpinBlade player;
    public List<SpinBlade> opponents;

    public Image imgPlayerIcon;
    public Text txtPlayerName;
    public Slider sliderPlayerEnergy;
    public Slider sliderPlayerHealth;

    public List<Image> imgOpponentIcons;
    public List<Text> txtOpponentNames;
    public List<Slider> sliderOpponentEnergies;
    public List<Slider> sliderOpponentHealths;

    public List<GameObject> opponentInfos;

    public void InitializePanel()
    {
        player = GameSceneController.Instance.player;
        opponents = GameSceneController.Instance.currentOpponents;
        imgPlayerIcon.sprite = uiController.bladeIconImages[player.iconId];
        txtPlayerName.text = player.name;
        sliderPlayerEnergy.value = player.initialEnergy;
        sliderPlayerHealth.value = DefenseToHealth(player.weightDisk.defense, player.weightDisk.defense);

        for (int i = 0; i < opponents.Count; i++)
        {
            opponentInfos[i].SetActive(true);
            sliderOpponentEnergies[i].value = opponents[i].initialEnergy;
            sliderOpponentHealths[i].value = DefenseToHealth(opponents[i].weightDisk.defense, opponents[i].weightDisk.defense);
            imgOpponentIcons[i].sprite = uiController.bladeIconImages[opponents[i].iconId];
            txtOpponentNames[i].text = opponents[i].name;
        }
    }

    float StaminaToSlider(float staminaValue, float maxStaminaValue)
    {
        float value = 0f;
        double scale = (double)(100 - 0) / maxStaminaValue;
        value = (int)(0 + ((staminaValue - 0) * scale));
        return value;
    }

    float DefenseToHealth(float defenseValue, float maxDefenseValue)
    {
        float value = 0f;
        double scale = (double)(100 - 0) / maxDefenseValue;
        value = (int)(0 + ((defenseValue - 0) * scale));
        return value;
    }

    public void UpdateEnergySliders(SpinController player, List<SpinController> opponents)
    {
        sliderPlayerEnergy.value = player.Energy;
        for (int i = 0; i < opponents.Count; i++)
        {
            sliderOpponentEnergies[i].value = opponents[i].Energy;
        }
    }

    public void UpdatePlayerHealthSlider(float playerDefense)
    {
        //Debug.Log("UpdatePlayer");
        sliderPlayerHealth.value = DefenseToHealth(playerDefense, player.weightDisk.defense);
    }

    public void UpdateOpponentHealthSlider(int id, float opponentDefense)
    {
        sliderOpponentHealths[id].value = DefenseToHealth(opponentDefense, opponents[id].weightDisk.defense);
    }
}
