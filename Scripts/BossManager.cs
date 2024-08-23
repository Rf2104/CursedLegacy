using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class BossManager : MonoBehaviour
    {
        BossHealthBar bossHealthBar;
        EnemyStats bossStats;
        public string bossName;

        void Awake()
        {
            bossHealthBar = FindObjectOfType<BossHealthBar>();
            bossStats = GetComponent<EnemyStats>();
        }

        void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetMaxHealthBoss(bossStats.maxHealth);
        }
    }
}