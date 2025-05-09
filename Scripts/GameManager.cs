//@Reference https://gamedevbeginner.com/singletons-in-unity-the-right-way/
//@Reference https://docs.unity3d.com/ScriptReference/JsonUtility.html
//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.io.file.writealltext?view=net-9.0
//@Reference https://learn.microsoft.com/en-us/dotnet/api/system.io.file.readalltext?view=net-9.0
//@Reference https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.html
//@Reference https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html
//@Reference https://docs.unity3d.com/Packages/com.unity.textmeshpro@2.0/manual/index.html
//@Reference https://discussions.unity.com/t/gamemanager-singleton-and-weaponmanager-communication/690170
//@Reference https://discussions.unity.com/t/how-to-load-game-to-checkpoint-via-main-menu/247181
//@Reference https://www.youtube.com/watch?v=Wu4SGitck7M&ab_channel=GameDevByKaupenjoe
//@Reference https://www.youtube.com/watch?v=pVXEUtMy_Hc&ab_channel=CodeFriend

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Singleton instance of GameManager to ensure only one exists across scenes
    public static GameManager Instance { get; private set; }

    // The path where the game save file will be stored
    public string saveFilePath => Application.persistentDataPath + "/savegame.json";

    // Reference to a UI text element for showing save/load feedback messages
    public TMP_Text feedbackText;

    // To store the running feedback coroutine
    private Coroutine feedbackCoroutine;  

    private void Awake()
    {
        // Set up singleton pattern - only one instance of GameManager allowed
        if (Instance == null)
        {
            Instance = this;
            // Persist this object between scene loads
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy duplicate GameManager if it already exists
            Destroy(gameObject);
        }
    }

    // Method to save the game state
    public void SaveGame()
    {
        // Find the player object in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // If player is not found, show an error message and exit
        if (player == null)
        {
            ShowFeedback("No player found to save!", Color.red);
            return;
        }

        // Display saving feedback message
        // Show this before saving
        ShowFeedback("Saving Game...", Color.white);  

        // Create game data from the player's current state
        GameData gameData = new GameData
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex,
            playerPosition = new Vector3Data(player.transform.position),
            playerRotation = new Vector3Data(player.transform.eulerAngles),

            // Save quest state
            currentQuestIndex = QuestManager.Instance.CurrentQuestIndex
        };

        // Loop through all quests in the quest manager
        foreach (var quest in QuestManager.Instance.quests)
        {
            // Create a new QuestProgressData instance to hold data for this quest
            var questData = new GameData.QuestProgressData
            {
                // Store whether the quest has been completed
                isCompleted = quest.isCompleted
            };

            // Loop through each objective in the quest
            foreach (var obj in quest.objectives)
            {
                // Add the current amount of progress made on this objective
                questData.objectiveProgress.Add(obj.currentAmount);
            }

            // Add the populated quest data to the list of saved quest progresses
            gameData.questProgresses.Add(questData);
        }

        try
        {
            // Convert game data to JSON and write to file
            string json = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(saveFilePath, json);

            // After saving, show the "Game Saved!" message
            ShowFeedback("Game Saved!", Color.white);
        }
        catch (IOException e)
        {
            // Handle file writing error
            ShowFeedback("Failed to save game: " + e.Message, Color.red);
        }
    }

    // Method to load the saved game state
    public void LoadGame()
    {
        // Check if a saved file exists
        if (File.Exists(saveFilePath))
        {
            // Read and deserialize the saved data
            string json = File.ReadAllText(saveFilePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json);

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Debug.Log("Skipping scene load in editor test mode.");
                QuestManager.Instance.LoadQuestProgress(gameData.currentQuestIndex, gameData.questProgresses);
                return;
            }
#endif

            // Load the saved scene and restore player state
            StartCoroutine(LoadSceneAndRestore(gameData));
        }
        else
        {
            ShowFeedback("No saved game found!", Color.white);
        }
    }

    // Coroutine to load the scene and restore the player state
    private IEnumerator LoadSceneAndRestore(GameData gameData)
    {
        // Load the saved scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameData.sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Small delay to allow objects to initialize
        yield return new WaitForSeconds(0.5f);

        // Find the player object in the loaded scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Restore the player's position and rotation
            player.transform.position = gameData.playerPosition.ToVector3();
            player.transform.eulerAngles = gameData.playerRotation.ToVector3();
            ShowFeedback("Game Loaded!", Color.white);
        }
        else
        {
            ShowFeedback("Player object not found after loading scene!", Color.red);
        }

        // Restore quest progress
        QuestManager.Instance.LoadQuestProgress(gameData.currentQuestIndex, gameData.questProgresses);
    }

    // Show a temporary feedback message to the player
    private void ShowFeedback(string message, Color color)
    {
        if (feedbackText != null)
        {
            // Make feedback text visible and set the message
            feedbackText.gameObject.SetActive(true);
            feedbackText.text = message;
            feedbackText.color = color;

            // Force update UI canvas to reflect changes immediately
            Canvas.ForceUpdateCanvases();

            // If a feedback coroutine is already running, stop it before starting a new one
            if (feedbackCoroutine != null)
            {
                StopCoroutine(feedbackCoroutine);
            }

            // Start a new coroutine to clear the message after a delay
            feedbackCoroutine = StartCoroutine(ClearFeedbackAfterDelay(2f));  // 2-second delay
        }
    }

    // Coroutine to clear the feedback text after a delay
    private IEnumerator ClearFeedbackAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Only proceed if feedbackText is still valid and its GameObject is active
        if (feedbackText != null && feedbackText.gameObject.activeInHierarchy)
        {
            feedbackText.gameObject.SetActive(false);
        }
    }

    // Exit the game (also stops play mode in the editor)
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Serializable class to store game state
    [System.Serializable]
    public class GameData
    {
        public int sceneIndex;
        public Vector3Data playerPosition;
        public Vector3Data playerRotation;

        // Quest progress data
        public int currentQuestIndex;
        public List<QuestProgressData> questProgresses = new List<QuestProgressData>();

        [System.Serializable]
        public class QuestProgressData
        {
            public bool isCompleted;
            public List<int> objectiveProgress = new List<int>();
        }
    }

    // Serializable helper class to store Vector3 data
    [System.Serializable]
    public class Vector3Data
    {
        public float x, y, z;

        public Vector3Data(Vector3 vec)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        // Convert back to Vector3
        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}
