using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF
{
    public class ControlMenuUI : MonoBehaviour
    {
        public void Back()
        {
            FindObjectOfType<MenuManager>().controlsUI.SetActive(false);
            FindObjectOfType<MenuManager>().pauseMenuUI.SetActive(true);
        }
    }
}