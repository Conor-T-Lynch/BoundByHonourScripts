using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LoadHelper : MonoBehaviour
{
    public void LoadSavedGame(GameManager.GameData data)
    {
        // Start coroutine to move player after the scene is fully loaded
        StartCoroutine(LoadGameCoroutine(data));
    }

    private IEnumerator LoadGameCoroutine(GameManager.GameData data)
    {
        // Wait a bit just in case for the scene to fully settle
        yield return null;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = data.playerPosition.ToVector3();
            player.transform.eulerAngles = data.playerRotation.ToVector3();
            Debug.Log("Player position restored.");
        }
        else
        {
            Debug.LogError("Player object not found after loading scene!");
        }

        Destroy(gameObject); // Clean up the helper object
    }
}