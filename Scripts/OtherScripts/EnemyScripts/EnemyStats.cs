using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class EnemyStats : MonoBehaviour
    {
        public Animator anim;

        public bool isDead = false;
        public bool isBoss = false;

        public UIEnemyHealthBar enemyHealthBar;
        public BossHealthBar bossHealthBar;

        EnemyLocomotionManager enemyLocomotionManager;

        public PlayerStats playerStats;

        public int maxHealth;
        public int currentHealth;

        [Header("Enemy Stats")]
        public int vigor = 10;
        public int damage = 10;
        public int soulsAwarded = 10;

        public void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            anim = GetComponentInChildren<Animator>();

            if (isBoss)
            {
                bossHealthBar = FindObjectOfType<BossHealthBar>();
            }
            else
            {
                enemyHealthBar = GetComponentInChildren<UIEnemyHealthBar>();
            }
            maxHealth = SetMaxHealthEnemy();
            currentHealth = maxHealth;
        }

        void Start()
        {
            playerStats = FindObjectOfType<PlayerStats>();
            if (isBoss)
            {
                bossHealthBar.SetMaxHealthBoss(maxHealth);
            }
            else 
            { 
                enemyHealthBar.SetMaxHealth(maxHealth);
            }
        }

        // set the max health of the enemy based on the vigor stat
        private int SetMaxHealthEnemy()
        {
            maxHealth = vigor * 10;
            return maxHealth;
        }

        // Common enemy take damage from the player
        public void TakeDamageEnemy(int damage)
        {
            // if the enemy doesnt have a target, the viewable angle is max after he gets hit
            if (enemyLocomotionManager.currentTarget == null)
            {
                anim.CrossFade("drakehurt", 0.2f);
                enemyLocomotionManager.TargetPlayer();
            }

            if (isDead)
            {
                return;
            }

            currentHealth -= damage;

            enemyHealthBar.SetHealth(currentHealth);

            // 30% random chance to play the hurt animation
            int randomHurt = Random.Range(0, 10);
            if (randomHurt > 7)
            {
                anim.CrossFade("drakehurt", 0.2f);
            }
            anim.SetFloat("Speed", 0);

            if (currentHealth <= 0 && !isDead)
            {
                anim.CrossFade("drakedie", 0.1f);
                // keep the enemy in dead animation for 10 seconds
                anim.applyRootMotion = true;
                //destroy the enemy after 10 seconds
                playerStats.souls += soulsAwarded;
                playerStats.SavePlayerStats();
                Destroy(gameObject, 10);
                currentHealth = 0;
                isDead = true;
            }
        }

        // Boss take damage from the player
        public void TakeDamageBoss(int damage)
        {
            if (isDead)
                return;

            currentHealth -= damage;

            bossHealthBar.SetCurrentHealthBoss(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                anim.CrossFade("Mutant Dying", 0.2f);
                playerStats.souls += soulsAwarded;
                playerStats.bossesKilled++;
                playerStats.SavePlayerStats();
                Destroy(gameObject, 10);
                FindObjectOfType<FogWall>().DeactivateFogWall();
                FindObjectOfType<WorldEventManager>().EndBossFightWin();
            }

            if (bossHealthBar != null)
                bossHealthBar.SetCurrentHealthBoss(currentHealth);
        }
    }
}