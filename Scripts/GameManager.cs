using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private string saveFilePath => Application.persistentDataPath + "/savegame.json";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No player found to save!");
            return;
        }

        GameData gameData = new GameData
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex,
            playerPosition = new Vector3Data(player.transform.position),
            playerRotation = new Vector3Data(player.transform.eulerAngles)
        };

        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game Saved!");
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json);

            // Load the saved scene
            StartCoroutine(LoadSceneAndRestore(gameData));
        }
        else
        {
            Debug.Log("No saved game found!");
        }
    }

    private IEnumerator LoadSceneAndRestore(GameData gameData)
    {
        // Load the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameData.sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null; // Wait until the scene is loaded
        }

        // Restore player position after the scene loads
        yield return new WaitForSeconds(0.5f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.transform.position = gameData.playerPosition.ToVector3();
            player.transform.eulerAngles = gameData.playerRotation.ToVector3();
            Debug.Log("Player position restored.");
        }
        else
        {
            Debug.LogError("Player object not found after loading scene!");
        }
    }

    // Quit Game
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Structs for saving/loading game data
    [System.Serializable]
    public class GameData
    {
        public int sceneIndex;
        public Vector3Data playerPosition;
        public Vector3Data playerRotation;
    }

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

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}
