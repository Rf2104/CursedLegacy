using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RF
{
    public class BossHealthBar : MonoBehaviour
    {
        public Slider slider;
        public Text bossName;
        public Slider easeHealthBarSlider;
        private float lerpSpeed = 0.05f;

        private void Awake()
        {
            bossName = GetComponentInChildren<Text>();
        }

        private void Start()
        {
            SetBossHealthBarInactive();
        }

        private void Update()
        {
            BossHealthBarSliderHandler();
        }

        public void SetBossHealthBarActive()
        {
            easeHealthBarSlider.gameObject.SetActive(true);
            slider.gameObject.SetActive(true);
        }

        public void SetBossHealthBarInactive()
        {
            easeHealthBarSlider.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
        }

        public void SetBossName(string name) 
        { 
            bossName.text = name;
        }

        public void SetMaxHealthBoss(int maxHealth)
        {
            easeHealthBarSlider.maxValue = maxHealth;
            easeHealthBarSlider.value = maxHealth;
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        public void SetCurrentHealthBoss(int currentHealth)
        {
            slider.value = currentHealth;
        }

        // Handles the health bar slider
        private void BossHealthBarSliderHandler()
        {
            if (slider == null)
                return;

            // Lerp the health bar slider
            if (slider.value < easeHealthBarSlider.value)
            {
                easeHealthBarSlider.value = Mathf.Lerp(easeHealthBarSlider.value, slider.value, lerpSpeed);
            }

            if (slider.value <= 0)
            {
                SetBossHealthBarInactive();
            }
        }
    }
}