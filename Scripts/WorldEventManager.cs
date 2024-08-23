using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class WorldEventManager : MonoBehaviour
    {
        public List<FogWall> FogWalls;
        public BossHealthBar bossHealthBar;
        public BossManager bossManager;
        public AudioSource bossMusic;

        public bool bossFight;
        public bool bossAwake;
        public bool bossDead;

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<BossHealthBar>();
        }

        public void StartBossFight()
        {
            bossFight = true;
            bossAwake = true;
            bossHealthBar.SetBossHealthBarActive();

            bossMusic.Play();

            foreach (FogWall fogWall in FogWalls)
            {
                fogWall.AtivateFogWall();
            }
        }

        public void EndBossFightLose()
        {
            bossFight = false;
            bossAwake = false;
            bossHealthBar.SetBossHealthBarInactive();
            FindObjectOfType<BeginBossFight>().gameObject.GetComponent<Collider>().enabled = true;
            bossMusic.Stop();
            foreach (FogWall fogWall in FogWalls)
            {
                fogWall.DeactivateFogWall();
            }
        }

        public void EndBossFightWin()
        {
            bossFight = false;
            bossDead = true;
            bossHealthBar.SetBossHealthBarInactive();
            bossMusic.Stop();
            Debug.Log("Boss is dead");

            foreach (FogWall fogWall in FogWalls)
            {
                fogWall.DeactivateFogWall();
            }
        }
    }
}