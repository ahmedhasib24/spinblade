using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VSPanel : MonoBehaviour
{
    public GameSceneUIController uiController; 
    public SpinBlade player;
    public SpinBlade opponent;

    public Image imgPlayerIcon;
    public Text txtPlayerName;
    public Slider sliderPlayerAttack;
    public Slider sliderPlayerDefense;
    public Slider sliderPlayerStamina;

    public Image imgOpponentIcon;
    public Text txtOpponentName;
    public Slider sliderOpponentAttack;
    public Slider sliderOpponentDefense;
    public Slider sliderOpponentStamina;

    public void InitializePanel ()
    {
        player = GameSceneController.Instance.player;
        opponent = GameSceneController.Instance.currentOpponents[0];

        

        imgPlayerIcon.sprite = uiController.bladeIconImages[player.iconId];
        txtPlayerName.text = player.name;
        sliderPlayerAttack.value = player.attackRing.attack;
        sliderPlayerDefense.value = player.weightDisk.defense;
        sliderPlayerStamina.value = player.baseRing.stamina;

        imgOpponentIcon.sprite = uiController.bladeIconImages[opponent.iconId];
        txtOpponentName.text = opponent.name;
        sliderOpponentAttack.value = opponent.attackRing.attack;
        sliderOpponentDefense.value = opponent.weightDisk.defense;
        sliderOpponentStamina.value = opponent.baseRing.stamina;
    }
}
