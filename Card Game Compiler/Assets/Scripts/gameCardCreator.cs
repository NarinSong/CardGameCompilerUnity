using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameCardCreator : MonoBehaviour
{
    public List<myGameInfo> gameInfos;
    public float yOff;
    public GameObject gameCardPrefab;
    public Transform gameCardParent;
    public TMP_Text nameText;
    public TMP_Text descText;
    public Button edit;
    public Button delete;
    public RectTransform panel;
    public Scrollbar sb;
    public void drawCards(List<myGameInfo> x)
    {
        float vertAmt = 0;
        foreach(Transform child in gameCardParent) 
        {
            Destroy(child.gameObject);
        }
        gameInfos = x;
        Vector2 pos = new Vector2(0,0);
        if(gameInfos.Count > 10)
        {
            vertAmt = 810f + 40*(gameInfos.Count-10);
            panel.sizeDelta = new Vector2(510f, vertAmt);
            vertAmt = 0.5f*vertAmt;
        }
        else
        {
            vertAmt = 810f/2;
            panel.sizeDelta = new Vector2(510f, 810f);
        }
        gameCardParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,vertAmt-60);
        sb.value = 1;
        for(int i = 0; i < gameInfos.Count; i++)
        {
            GameObject holder = Instantiate(gameCardPrefab, gameCardParent);
            holder.GetComponent<gameCardController>().Init(gameInfos[i].name,gameInfos[i].description,gameInfos[i].id,nameText,descText,edit,delete,pos);
            pos += new Vector2(0,yOff);
        }
    }
}