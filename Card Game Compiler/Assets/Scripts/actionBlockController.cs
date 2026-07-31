using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class actionBlockController : MonoBehaviour
{
    public Sprite auto;
    public Sprite trigger;
    public TMP_Dropdown drop;
    public GameObject snappingPoint;
    public Image image;
    public bool type;
    public void swap()
    {
        if(drop.value == 0)
        {
            snappingPoint.SetActive(false);
            image.sprite = auto;
            type = false;
        }
        if(drop.value == 1)
        {
            snappingPoint.SetActive(true);
            image.sprite = trigger;
            type = true;
        }
    }

    public string returnType()
    {
        if(type == false)
        {
            return "AUTO";
        }
        else
        {
            return "CLICK";
        }
    }
}
