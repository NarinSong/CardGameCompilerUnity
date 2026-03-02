using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeScene(string targetScene)
    {
        SceneManager.LoadScene(targetScene);
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
