using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace RF
{
    public class DataManager : MonoBehaviour
    {
        private string playerDataPath;

        private void Awake()
        {
            playerDataPath = Application.persistentDataPath + "/playerdata.json";

            if (!File.Exists(playerDataPath))
            {
                // Create a new player data file
                PlayerData playerData = new PlayerData();
                playerData.vigor = 10;
                playerData.endurance = 10;
                playerData.strength = 10;
                playerData.souls = 0;
                playerData.checkPointID = 0;
                playerData.bossesKilled = 0;
                SavePlayerData(playerData);
            }
        }

        void Start()
        {
            Debug.Log("DataManager Start");
        }

        public void SavePlayerData(PlayerData playerData)
        { 
            string jsonData = JsonUtility.ToJson(playerData);
            File.WriteAllText(playerDataPath, jsonData);
        }

        public PlayerData LoadPlayerData()
        {      
            if (File.Exists(playerDataPath))
            {
                Debug.Log("Player data file found.");
                string jsonData = File.ReadAllText(playerDataPath);
                return JsonUtility.FromJson<PlayerData>(jsonData);
            }
            else
            {
                Debug.Log("Player data file not found.");
                return null;
            }
        }
    }
}