using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    // Starts a new game by loading the first scene and setting the player stats to default values
    public void NewGame()
    {
        // Load the first scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.SetInt("NewGame", 0); // Set the NewGame key to 0 to indicate that this is a new game
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.GetInt("FirstTime", 0) == 1)
        {
            // Load the first scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("NewGame", 1); // Set the NewGame key to 1 to indicate that this is a continued game
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
