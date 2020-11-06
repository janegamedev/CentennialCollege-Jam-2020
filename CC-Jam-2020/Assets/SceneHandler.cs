using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public string gameScene;
    
    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(gameScene);
    }
    
    public void Exit()
    {
        Application.Quit();
    }
}
