using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class arrayController : MonoBehaviour
{
    public GameObject boundPrefab;
    public GameObject snappingPointPrefab;
    public List<GameObject> pointSnappers;
    public List<GameObject> boundSnappers;
    public List<GameObject> pointArray;
    public List<GameObject> boundArray;
    public bool isOn = true;
    public UIDraggableBlock bC;

    void Start()
    {

    }

    public void addItem()
    {
        if(isOn)
        {
            GameObject b = Instantiate(boundPrefab, boundSnappers[boundSnappers.Count-1].transform.position, Quaternion.identity, boundArray[boundArray.Count-1].transform);
            b.name = "Bound";
            GameObject s = Instantiate(snappingPointPrefab, pointSnappers[pointSnappers.Count-1].transform.position, Quaternion.identity, boundArray[boundArray.Count-1].transform);
            s.name = "Nested Snapping Point";
            this.GetComponent<UIDraggableBlock>().myParts.Insert(0,s.GetComponent<SnappablePart>());
            s.GetComponent<SnappablePart>().Init(pointSnappers.Count,this.gameObject);
            pointSnappers.Add(b.transform.Find("SnapAligner").gameObject);
            boundSnappers.Add(b.transform.Find("BoundAligner").gameObject);
            boundArray.Add(b);
            pointArray.Add(s);
            changeOffset();
        }
    }

    public void removeItem()
    {
        if(isOn && boundArray.Count > 1)
        {
            GameObject bound = boundArray[boundArray.Count-1];
            GameObject point = pointArray[pointArray.Count-1];
            this.GetComponent<UIDraggableBlock>().myParts.RemoveAt(0);
            pointSnappers.RemoveAt(pointSnappers.Count-1);
            boundSnappers.RemoveAt(boundSnappers.Count-1);
            pointArray.RemoveAt(pointArray.Count-1);
            boundArray.RemoveAt(boundArray.Count-1);
            if(point.transform.GetComponentInChildren<UIDraggableBlock>() != null)
            {
                point.transform.GetComponentInChildren<UIDraggableBlock>().undoSnapPadding();
            }
            Destroy(bound);
            Destroy(point);
            changeOffset();
        }
    }

    public void changeOffset()
    {
        bC.blockOffset = new Vector2(260+pointSnappers.Count*125.2f,0);
    }
}
