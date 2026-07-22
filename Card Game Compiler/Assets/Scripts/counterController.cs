using TMPro;
using UnityEngine;

public class counterController : MonoBehaviour
{
    public int owner;
    public vis visibility;
    public int value;
    public string label;
    public string displayName;
    public string[] actionRoles;
    public TMP_Text displayText;
    public websocketController WS;
    public void Init(int ownerP, vis visP, int valueP, string labelP, string displayNameP, string[] actionRolesP, loc local)
    {
        owner = ownerP;
        visibility = visP;
        value = valueP;
        label = labelP;
        displayName = displayNameP;
        actionRoles = actionRolesP;
        Debug.Log("drawng counter at " + local.x/100 + ", " + local.y/100);
        transform.position = new Vector3(local.x/100,local.y/100,0);
        displayText.text = valueP.ToString();
        WS = GameObject.Find("Websocketer").GetComponent<websocketController>();
    }

    public void clickEvent()
    {
        WS.EmitPlayerClickEvent(0,label);
    }
}
