using System.Collections;
using System.Collections.Generic;
using RF;
using UnityEngine;

namespace RF
{
    public class LevelUpInteractable : MonoBehaviour
    {
        public GameObject levelUpUI;
        public Transform interactorSource;

        private void Start()
        {
            levelUpUI.SetActive(false);
        }

        // press the button near the bonefire to pop up the level up UI
        private void Update()
        {
            if (interactorSource != null)
            {
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0) && !FindObjectOfType<MenuManager>().isPaused)
                {
                    Ray r = new Ray(interactorSource.position, interactorSource.forward);
                    if (Physics.Raycast(r, out RaycastHit hit, 1f))
                    {
                        if (hit.collider.gameObject.CompareTag("cp1"))
                        {
                            FindObjectOfType<PlayerStats>().checkPointID = 1;
                            FindObjectOfType<PlayerStats>().SavePlayerStats();
                            FindObjectOfType<MenuManager>().Pause();
                            levelUpUI.SetActive(true);
                        }
                        else if (hit.collider.gameObject.CompareTag("cp2"))
                        {
                            FindObjectOfType<PlayerStats>().checkPointID = 2;
                            FindObjectOfType<PlayerStats>().SavePlayerStats();
                            FindObjectOfType<MenuManager>().Pause();
                            levelUpUI.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}