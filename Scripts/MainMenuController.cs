using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Method to start a new game
    public void StartNewGame()
    {
        // Load the first level or scene of your game
        SceneManager.LoadScene("SampleScene");
    }

    // Method to load an existing game
    public void LoadGame()
    {
        
        SceneManager.LoadScene("GameScene");
    }

    // Method to quit the game
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}