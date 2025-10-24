using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("The name of the gameplay scene to load when Play is clicked.")]
    public string gameSceneName = "GameScene"; 

    public void PlayGame()
    {
        Debug.Log("Play button clicked!"); 
        SceneManager.LoadScene(gameSceneName); 
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game"); 
        Application.Quit();
    }
}

