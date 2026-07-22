using UnityEngine;
public class pileController : MonoBehaviour
{
    public int owner;
    public vis visibility;
    public card[] cards;
    public string label;
    public string displayName;
    public string[] actionRoles;
    public loc location;
    public GameObject ObjectCards;
    public websocketController WS;
    public CardController CC;
    public void Init(int ownerP, vis visP, card[] cardsP, loc local, string labelP, string displayNameP, string[] actionRolesP)
    {
        owner = ownerP;
        visibility = visP;
        cards = cardsP;
        location = local;
        label = labelP;
        displayName = displayNameP;
        actionRoles = actionRolesP;
        Debug.Log("Drawing pile at " + local.x + ", " + local.y + ", " + 0);
        transform.position = new Vector3(local.x/100,local.y/100,0);
        ObjectCards = transform.GetChild(0).gameObject;
        WS = GameObject.Find("Websocketer").GetComponent<websocketController>();
        if(visibility == vis.FACE_UP)
        {
            if(!isEmpty())
            {
                CC.updateCard(cards[0].rank,cards[0].suit,false,isEmpty());
            }
            else
            {
                CC.updateCard(0,0,true,isEmpty());
            }
        }
        if(visibility == vis.FACE_DOWN)
        {
            CC.updateCard(0,0,true,isEmpty());
        }
    }

    public void clickEvent()
    {
        Debug.Log("Pile " + label + " clicked");
        if(visibility == vis.FACE_UP)
        {
            if(isEmpty())
            {
                WS.EmitPlayerClickEvent(0,label);
            }
            else
            {
                WS.EmitPlayerClickEvent(cards[0].id,label);
            }
        }
        if(visibility == vis.FACE_DOWN)
        {
            WS.EmitPlayerClickEvent(0,label);
        }
    }

    public bool isEmpty()
    {
        if(cards.Length == 0)
        {
            return true;
        }
        return false;
    }
}
