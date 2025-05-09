//@Reference https://docs.unity3d.com/Manual/coroutines.html
//@Reference https://docs.unity3d.com/ScriptReference/GameObject.FindWithTag.html
//@Reference https://docs.unity3d.com/Manual/Tags.html
//@Reference https://docs.unity3d.com/ScriptReference/Transform.html
//@Reference https://docs.unity3d.com/ScriptReference/Object.Destroy.html
//@Reference https://docs.unity3d.com/ScriptReference/Debug.Log.html
//@Reference https://docs.unity3d.com/ScriptReference/Debug.LogError.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadHelper : MonoBehaviour
{
    // Public method to initiate loading the saved game data
    public void LoadSavedGame(GameManager.GameData data)
    {
        // Start coroutine to move player after the scene is fully loaded
        StartCoroutine(LoadGameCoroutine(data));
    }

    // Coroutine to restore the player's position and rotation after the scene loads
    private IEnumerator LoadGameCoroutine(GameManager.GameData data)
    {
        // Wait one frame to ensure the scene is fully initialized
        yield return null;

        // Attempt to find the player object by its tag
        GameObject player = GameObject.FindWithTag("Player");

        // If the player object is found, restore its position and rotation
        if (player != null)
        {
            player.transform.position = data.playerPosition.ToVector3();
            player.transform.eulerAngles = data.playerRotation.ToVector3();

            // Log successful restoration
            Debug.Log("Player position restored.");
        }
        else
        {
            // Log error if player object was not found
            Debug.LogError("Player object not found after loading scene!");
        }

        // Restore quest progress
        if (QuestManager.Instance != null)

            // Check if the QuestManager instance exists
            if (QuestManager.Instance != null)
            {
                // Restore the quest progress using the saved data
                QuestManager.Instance.LoadQuestProgress(data.currentQuestIndex, data.questProgresses);

                // Log that quest progress has been successfully restored
                Debug.Log("Quest progress restored.");
            }
            else
            {
                // Log an error if the QuestManager instance is not found
                Debug.LogError("QuestManager not found when trying to restore quest progress!");
            }

        // Destroy the LoadHelper object since it's no longer needed
        Destroy(gameObject);
    }
}
