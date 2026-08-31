using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gameCardController : MonoBehaviour
{
    public string gameName;
    public string gameDesc;
    public int gameID;
    
    public void Init(string gN, string gD, int gID)
    {
        gameName = gN;
        gameDesc = gN;
        gameID = gID;
    }
}
