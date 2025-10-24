using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";

    [Header("UI References")]
    public GameObject pauseText; 

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Debug.Log("Game Paused");
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseText != null)
            pauseText.SetActive(true);
    }

    public void ResumeGame()
    {
        Debug.Log("Game Resumed");
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseText != null)
            pauseText.SetActive(false);
    }

    public void QuitToMenu()
    {
        Debug.Log("Quit to Main Menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game...");
        Application.Quit();
    }
}
