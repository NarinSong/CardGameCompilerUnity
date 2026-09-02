using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameCardButton : MonoBehaviour
{
    public int gameID;  
    public websocketController wS;
    public Button edit;
    public Button delete;
    public TMP_Text gName;
    public TMP_Text gDesc;

    public void setID(int newID)
    {
        gameID = newID;
    }

    public void editEvent()
    {
        //fetch game code and edit blocks based on game id
    }

    public void deleteEvent()
    {
        wS.deleteGame(gameID);
        edit.interactable = false;
        delete.interactable = false;
        gName.text = "";
        gDesc.text = "";
    }
}
