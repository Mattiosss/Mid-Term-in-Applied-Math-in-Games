using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Play button clicked!");
        SceneManager.LoadScene("GameplayScene"); 
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked!");
        Application.Quit();
    }
}
