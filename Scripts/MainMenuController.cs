//@Reference https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class MainMenuController : MonoBehaviour
{
    private string saveFilePath => Application.persistentDataPath + "/savegame.json";

    public void StartNewGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameManager.GameData gameData = JsonUtility.FromJson<GameManager.GameData>(json);

            // Load the saved scene
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to sceneLoaded event
            SceneManager.LoadScene(gameData.sceneIndex);

            // Store the game data for later use
            GameDataToRestore = gameData;
        }
        else
        {
            Debug.Log("No saved game found!");
        }
    }

    private GameManager.GameData GameDataToRestore; // Temporary storage for game data

    // Callback method when a scene has finished loading
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the event

        if (GameDataToRestore != null)
        {
            // Use a temporary object to call the coroutine after the scene loads
            GameObject loader = new GameObject("LoadHelper");
            loader.AddComponent<LoadHelper>().LoadSavedGame(GameDataToRestore);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}