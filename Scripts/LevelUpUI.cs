using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RF
{
    public class LevelUpUI : MonoBehaviour
    {
        public PlayerStats playerStats;

        public Button confirmButton;

        [Header("Souls")]
        public TextMeshProUGUI currentSouls;
        public TextMeshProUGUI requiredSoulsText;
        private int requiredSouls;

        [Header("Vigor")]
        public Slider vigorSlider;
        public TextMeshProUGUI currentVigor;
        public TextMeshProUGUI finalVigor;

        [Header("Endurance")]
        public Slider EnduranceSlider;
        public TextMeshProUGUI currentEndurance;
        public TextMeshProUGUI finalEndurance;

        [Header("Strength")]
        public Slider strengthSlider;
        public TextMeshProUGUI currentStrength;
        public TextMeshProUGUI finalStrength;

        // if the player has enough souls, level up the stats
        public void ConfirmPlayerStats()
        {
            requiredSouls = 0;
            CalculateSoulCost();
            if (playerStats.souls >= requiredSouls)
            {
                playerStats.vigor = (int)vigorSlider.value;
                playerStats.endurance = (int)EnduranceSlider.value;
                playerStats.damage = (int)strengthSlider.value;
                playerStats.souls -= requiredSouls;
                currentSouls.text = playerStats.souls.ToString();
                currentEndurance.text = playerStats.endurance.ToString();
                currentVigor.text = playerStats.vigor.ToString();
                currentStrength.text = playerStats.damage.ToString();
                playerStats.SavePlayerStats();
                requiredSoulsText.text = "0";
                requiredSouls = 0;
                vigorSlider.minValue = playerStats.vigor;
                EnduranceSlider.minValue = playerStats.endurance;
                strengthSlider.minValue = playerStats.damage;
            }
        }

        public void CalculateSoulCost() 
        {
            requiredSouls = (int)((vigorSlider.value - playerStats.vigor) + (EnduranceSlider.value - playerStats.endurance) + (strengthSlider.value - playerStats.damage)) * 100;
            requiredSoulsText.text = requiredSouls.ToString();
        }

        // Update the stats of the player
        public void UpdateVigorValue()
        {
            finalVigor.text = vigorSlider.value.ToString();
            CalculateSoulCost();
        }

        public void UpdateEnduranceValue()
        {
            finalEndurance.text = EnduranceSlider.value.ToString();
            CalculateSoulCost();
        }

        public void UpdateStrengthValue()
        {
            finalStrength.text = strengthSlider.value.ToString();
            CalculateSoulCost();
        }

        private void OnEnable()
        {
            playerStats = FindObjectOfType<PlayerStats>();

            vigorSlider.value = playerStats.vigor;
            currentVigor.text = playerStats.vigor.ToString();
            finalVigor.text = playerStats.vigor.ToString();
            vigorSlider.minValue = playerStats.vigor;
            vigorSlider.maxValue = 99;

            EnduranceSlider.value = playerStats.endurance;
            currentEndurance.text = playerStats.endurance.ToString();
            finalEndurance.text = playerStats.endurance.ToString();
            EnduranceSlider.minValue = playerStats.endurance;
            EnduranceSlider.maxValue = 99;

            strengthSlider.value = playerStats.damage;
            currentStrength.text = playerStats.damage.ToString();
            finalStrength.text = playerStats.damage.ToString();
            strengthSlider.minValue = playerStats.damage;
            strengthSlider.maxValue = 99;

            currentSouls.text = playerStats.souls.ToString();
        }
    }
}
