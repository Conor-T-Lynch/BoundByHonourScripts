//@Reference https://docs.unity3d.com/ScriptReference/MonoBehaviour.Invoke.html
//@Reference https://docs.unity3d.com/ScriptReference/Input.GetKeyDown.html
//@Reference https://docs.unity3d.com/Manual/class-InputManager.html
//@Reference https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
//@Reference https://docs.unity3d.com/Manual/BuildSettings.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    // Time delay before allowing player input to return to the main menu
    public float delayBeforeReturn = 1f;

    // Flag to check if returning is allowed
    private bool canReturn = false;

    // Called when the GameObject is initialized
    public void Start()
    {
        // Wait for a short delay before enabling input
        Invoke(nameof(EnableInput), delayBeforeReturn);
    }

    // Enables input so the player can return to the main menu
    public void EnableInput()
    {
        canReturn = true;
    }

    // Update is called once per frame
    public void Update()
    {
        // Check if the delay is complete and player presses the "A" button (joystick button 0)
        if (canReturn && Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            ReturnToMainMenu();
        }
    }

    // Loads the main menu scene
    public void ReturnToMainMenu()
    {
        // Replace "MainMenu" with the actual name of your main menu scene in the build settings
        SceneManager.LoadScene("MainMenu");
    }
}
