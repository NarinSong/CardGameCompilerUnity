using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gameCardController : MonoBehaviour
{
    public string gameName;
    public string gameDesc;
    public int gameID;
    public TMP_Text nameText;
    public TMP_Text descText;
    public Button editButton;
    public Button deleteButton;
    public TMP_Text selfText;
    public GameObject background;

    public void Init(string gN, string gD, int gID, TMP_Text nText, TMP_Text dText,Button edit, Button delete, Vector2 pos)
    {
        gameName = gN;
        gameDesc = gN;
        gameID = gID;
        nameText = nText;
        descText = dText;
        editButton = edit;
        deleteButton = delete;
        selfText.text = gN;
        Image x = background.GetComponent<Image>();
        x.color = new Color(Random.Range(0.4f,1f),Random.Range(0.4f,1f),Random.Range(0.4f,1f));
        GetComponent<RectTransform>().anchoredPosition = pos;
    }

    public void clickEvent()
    {
        nameText.text = gameName;
        descText.text = gameDesc;
        editButton.GetComponent<gameCardButton>().setID(gameID);
        deleteButton.GetComponent<gameCardButton>().setID(gameID);
        editButton.interactable = true;
        deleteButton.interactable = true;
    }
}
