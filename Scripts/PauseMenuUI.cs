using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class PauseMenuUI : MonoBehaviour
    {
        public void Resume()
        {
            Time.timeScale = 1f;
            FindObjectOfType<MenuManager>().pauseMenuUI.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void OpenControls()
        { 
            FindObjectOfType<MenuManager>().pauseMenuUI.SetActive(false);
            FindObjectOfType<MenuManager>().controlsUI.SetActive(true);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}