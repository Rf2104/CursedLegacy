using System.Collections;
using System.Collections.Generic;
using RF;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    DataManager dataManager;
    public GameObject playerPrefab;

    void Start()
    {
         dataManager = FindObjectOfType<DataManager>();

        PlayerData playerData = dataManager.LoadPlayerData();

        if (playerData == null)
        {
            Debug.LogError("player data not found.");
        }

        int checkpoint = playerData.checkPointID;
        Transform currentCheckpoint;

        if (checkpoint == 0)
            currentCheckpoint = GameObject.FindGameObjectWithTag("Start").transform;
        else if (checkpoint == 1)
            currentCheckpoint = GameObject.FindGameObjectWithTag("Mid").transform;
        else
            currentCheckpoint = GameObject.FindGameObjectWithTag("End").transform;

        Instantiate(playerPrefab, currentCheckpoint.position, Quaternion.identity);
        playerPrefab.GetComponent<PlayerStats>().vigor = playerData.vigor;
        playerPrefab.GetComponent<PlayerStats>().endurance = playerData.endurance;
        playerPrefab.GetComponent<PlayerStats>().damage = playerData.strength;
        playerPrefab.GetComponent<PlayerStats>().checkPointID = playerData.checkPointID;
        playerPrefab.GetComponent<PlayerStats>().bossesKilled = playerData.bossesKilled;

        FindObjectOfType<EnemySpawn>().SpawnEnemies(playerData.bossesKilled);

        FadeOut();
    }

    // Black screen fade Out when the game starts
    public void FadeOut()
    {
        StartCoroutine(FadeOutBlackScreen());
    }

    private IEnumerator FadeOutBlackScreen()
    {
        yield return new WaitForSeconds(1);
        float duration = 2.0f;
        float targetAlpha = 0.0f;
        float startAlpha = FindObjectOfType<BlackScreen>().blackScreen.color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            FindObjectOfType<BlackScreen>().blackScreen.color = new Color(FindObjectOfType<BlackScreen>().blackScreen.color.r, FindObjectOfType<BlackScreen>().blackScreen.color.g, FindObjectOfType<BlackScreen>().blackScreen.color.b, alpha);
            yield return null;
        }

        FindObjectOfType<BlackScreen>().blackScreen.color = new Color(FindObjectOfType<BlackScreen>().blackScreen.color.r, FindObjectOfType<BlackScreen>().blackScreen.color.g, FindObjectOfType<BlackScreen>().blackScreen.color.b, targetAlpha);
    }
}
