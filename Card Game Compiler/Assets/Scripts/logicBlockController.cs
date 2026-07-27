using TMPro;
using UnityEngine;

public class logicBlockController : MonoBehaviour
{
    public TMP_Dropdown drop;
    public GameObject bottomSnappingPoint;
    public GameObject bottomSnappingPointAligner;
    public GameObject elseSnappingPoint;
    public GameObject elseSect;
    public GameObject place1;
    public GameObject place2;
    public bool isIf;
    public GameObject thing;

    void Update()
    {
        bottomSnappingPoint.transform.position = bottomSnappingPointAligner.transform.position;
    }

    public void swap()
    {
        if(drop.value == 0)
        {
            bottomSnappingPointAligner = place1;
            elseSect.SetActive(false);
            elseSnappingPoint.SetActive(false);
            thing.SetActive(false);
        }
        if(drop.value == 1)
        {
            elseSect.SetActive(true);
            elseSnappingPoint.SetActive(true);
            thing.SetActive(true);
            bottomSnappingPointAligner = place2;
        }
    }
}
