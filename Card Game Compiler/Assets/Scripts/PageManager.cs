using UnityEditor;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    public GameObject editorScene;
    public GameObject menuScene;
    public GameObject gameScene;
    public TabNavigation tN;
    public void setGame()
    {
        menuScene.SetActive(false);
        editorScene.SetActive(false);
        menuScene.GetComponent<Canvas>().sortingOrder = 0;
        editorScene.GetComponent<Canvas>().sortingOrder = 0;
        gameScene.GetComponent<Canvas>().sortingOrder = 1;
        tN.swapFields("game");
    }
    public void setEditor()
    {
        editorScene.SetActive(true);
        menuScene.SetActive(false);
        menuScene.GetComponent<Canvas>().sortingOrder = 0;
        editorScene.GetComponent<Canvas>().sortingOrder = 1;
        gameScene.GetComponent<Canvas>().sortingOrder = 0;
        tN.swapFields("editor");
    }
    public void setMain()
    {
        editorScene.SetActive(true);
        gameScene.SetActive(true);
        menuScene.SetActive(true);
        menuScene.GetComponent<Canvas>().sortingOrder = 1;
        editorScene.GetComponent<Canvas>().sortingOrder = 0;
        gameScene.GetComponent<Canvas>().sortingOrder = 0;
        tN.swapFields("postAuth");
    }
}
