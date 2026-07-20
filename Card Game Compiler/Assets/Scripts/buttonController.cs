using TMPro;
using UnityEngine;

public class buttonController : MonoBehaviour
{
    public int owner;
    public string label;
    public string displayName;
    public vis visibility;
    public string[] actionRoles;
    public loc location;
    public rangeObj range;
    public ButtonType type;
    public TMP_Text displayText;
    public websocketController WS;
    public void Init(int ownerX,vis visibilityX,string labelX,string displayNameX,string[] actionRolesX,rangeObj rangeX,loc local)
    {
        owner = ownerX;
        visibility = visibilityX;
        range = rangeX;
        label = labelX;
        displayName = displayNameX;
        actionRoles = actionRolesX;
        transform.position = new Vector3(local.x,local.y,0);
        WS = GameObject.Find("Websocketer").GetComponent<websocketController>();
        if(type == ButtonType.CLICK)
        {
            displayText.text = label;
        }
        else
        {
            if(range.increment > 0)
            {
                displayText.text = "+ " + range.increment.ToString();
            }
            else
            {
                displayText.text = range.increment.ToString();
            }
        }
    }

    public void clickEvent()
    {
        WS.EmitPlayerClickEvent(0,label);
    }
}
