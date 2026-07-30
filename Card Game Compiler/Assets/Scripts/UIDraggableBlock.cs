using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;
using System.Linq;
using Unity.VisualScripting;

/*
 * UIDraggableBlock
 * Attach this to every image block in the palette.
 *
 * How it works:
 *   - Palette blocks (isPaletteItem = true) never move. Dragging one creates
 *     a copy that you drop into the Steps area. The original stays so you can
 *     drag as many copies as you want.
 *   - Once a copy lands in Steps it gets a red X button in the corner to delete it.
 *   - You can drag blocks already in Steps to reposition them.
 *   - Drop a block close to another one and it snaps below it.
 */
public class UIDraggableBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Settings")]
    public string actionName = "Action";
    public bool isPaletteItem = true;
    public float snapDistance = 60f;

    private Canvas rootCanvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    // Used when repositioning a block already in Steps
    private Transform originalParent;
    private int originalSiblingIndex;
    private Vector3 originalPosition;

    // The copy we create when dragging from the palette
    private GameObject activeClone;
    private RectTransform cloneRect;
    public Vector2 blockOffset;
    public List<SnappablePart> snappedTo;
    public GameObject xButton;
    public SnappablePart[] myParts;
    public bool actionBlock;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        rootCanvas = GetComponentInParent<Canvas>();
        if (rootCanvas != null && !rootCanvas.isRootCanvas)
            rootCanvas = rootCanvas.rootCanvas;
    }

    void Start()
    {
        if (!isPaletteItem)
            xButton.SetActive(true);
    }

    // --- Drag events ---

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPaletteItem)
            BeginPaletteDrag();
        else
            BeginStepsDrag();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 move = eventData.delta / rootCanvas.scaleFactor;

        if (isPaletteItem && cloneRect != null)
            cloneRect.anchoredPosition += move;
        else
            rectTransform.anchoredPosition += move;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isPaletteItem)
            EndPaletteDrag(eventData);
        else
            EndStepsDrag();
    }

    // --- Palette drag: create a copy, drag the copy ---

    void BeginPaletteDrag()
    {
        activeClone = Instantiate(gameObject, rootCanvas.transform);
        activeClone.transform.position = transform.position;
        activeClone.transform.localScale = transform.localScale;

        var cloneScript = activeClone.GetComponent<UIDraggableBlock>();
        if (cloneScript != null) 
        {
            cloneScript.isPaletteItem = false;
            cloneScript.xButton.SetActive(true);
            //Debug.Log("setting snapping points to not palette items");
            SnappablePart[] myPartsTemp = cloneScript.myParts;
            foreach(SnappablePart x in myPartsTemp)
            {
                x.GetComponent<SnappablePart>().setPaletteItemFalse();
                x.GetComponent<SnappablePart>().setIsOn(true);
            }
        }
        cloneRect = activeClone.GetComponent<RectTransform>();

        var cg = activeClone.GetComponent<CanvasGroup>();
        if (cg == null) cg = activeClone.AddComponent<CanvasGroup>();
        cg.alpha = 0.75f;
        cg.blocksRaycasts = false;

        canvasGroup.blocksRaycasts = false;
    }

    void EndPaletteDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (activeClone == null) return;

        UIStepsPanel steps = FindStepsPanelUnderPointer(eventData);

        if (steps != null)
        {
            PlaceCloneInSteps(steps);
        }
        else
        {
            Destroy(activeClone);
            activeClone = null;
        }
    }

    void PlaceCloneInSteps(UIStepsPanel steps)
    {
        Vector3 dropPosition = activeClone.transform.position;

        activeClone.transform.SetParent(steps.transform);
        activeClone.transform.position = dropPosition;
        activeClone.transform.localScale = Vector3.one;

        var cg = activeClone.GetComponent<CanvasGroup>();
        if (cg != null) { cg.alpha = 1f; cg.blocksRaycasts = true; }

        var cloneScript = activeClone.GetComponent<UIDraggableBlock>();
        if (cloneScript != null) cloneScript.TrySnapToNearbyBlock();

        activeClone = null;
    }

    // --- Steps drag: move the block itself ---

    void BeginStepsDrag()
    {
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
        originalPosition = rectTransform.position;
        List<SnappablePart> toRemove = new List<SnappablePart>();

        transform.SetParent(rootCanvas.transform);
        transform.SetAsLastSibling();
        foreach(SnappablePart x in myParts)
        {
            if(x.name == "Nested Snapping Point") continue;
            x.GetComponent<SnappablePart>().setIsOn(true);
        }
        foreach(SnappablePart x in snappedTo)
        {
            if(x.transform.IsChildOf(this.transform)) continue;
            foreach(SnappablePart y in myParts)
            {
                if(y.name == "Top Snapping Point")
                {
                    x.parent.GetComponent<UIDraggableBlock>().snappedTo.Remove(y);
                    toRemove.Add(x);
                }
            }
            if(x.name == "Bottom Snapping Point")
            {
                if(x.logicPad != null)
                {
                    x.logicPad.GetComponent<RectTransform>().anchoredPosition += new Vector2(0,71.3071f*findLogicPadAmt());
                    foreach(SnappablePart pt in this.GetComponentsInChildren<SnappablePart>())
                    {
                        pt.logicPad = null;
                    }
                }
            }
            x.GetComponent<SnappablePart>().setIsOn(true);
            if(x.name == "Nested Snapping Point")
            {
                if(x.logicPad != null)
                {
                    x.logicPad.GetComponent<RectTransform>().anchoredPosition += new Vector2(0,71.3071f*findLogicPadAmt());
                    foreach(SnappablePart pt in this.GetComponentsInChildren<SnappablePart>())
                    {
                        pt.logicPad = null;
                    }
                }
                Transform TargetBound = x.transform.parent.Find("Bound");
                if(TargetBound != null)
                {
                    TargetBound.GetComponent<RectTransform>().anchoredPosition -= blockOffset - new Vector2(81,0);
                }
                Transform recursiveBound = x.parent.transform;
                recursiveBound.GetComponent<UIDraggableBlock>().blockOffset -= blockOffset - new Vector2(81,0);
                if(recursiveBound.parent.gameObject.name == "Top Snapping Point" || recursiveBound.parent.gameObject.name == "Bottom Snapping Point")
                {
                    recursiveBound = null;
                }
                while(recursiveBound != null)
                {
                    recursiveBound = recursiveBound.parent.parent.Find("Bound");
                    if(recursiveBound != null)
                    {
                        Debug.Log("moving bound " + TargetBound.name);
                        recursiveBound.GetComponent<RectTransform>().anchoredPosition -= blockOffset - new Vector2(81,0);
                        SnappablePart temp;
                        Transform tempTrans = recursiveBound.parent.Find("Nested Snapping Point");
                        if(tempTrans != null)
                        {
                            if(tempTrans.TryGetComponent<SnappablePart>(out temp))
                            {
                                recursiveBound = temp.parent.transform;
                                recursiveBound.GetComponent<UIDraggableBlock>().blockOffset -= blockOffset - new Vector2(81,0);
                                if(recursiveBound.parent.gameObject.name == "Top Snapping Point" || recursiveBound.parent.gameObject.name == "Bottom Snapping Point")
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
        snappedTo = snappedTo.Except(toRemove).ToList();
        canvasGroup.alpha = 0.75f;
        canvasGroup.blocksRaycasts = false;
    }

    void EndStepsDrag()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        bool droppedOutsideSteps = transform.parent == rootCanvas.transform;

        if (droppedOutsideSteps)
        {
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalSiblingIndex);
            rectTransform.position = originalPosition;
        }
        else
        {
            TrySnapToNearbyBlock();
        }
    }

    // --- Snapping ---

    public void TrySnapToNearbyBlock()
    {
        SnappablePart[] allParts = Object.FindObjectsByType<SnappablePart>(FindObjectsSortMode.None);
        SnappablePart bestMyPart = null;
        SnappablePart bestTargetPart = null;
        List<SnappablePart> toRemove = new List<SnappablePart>();
        float closestDistance = snapDistance;
        foreach (SnappablePart myPart in myParts)
        {
            foreach (SnappablePart targetPart in allParts)
            {
                if(targetPart.GetComponent<SnappablePart>().isPaletteItem) continue;
                if(!targetPart.GetComponent<SnappablePart>().isOn) continue;
                if(myPart.name == targetPart.name) continue;
                if(myPart.name == "Bottom Snapping Point" && targetPart.name == "Nested Snapping Point") continue;
                if(myPart.GetComponent<SnappablePart>().notNestable && targetPart.name == "Nested Snapping Point") continue;
                if(!myPart.GetComponent<SnappablePart>().var && targetPart.GetComponent<SnappablePart>().varOnly) continue;
                if(myPart.name == "Nested Snapping Point") continue;
                if(targetPart.transform.IsChildOf(this.transform)) continue;
                if(targetPart.name == "Top Snapping Point") continue;

                float distance = Vector3.Distance(myPart.transform.position, targetPart.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestMyPart = myPart;
                    bestTargetPart = targetPart;
                }
            }
        }
        if (bestMyPart != null && bestTargetPart != null)
        {
            Vector3 offset = bestTargetPart.transform.position - bestMyPart.transform.position;
            this.transform.position += offset;
            bestMyPart.setIsOn(false);
            bestTargetPart.setIsOn(false);
            snappedTo.Add(bestTargetPart);
            bestTargetPart.parent.GetComponent<UIDraggableBlock>().snappedTo.Add(bestMyPart);
            bestMyPart.transform.parent.SetParent(bestTargetPart.transform,true);
            if(bestTargetPart.name == "Bottom Snapping Point")
            {
                bestTargetPart.padAmt += 1;
                if(bestTargetPart.logicPad != null)
                {
                    bestTargetPart.logicPad.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0,71.3071f*findLogicPadAmt());
                    foreach(SnappablePart part in myParts)
                    {
                        if(part.name == "Bottom Snapping Point")
                        {
                            part.logicPad = bestTargetPart.logicPad;
                        }
                    }
                }
            }
            if(bestTargetPart.name == "Nested Snapping Point")
            {
                foreach(SnappablePart part in myParts)
                {
                    if(part.name == "Top Snapping Point" || part.name == "Bottom Snapping Point")
                    {
                        if(bestTargetPart.logical && part.name == "Bottom Snapping Point")
                        {
                            part.logicPad = bestTargetPart.logicPad;
                            part.logicPad.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0,71.3071f*(findLogicPadAmt()+1));
                            continue;
                        }
                        part.setIsOn(false);
                    }
                }
                foreach(SnappablePart part in snappedTo)
                {
                    List<SnappablePart> toRemove2 = new List<SnappablePart>();
                    foreach(SnappablePart partInc in part.parent.GetComponent<UIDraggableBlock>().snappedTo)
                    {
                        if(part.name == "Top Snapping Point" && partInc.name == "Bottom Snapping Point")
                        {
                            if(bestTargetPart.logical) continue;
                            part.parent.transform.SetParent(originalParent);
                            foreach(SnappablePart partMe in myParts)
                            {
                                toRemove2.Add(partMe);
                            }
                            part.isOn = true;
                            toRemove.Add(part);
                        }
                    }
                    part.parent.GetComponent<UIDraggableBlock>().snappedTo = part.parent.GetComponent<UIDraggableBlock>().snappedTo.Except(toRemove2).ToList();
                }
                Transform TargetBound = bestTargetPart.transform.parent.Find("Bound");
                //Debug.Log(TargetBound);
                if(TargetBound != null)
                {
                    TargetBound.GetComponent<RectTransform>().anchoredPosition += blockOffset - new Vector2(81,0);
                }
                bestTargetPart.parent.GetComponent<UIDraggableBlock>().blockOffset += blockOffset - new Vector2(81,0);
                Transform recursiveBound = bestTargetPart.parent.transform;
                if(recursiveBound.parent.gameObject.name == "Top Snapping Point" || recursiveBound.parent.gameObject.name == "Bottom Snapping Point")
                {
                    recursiveBound = null;
                }
                while(recursiveBound != null)
                {
                    recursiveBound = recursiveBound.parent.parent.Find("Bound");
                    if(recursiveBound != null)
                    {
                        recursiveBound.GetComponent<RectTransform>().anchoredPosition += blockOffset - new Vector2(81,0);
                        SnappablePart temp = null;
                        Transform tempTrans = recursiveBound.parent.Find("Nested Snapping Point");
                        if(tempTrans != null)
                        {
                            if(tempTrans.TryGetComponent<SnappablePart>(out temp))
                            {
                                recursiveBound = temp.parent.transform;
                                recursiveBound.GetComponent<UIDraggableBlock>().blockOffset += blockOffset - new Vector2(81,0);
                                if(recursiveBound.parent.gameObject.name == "Top Snapping Point" || recursiveBound.parent.gameObject.name == "Bottom Snapping Point")
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("Exiting at " + recursiveBound.gameObject.name);
                            break;
                        }
                    }
                }
            }
        }
        snappedTo = snappedTo.Except(toRemove).ToList();
    }

    public void undoSnapPadding()
    {
        foreach(SnappablePart x in snappedTo)
        {
            x.GetComponent<SnappablePart>().setIsOn(true);
            foreach(SnappablePart y in myParts)
            {
                x.parent.GetComponent<UIDraggableBlock>().snappedTo.Remove(y);
            }
            if(x.name == "Bottom Snapping Point")
            {
                if(x.logicPad != null)
                {
                    x.logicPad.GetComponent<RectTransform>().anchoredPosition += new Vector2(0,71.3071f*findLogicPadAmt());
                }
            }
            if(x.name == "Nested Snapping Point")
            {
                Transform TargetBound = x.transform.parent.Find("Bound");
                if(x.logicPad != null)
                {
                    x.logicPad.GetComponent<RectTransform>().anchoredPosition += new Vector2(0,71.3071f*(findLogicPadAmt()+1));
                }
                //Debug.Log(TargetBound);
                if(TargetBound != null)
                {
                    TargetBound.GetComponent<RectTransform>().anchoredPosition -= blockOffset - new Vector2(81,0);
                }
                Transform recursiveBound = x.parent.transform;
                recursiveBound.GetComponent<UIDraggableBlock>().blockOffset -= blockOffset - new Vector2(81,0);
                if(recursiveBound.parent.gameObject.name == "Top Snapping Point" || recursiveBound.parent.gameObject.name == "Bottom Snapping Point")
                {
                    recursiveBound = null;
                }
                while(recursiveBound != null)
                {
                    recursiveBound = recursiveBound.parent.parent.Find("Bound");
                    if(recursiveBound != null)
                    {
                        Debug.Log("moving bound " + TargetBound.name);
                        recursiveBound.GetComponent<RectTransform>().anchoredPosition -= blockOffset - new Vector2(81,0);
                        SnappablePart temp;
                        Transform tempTrans = recursiveBound.parent.Find("Nested Snapping Point");
                        if(tempTrans != null)
                        {
                            if(tempTrans.TryGetComponent<SnappablePart>(out temp))
                            {
                                recursiveBound = temp.parent.transform;
                                recursiveBound.GetComponent<UIDraggableBlock>().blockOffset -= blockOffset - new Vector2(81,0);
                                if(recursiveBound.parent.gameObject.name == "Top Snapping Point" || recursiveBound.parent.gameObject.name == "Bottom Snapping Point")
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
        Destroy(gameObject);
    }
    // --- Helpers ---

    UIStepsPanel FindStepsPanelUnderPointer(PointerEventData eventData)
    {
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);

        foreach (var hit in hits)
        {
            var steps = hit.gameObject.GetComponentInParent<UIStepsPanel>();
            if (steps != null) return steps;
        }
        return null;
    }

    public int findLogicPadAmt()
    {
        UIDraggableBlock[] blockArr = this.GetComponentsInChildren<UIDraggableBlock>();
        int count = 0;
        foreach(UIDraggableBlock block in blockArr)
        {
            if(block.transform.parent.name == "Nested Snapping Point")
            {
                continue;
            }
            else
            {
                count ++;
            }
        }
        return count;
    }

    public void evalutate()
    {
        foreach(SnappablePart part in myParts)
        {
            if(part.name == "Nested Snapping Point")
            {
                //add checks for nested snapping value for final assignment
                UIDraggableBlock temp = part.transform.GetComponentInChildren<UIDraggableBlock>();
                temp.evalutate();
            }
            if(part.name == "Bottom Snapping Point")
            {
                UIDraggableBlock temp = part.transform.GetComponentInChildren<UIDraggableBlock>();
                temp.evalutate();
            }
        }
    }
}
