using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RF
{
    public class UIEnemyHealthBar : MonoBehaviour
    {
        public Slider slider;
        public Slider easeHealthBarSlider;
        float timeUntilHide = 0;
        private float lerpSpeed = 0.05f;

        private void Update()
        {
            HealthBarSliderHandler();
        }

        public void SetHealth(int health)
        {
            slider.value = health;
            timeUntilHide = 6;
        }

        public void SetMaxHealth(int maxHealth)
        {
            easeHealthBarSlider.maxValue = maxHealth;
            easeHealthBarSlider.value = maxHealth;
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        // Handles the health bar slider
        private void HealthBarSliderHandler() 
        {
            if (slider == null)
                return;

            timeUntilHide -= Time.deltaTime;

            if (slider.value < easeHealthBarSlider.value)
            {
                easeHealthBarSlider.value = Mathf.Lerp(easeHealthBarSlider.value, slider.value, lerpSpeed);
            }

            if (timeUntilHide <= 0)
            {
                timeUntilHide = 0;
                slider.gameObject.SetActive(false);
                easeHealthBarSlider.gameObject.SetActive(false);
            }
            else
            {
                if (!slider.gameObject.activeInHierarchy)
                {
                    slider.gameObject.SetActive(true);
                    easeHealthBarSlider.gameObject.SetActive(true);
                }
            }

            if (slider.value <= 0)
            {
                Destroy(slider.gameObject);
                Destroy(easeHealthBarSlider.gameObject);
            }
        }
    }
}