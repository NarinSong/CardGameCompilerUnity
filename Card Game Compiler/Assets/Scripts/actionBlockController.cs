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
    public void swap()
    {
        if(drop.value == 0)
        {
            snappingPoint.SetActive(false);
            image.sprite = auto;
        }
        if(drop.value == 1)
        {
            snappingPoint.SetActive(true);
            image.sprite = trigger;
        }
    }
}
