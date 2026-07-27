using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public TMP_Text incUpText;
    public GameObject RangeIn;
    public TMP_Text incDownText;
    public TMP_Text numText;
    public int max;
    public int min;
    public int val;
    public void Init(int ownerX,vis visibilityX,string labelX,string displayNameX,string[] actionRolesX,rangeObj rangeX,loc local,ButtonType typeX)
    {
        owner = ownerX;
        visibility = visibilityX;
        range = rangeX;
        label = labelX;
        displayName = displayNameX;
        actionRoles = actionRolesX;
        type = typeX;
        val = 0;
        Debug.Log("drawng button at " + local.x + ", " + local.y);
        transform.position = new Vector3(local.x/100,local.y/100,0);
        displayText.text = displayName;
        WS = GameObject.Find("Websocketer").GetComponent<websocketController>();
        if(type == ButtonType.NUMBER)
        {
            //Debug.Log(range.ToString() + float.NaN);
            if(range.increment == float.NaN)
            {
                range.increment = 1;
            }
            if(range.max != float.NaN)
            {
                max = (int)range.max;
            }
            if(range.min != float.NaN)
            {
                min = (int)range.min;
            }
            if(max == int.MinValue)
            {
                max = int.MaxValue;
            }
            RangeIn.SetActive(true);
            incDownText.text = "-" + (int)range.increment;
            incUpText.text = "+" + (int)range.increment;
            val = 0;
            updateNumText();
        }
    }

    public void clickEvent()
    {
        if(type == ButtonType.NUMBER)
        {
            WS.EmitPlayerClickEvent(val,label);
        }
        else
        {
            WS.EmitPlayerClickEvent(0,label);
        }
    }

    public void incUp()
    {
        val += (int)range.increment;
        if(val > max)
        {
            val = max;
        }
        updateNumText();
    }

    public void incDown()
    {
        val -= (int)range.increment;
        if(val < min)
        {
            val = min;
        }
        updateNumText();
    }

    public void updateNumText()
    {
        numText.text = val.ToString();
    }
}
