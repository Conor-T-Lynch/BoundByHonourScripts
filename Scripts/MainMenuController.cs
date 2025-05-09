//@Reference https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
//@Reference https://docs.unity3d.com/Manual/class-InputManager.html
//@Reference https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
//@Reference https://docs.unity3d.com/ScriptReference/Input.GetButtonDown.html
//@Reference https://docs.unity3d.com/2019.1/Documentation/Manual/script-Button.html
//@Reference https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/index.html
//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.io.file.exists?view=net-9.0
//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.io.file.readalltext?view=net-9.0
//@Reference https://docs.unity3d.com/ScriptReference/JsonUtility.html
//@Reference https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
//@Reference https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager-sceneLoaded.html
//@Reference https://docs.unity3d.com/ScriptReference/SceneManagement.LoadSceneMode.html
//@Reference https://docs.unity3d.com/ScriptReference/Application.Quit.html

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // Path to save file stored in the persistent data directory
    private string saveFilePath => Application.persistentDataPath + "/savegame.json";

    // Array of menu buttons to navigate through
    public Button[] menuButtons;

    // Index of the currently selected button
    private int selectedIndex = 0;

    // Threshold to detect vertical input movement
    public float moveSpeed = 0.1f;

    // Called once at the start
    private void Start()
    {
        // Highlight the initial selection
        UpdateSelection();
    }

    // Called once per frame
    private void Update()
    {
        // Get the vertical axis input (up/down)
        float moveVertical = Input.GetAxis("Vertical");

        // Move down through the menu
        if (moveVertical > moveSpeed)
        {
            selectedIndex = Mathf.Min(selectedIndex + 1, menuButtons.Length - 1);
            UpdateSelection();
        }
        // Move up through the menu
        else if (moveVertical < -moveSpeed)
        {
            selectedIndex = Mathf.Max(selectedIndex - 1, 0);
            UpdateSelection();
        }

        // Loop to the top when navigating down at the end
        if (moveVertical > moveSpeed && selectedIndex == menuButtons.Length - 1)
        {
            selectedIndex = 0;
            UpdateSelection();
        }
        // Loop to the bottom when navigating up at the start
        else if (moveVertical < -moveSpeed && selectedIndex == 0)
        {
            selectedIndex = menuButtons.Length - 1;
            UpdateSelection();
        }

        // Submit the current selection using the A button (Submit input)
        if (Input.GetButtonDown("Submit"))
        {
            menuButtons[selectedIndex].onClick.Invoke();
        }
    }

    // Highlight the currently selected button
    private void UpdateSelection()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            menuButtons[i].interactable = (i == selectedIndex);
        }
    }

    // Start a new game by loading the main scene
    public void StartNewGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Load a previously saved game
    public void LoadGame()
    {
        // Check if save file exists
        if (File.Exists(saveFilePath))
        {
            // Read the save data from file
            string json = File.ReadAllText(saveFilePath);
            GameManager.GameData gameData = JsonUtility.FromJson<GameManager.GameData>(json);

            // Subscribe to the scene loaded event
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Load the saved scene
            SceneManager.LoadScene(gameData.sceneIndex);

            // Temporarily store game data for use after scene loads
            GameDataToRestore = gameData;
        }
        else
        {
            Debug.Log("No saved game found!");
        }
    }

    // Temporary storage for game data until the scene has loaded
    private GameManager.GameData GameDataToRestore;

    // Called when a scene finishes loading
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Unsubscribe from the event to avoid duplicate calls
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // If saved data exists, restore the player's state
        if (GameDataToRestore != null)
        {
            // Create a temporary GameObject to restore player position/rotation
            GameObject loader = new GameObject("LoadHelper");
            loader.AddComponent<LoadHelper>().LoadSavedGame(GameDataToRestore);
        }
    }

    // Exit the game (works in build, not in editor)
    public void QuitGame()
    {
        Application.Quit();
    }
}
