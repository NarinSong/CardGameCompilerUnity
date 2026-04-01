using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dynamicButton : MonoBehaviour
{
    public string gameName;
    public int id;
    public Button button; 
    public TextMeshProUGUI buttonText;
    public websocketController websocket;

    public void Init(string gN, int gId, Vector3 pos)
    {
        Debug.Log("initializing button " + gN + gId);
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>(true);
        websocket = GameObject.Find("Websocketer").GetComponent<websocketController>();
        gameName = gN;
        id = gId;
        buttonText.text = gN;
        transform.position = pos;
    }

    void start()
    {
        
    }

    public void ClickTask()
    {
        websocket.EmitStartGame(id);
    }
}