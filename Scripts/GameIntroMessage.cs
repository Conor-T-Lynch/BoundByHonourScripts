//@Reference https://docs.unity3d.com/ScriptReference/Input.GetKeyDown.html
//@Reference https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html
//@Reference https://docs.unity3d.com/ScriptReference/Debug.Log.html
//@Reference https://docs.unity3d.com/Manual/class-InputManager.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntroMessage : MonoBehaviour
{
    // A reference to the player controls GameObject
    public GameObject playerControls;

    void Update()
    {
        // Check if the player presses the "B" button on a gamepad (Joystick Button 1).
        if (Input.GetKeyDown("joystick button 1"))
        {
            // Log a debug message to confirm the button press was detected.
            Debug.Log("Toggle pressed!");

            // Toggle the visibility of the playerControls GameObject.
            // If it's currently active, it will be deactivated, and vice versa.
            playerControls.SetActive(!playerControls.activeSelf);
        }
    }
}
