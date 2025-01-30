using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    public GameObject menuPanel;
    private bool isPaused = false;

    void Start()
    {
        menuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isPaused = !isPaused;
        menuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void SaveGame()
    {
        GameManager.Instance.SaveGame();
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadGame();
        ToggleMenu(); // Close menu after loading
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}

