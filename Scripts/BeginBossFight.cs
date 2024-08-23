using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class BeginBossFight : MonoBehaviour
    {
        public WorldEventManager worldEventManager;

        private void Awake()
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (FindObjectOfType<PlayerStats>().bossesKilled == 1)
                    return;

                worldEventManager.StartBossFight();
                // deactivate the collider but not the game object
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
