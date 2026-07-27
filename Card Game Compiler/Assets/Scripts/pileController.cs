using Unity.VisualScripting;
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
    public websocketController WS;
    public CardController CC;
    public GameObject pilePrefab;
    public Transform pileParent;
    public void Init(int ownerP, vis visP, card[] cardsP, loc local, string labelP, string displayNameP, string[] actionRolesP)
    {
        owner = ownerP;
        visibility = visP;
        cards = cardsP;
        location = local;
        label = labelP;
        displayName = displayNameP;
        actionRoles = actionRolesP;
        Debug.Log("Called Init");
        transform.position = new Vector3(local.x/100,local.y/100,0);
        WS = GameObject.Find("Websocketer").GetComponent<websocketController>();
        pileParent = transform.parent;
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
        else if(visibility == vis.FACE_UP_SPREAD)
        {
            if(!isEmpty())
            {
                CC.updateCard(cards[0].rank,cards[0].suit,false,isEmpty());
            }
            else
            {
                CC.updateCard(0,0,true,isEmpty());
            }
            for(int i = 1; i < cards.Length; i++)
            {
                GameObject x = Instantiate(pilePrefab, new Vector3(0,0,0), pileParent.rotation, pileParent);
                pileController PC = x.GetComponent<pileController>();
                card[] temp = new card[]{cards[i]};
                loc mLocation = location;
                mLocation.x += 120f;
                PC.InitRecursive(owner, visibility, temp,mLocation, label, displayName, actionRoles);
            }
        }
        else if(visibility == vis.FACE_DOWN)
        {
            CC.updateCard(0,0,true,isEmpty());
        }
        else if(visibility == vis.FACE_DOWN_SPREAD)
        {
            CC.updateCard(0,0,true,isEmpty());
            for(int i = 1; i < cards.Length; i++)
            {
                GameObject x = Instantiate(pilePrefab, new Vector3(0,0,0), pileParent.rotation, pileParent);
                pileController PC = x.GetComponent<pileController>();
                card[] temp = new card[]{cards[i]};
                loc mLocation = location;
                mLocation.x += 120f;
                PC.InitRecursive(owner, visibility, temp,mLocation, label, displayName, actionRoles);
            }
        }
    }

    public void InitRecursive(int ownerP, vis visP, card[] cardsP, loc local, string labelP, string displayNameP, string[] actionRolesP)
    {
        Debug.Log("calling init recursive");
        owner = ownerP;
        visibility = visP;
        cards = cardsP;
        location = local;
        label = labelP;
        displayName = displayNameP;
        actionRoles = actionRolesP;
        //Debug.Log("Drawing pile at " + local.x + ", " + local.y + ", " + 0);
        transform.position = new Vector3(local.x/100,local.y/100,0);
        WS = GameObject.Find("Websocketer").GetComponent<websocketController>();
        if(visibility == vis.FACE_UP_SPREAD)
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
        else if(visibility == vis.FACE_DOWN_SPREAD)
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
        else if(visibility == vis.FACE_UP_SPREAD)
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