using System.Collections;
using System.Collections.Generic;
using RF;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{   
    public GameObject drakePrefab;
    public GameObject bossPrefab;
    public GameObject boss2Prefab;
    public List<Transform> drakeSpawnPoints;
    public Transform bossSpawnPoint;
    public Transform bossSpawnPoint2;
   // public GameObject otherPrefab;
    //public Transform[] otherSpawnPoints;

    public void RemoveAllEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        foreach (GameObject boss1 in GameObject.FindGameObjectsWithTag("Boss1"))
        {
            FindObjectOfType<WorldEventManager>().EndBossFightLose();
            Destroy(boss1);
        }

        foreach (GameObject boss2 in GameObject.FindGameObjectsWithTag("Boss2"))
        {
            Destroy(boss2);
        }
    }

    public void SpawnEnemies(int bossesKilled) {
        // find all the spawn points in the scene (tag EnemyCheckpoint)
        foreach (GameObject enemySpawnPoint in GameObject.FindGameObjectsWithTag("EnemyCheckpoint"))
        {
            drakeSpawnPoints.Add(enemySpawnPoint.GetComponent<Transform>());
        }

        foreach (Transform drakeSpawnPoint in drakeSpawnPoints)
        {
            Instantiate(drakePrefab, drakeSpawnPoint.position, drakeSpawnPoint.rotation);
        }
        drakeSpawnPoints.Clear();

        if (bossesKilled == 0)
        {
            bossSpawnPoint = GameObject.FindGameObjectWithTag("BossCheckpoint1").GetComponent<Transform>();
            Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            //bossSpawnPoint2 = GameObject.FindGameObjectWithTag("BossCheckpoint2").GetComponent<Transform>();
            //Instantiate(boss2Prefab, bossSpawnPoint2.position, bossSpawnPoint2.rotation);
        }
        else if (bossesKilled == 1)
        {
            //bossSpawnPoint2 = GameObject.FindGameObjectWithTag("BossCheckpoint2").GetComponent<Transform>();
            //Instantiate(boss2Prefab, bossSpawnPoint2.position, bossSpawnPoint2.rotation);
        }
        else
            return;
    }
}
