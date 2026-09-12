using SocketIOClient;
using UnityEngine;

public class editorRebuilder : MonoBehaviour
{
    public variableController vC;
    public websocketController wS;
    public editorController eD;
    public UIDraggableBlock currentBlock;
    public UIDraggableBlock prevBlock;
    public actionBlockController currentAction;

    public void parseGameJSON(SocketIOResponse game)
    {
        //HOW DO I DO THIS?????????????????????????????????? yeah no clue still xd
    }
}
