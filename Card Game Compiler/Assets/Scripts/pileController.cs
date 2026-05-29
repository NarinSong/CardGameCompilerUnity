using UnityEngine;

public class pileController : MonoBehaviour
{
    public int owner;
    public vis visibility;
    public card[] cards;
    public string label;
    public string displayName;
    public string[] actionRoles;
    public GameObject ObjectCards;
    public websocketController WS;
    public CardController CC;
    public void Init(int ownerP, vis visP, card[] cardsP, string labelP, string displayNameP, string[] actionRolesP)
    {
        owner = ownerP;
        visibility = visP;
        cards = cardsP;
        label = labelP;
        displayName = displayNameP;
        actionRoles = actionRolesP;
        ObjectCards = transform.GetChild(0).gameObject;
        WS = GameObject.Find("Websocketer").GetComponent<websocketController>();
        if(visibility == vis.FACE_UP)
        {
            CC.updateCard(cards[0].rank,cards[0].suit,false);
        }
        if(visibility == vis.FACE_DOWN)
        {
            CC.updateCard(0,0,true);
        }
    }

    public void clickEvent()
    {
        Debug.Log("Pile " + label + " clicked");
        if(visibility == vis.FACE_UP)
        {
            WS.EmitPlayerClickEvent(cards[0].id,label);
        }
        if(visibility == vis.FACE_DOWN)
        {
            WS.EmitPlayerClickEvent(0,label);
        }
    }
}
