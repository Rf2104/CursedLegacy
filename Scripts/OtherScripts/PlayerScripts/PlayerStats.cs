using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RF
{
    public class PlayerStats : MonoBehaviour
    {
        public DataManager dataManager;

        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public Souls soulsUI;
        public BlackScreen blackScreen;
        public Animator anim;
        public CharacterController controller;

        public bool isDead = false;

        public int maxHealth;
        public int currentHealth;
        public float currentStamina;
        public int maxStamina;
        public int vigor;
        public int endurance;
        public int damage;
        public int souls;
        public int checkPointID;
        public int bossesKilled;

        private void Awake()
        {
            dataManager = FindObjectOfType<DataManager>();
        }

        void Start()
        {
            isDead = false;

            dataManager = FindObjectOfType<DataManager>();
            soulsUI = FindObjectOfType<Souls>();
            blackScreen = FindObjectOfType<BlackScreen>();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            anim = GetComponentInChildren<Animator>();
            controller = GetComponent<CharacterController>();

            FindObjectOfType<LevelUpInteractable>().interactorSource = gameObject.transform;

            LoadPlayerStats();
        }

        public void LoadPlayerStats()
        {
            // load the player stats from the player data file
            PlayerData playerData = dataManager.LoadPlayerData();
            
            if (playerData != null && PlayerPrefs.GetInt("NewGame", 0) == 1)
            {
                Debug.Log("Loading player data");
                vigor = playerData.vigor;
                endurance = playerData.endurance;
                damage = playerData.strength;
                souls = playerData.souls;

                checkPointID = playerData.checkPointID;
                bossesKilled = playerData.bossesKilled;
            }
            else
            {
                Debug.Log("New Game");
                PlayerPrefs.SetInt("FirstTime", 1);
                PlayerPrefs.SetInt("NewGame", 1);
                vigor = 10;
                endurance = 10;
                damage = 10;
                souls = 0;
                checkPointID = 0;
                bossesKilled = 0;

                // if the player is new, set the player stats to default values
                PlayerData newPlayerData = new PlayerData();
                newPlayerData.vigor = vigor;
                newPlayerData.endurance = endurance;
                newPlayerData.strength = damage;
                newPlayerData.souls = souls;
                newPlayerData.checkPointID = checkPointID;
                newPlayerData.bossesKilled = bossesKilled;
                dataManager.SavePlayerData(newPlayerData);
            }

            maxHealth = SetMaxHealthFromVigorLevel();
            currentHealth = maxHealth;
            maxStamina = SetMaxStaminaFromEnduranceLevel();
            currentStamina = maxStamina;
            healthBar.SetMaxHealth(maxHealth);
            staminaBar.SetMaxStamina(maxStamina);
            soulsUI.UpdateUIText(souls);
        }

        public void SavePlayerStats()
        {
            // save the player stats to the player data file
            PlayerData playerData = new PlayerData();
            playerData.vigor = vigor;
            playerData.endurance = endurance;
            playerData.strength = damage;
            playerData.souls = souls;
            playerData.checkPointID = checkPointID;
            playerData.bossesKilled = bossesKilled;
            dataManager.SavePlayerData(playerData);

            maxHealth = SetMaxHealthFromVigorLevel();
            currentHealth = maxHealth;
            maxStamina = SetMaxStaminaFromEnduranceLevel();
            currentStamina = maxStamina;
            healthBar.SetMaxHealth(maxHealth);
            staminaBar.SetMaxStamina(maxStamina);
            soulsUI.UpdateUIText(souls);
            Debug.Log("Player stats saved");
        }

        private int SetMaxHealthFromVigorLevel()
        {
            maxHealth = vigor * 10;
            return maxHealth;
        }

        private int SetMaxStaminaFromEnduranceLevel()
        {
            maxStamina = endurance * 10;
            return maxStamina;
        }

        public void TakeDamage(int damage)
        {
            // if the player is dead or rolling, don't take damage
            if (isDead || FindObjectOfType<PlayerController>().imunityFrames)
                return;

            currentHealth -= damage;
            healthBar.SetCurrentHealth(currentHealth);
            if (currentHealth <= 0 && !isDead)
            {
                currentHealth = 0;
                anim.applyRootMotion = true;
                anim.CrossFade("drakedie", 0.2f);
                FindObjectOfType<PlayerController>().enabled = false;
                // wait for 3 seconds before respawning the player
                blackScreen.HandleBlackScreen();
                isDead = true;
            }
        }

        public void UseStamina(int amount)
        {
            currentStamina -= amount * 0.1f;
            staminaBar.SetCurrentStamina(currentStamina);
        }
    }
}