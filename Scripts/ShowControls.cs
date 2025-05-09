//@Reference https://docs.unity3d.com/ScriptReference/Input.GetKeyDown.html
//@Reference https://docs.unity3d.com/Manual/class-InputManager.html
//@Reference https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html
//@Reference https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowControls : MonoBehaviour
{
    // Reference to the UI GameObject that displays player control instructions
    public GameObject playerControls;

    void Update()
    {
        // Check if the player presses the 'B' button (Joystick Button 1)
        if (Input.GetKeyDown("joystick button 1"))
        {
            // Log to the console for debugging
            Debug.Log("Toggle pressed!");

            // Toggle the active state of the playerControls UI
            playerControls.SetActive(!playerControls.activeSelf);
        }
    }
}
