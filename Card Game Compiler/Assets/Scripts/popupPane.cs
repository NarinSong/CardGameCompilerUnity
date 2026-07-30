using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class popupPane : MonoBehaviour
{
    public int msgID;
    public TMP_Text tb;
    public RectTransform pane;
    public Scrollbar sb;

    public void startPanel(string text)
    {
        tb.text = tb.text + "\n" + "[" + msgID + "] " + text;
        msgID += 1;
        if(msgID > 112)
        {
            pane.sizeDelta = new Vector2(414,(tb.textInfo.lineCount-12)*28.57f+400);
            sb.value = 0;
        }
    }

    public void ResetPanel()
    {
        tb.text = "";
        msgID = 100;
        pane.sizeDelta = new Vector2(414,400);
    }

    public void HidePanel()
    {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(430,0);
    }

    public void UnhidePanel()
    {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(430,400);
        sb.value = 0;
    }
}
