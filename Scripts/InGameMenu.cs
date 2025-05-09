//@Reference https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
//@Reference https://docs.unity3d.com/ScriptReference/Input.GetButtonDown.html
//@Reference https://docs.unity3d.com/ScriptReference/Time-timeScale.html
//@Reference https://docs.unity3d.com/ScriptReference/Mathf.Min.html
//@Reference https://docs.unity3d.com/ScriptReference/Mathf.Max.html
//@Reference https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.html

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI; 

public class InGameMenu : MonoBehaviour
{
    // UI panel for the in-game menu
    public GameObject menuPanel;
    // Track whether the menu is currently open (and game is paused)
    private bool isPaused = false;
    // Array of buttons on the menu (assign in Inspector)
    public Button[] menuButtons;
    // Index of the currently selected button
    private int selectedIndex = 0;
    // Threshold for detecting vertical axis movement
    public float moveSpeed = 0.1f;         

    void Start()
    {
        // Hide the menu at the start of the game
        menuPanel.SetActive(false);

        // Ensure buttons are properly assigned
        if (menuButtons != null && menuButtons.Length > 0)
        {
            // Highlight the first button
            UpdateSelection(); 
        }
        else
        {
            Debug.LogError("Menu buttons are not assigned in the Inspector!");
        }
    }

    void Update()
    {
        // Toggle the menu when the "Start" button is pressed (mapped in Input settings)
        if (Input.GetButtonDown("Start"))
        {
            ToggleMenu();
        }

        // Read vertical movement input
        float moveVertical = Input.GetAxis("Vertical");

        // Menu navigation - DOWN
        if (moveVertical > moveSpeed)
        {
            selectedIndex = Mathf.Min(selectedIndex + 1, menuButtons.Length - 1);
            UpdateSelection();
        }
        // Menu navigation - UP
        else if (moveVertical < -moveSpeed)
        {
            selectedIndex = Mathf.Max(selectedIndex - 1, 0);
            UpdateSelection();
        }

        // Optional: Looping menu navigation
        if (moveVertical > moveSpeed && selectedIndex == menuButtons.Length - 1)
        {
            selectedIndex = 0;
            UpdateSelection();
        }
        else if (moveVertical < -moveSpeed && selectedIndex == 0)
        {
            selectedIndex = menuButtons.Length - 1;
            UpdateSelection();
        }

        // Select menu item with "Submit" (usually mapped to A button)
        if (Input.GetButtonDown("Submit"))
        {
            menuButtons[selectedIndex].onClick.Invoke();
        }
    }

    // Opens or closes the menu and pauses/unpauses the game
    public void ToggleMenu()
    {
        isPaused = !isPaused;
        menuPanel.SetActive(isPaused);
        // Pauses gameplay while menu is active
        Time.timeScale = isPaused ? 0 : 1; 
    }

    // Called when "Save" is selected in the menu
    public void SaveGame()
    {
        // Calls the singleton GameManager to save the game
        GameManager.Instance.SaveGame(); 
    }

    // Called when "Load" is selected
    public void LoadGame()
    {
        GameManager.Instance.LoadGame();
        // Close the menu after loading
        ToggleMenu(); 
    }

    // Called when "Quit" is selected
    public void QuitGame()
    {
        // Calls GameManager to quit the game
        GameManager.Instance.QuitGame(); 
    }

    // Visually updates which button is currently selected
    private void UpdateSelection()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            // Set only the selected button as interactable
            menuButtons[i].interactable = (i == selectedIndex);
        }
    }
}
