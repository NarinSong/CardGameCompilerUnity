using Unity.VisualScripting;
using UnityEngine;

public class SnappablePart : MonoBehaviour
{
    public bool isPaletteItem;
    public bool isOn;
    public RectTransform rectTransform;
    public string technical;
    public bool notNestable;
    public bool varOnly;
    public bool var;
    public GameObject parent;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        technical = name + transform.parent.gameObject.name;
    }

    public void setPaletteItemFalse()
    {
        //Debug.Log("set " + technical + " to false");
        isPaletteItem = false;
    }

    public void setIsOn(bool set)
    {
        //Debug.Log("set ON " + technical + " to " + set);
        isOn = set;
    }
}
