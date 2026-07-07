using UnityEngine;

public class counterController : MonoBehaviour
{
    public int owner;
    public vis visibility;
    public int value;
    public string label;
    public string displayName;
    public string[] actionRoles;
    public websocketController WS;
    public void Init(int ownerP, vis visP, int valueP, string labelP, string displayNameP, string[] actionRolesP, loc local)
    {
        owner = ownerP;
        visibility = visP;
        value = valueP;
        label = labelP;
        displayName = displayNameP;
        actionRoles = actionRolesP;
        transform.position = new Vector3(local.x,local.y,0);
        WS = GameObject.Find("Websocketer").GetComponent<websocketController>();
    }

    //TODO counter prefab so counters can be drawn

    public void clickEvent()
    {
        WS.EmitPlayerClickEvent(0,label);
    }
}
