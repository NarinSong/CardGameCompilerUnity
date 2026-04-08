using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void changeScene(string targetScene)
    {
        SceneManager.LoadScene(targetScene);
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
