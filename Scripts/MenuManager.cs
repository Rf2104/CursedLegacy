using UnityEngine;
using UnityEngine.UI;

namespace RF
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject levelUpUI; // Reference to your pause menu UI
        public GameObject pauseMenuUI;
        public GameObject controlsUI;
        public bool isPaused = false;

        public void Update()
        {
            if (levelUpUI.activeSelf)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button2))
                {
                    levelUpUI.SetActive(false);
                    Resume();
                    isPaused = false;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                pauseMenuUI.SetActive(true);
                Time.timeScale = 0f;
            }
        }

        public void Pause()
        {
            levelUpUI.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;

        }

        public void Resume()
        {
            levelUpUI.SetActive(false);
            Time.timeScale = 1f;
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}