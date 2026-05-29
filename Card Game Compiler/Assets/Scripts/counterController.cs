using UnityEngine;

public class counterController : MonoBehaviour
{
    public int owner;
    public vis visibility;
    public int value;
    public string label;
    public string displayName;
    public string[] actionRoles;
    public void Init(int ownerP, vis visP, int valueP, string labelP, string displayNameP, string[] actionRolesP)
    {
        owner = ownerP;
        visibility = visP;
        value = valueP;
        label = labelP;
        displayName = displayNameP;
        actionRoles = actionRolesP;
    }

    

    public void clickEvent()
    {
        //do Counters need click events?
    }
}
