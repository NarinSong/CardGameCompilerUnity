using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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
            SpawnDeleteButton();
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
        if (cloneScript != null) cloneScript.isPaletteItem = false;

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

        transform.SetParent(rootCanvas.transform);
        transform.SetAsLastSibling();

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
        UIDraggableBlock[] allBlocks = FindObjectsOfType<UIDraggableBlock>();
        UIDraggableBlock closest = null;
        float closestDist = snapDistance;

        foreach (var other in allBlocks)
        {
            if (other == this) continue;
            if (other.isPaletteItem) continue;
            if (other.transform.parent != transform.parent) continue;

            float otherHeight = other.rectTransform.rect.height;
            Vector2 snapPoint = other.rectTransform.anchoredPosition + new Vector2(0, -otherHeight);
            float dist = Vector2.Distance(rectTransform.anchoredPosition, snapPoint);

            if (dist < closestDist)
            {
                closestDist = dist;
                closest = other;
            }
        }

        if (closest != null)
        {
            rectTransform.anchoredPosition = closest.rectTransform.anchoredPosition
                + new Vector2(0, -closest.rectTransform.rect.height);
        }
    }

    // --- Delete button (only on placed blocks, not palette originals) ---

    void SpawnDeleteButton()
    {
        GameObject btn = new GameObject("DeleteBtn");
        btn.transform.SetParent(transform, false);

        RectTransform btnRect = btn.AddComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(1, 1);
        btnRect.anchorMax = new Vector2(1, 1);
        btnRect.pivot = new Vector2(1, 1);
        btnRect.sizeDelta = new Vector2(22, 22);
        btnRect.anchoredPosition = new Vector2(6, 6);

        var bg = btn.AddComponent<UnityEngine.UI.Image>();
        bg.color = new Color(0.85f, 0.15f, 0.15f, 1f);

        var button = btn.AddComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(() => Destroy(gameObject));

        GameObject label = new GameObject("Label");
        label.transform.SetParent(btn.transform, false);

        RectTransform labelRect = label.AddComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.sizeDelta = Vector2.zero;
        labelRect.anchoredPosition = Vector2.zero;

        var text = label.AddComponent<TMPro.TextMeshProUGUI>();
        text.text = "X";
        text.fontSize = 13;
        text.fontStyle = TMPro.FontStyles.Bold;
        text.alignment = TMPro.TextAlignmentOptions.Center;
        text.color = Color.white;
        text.raycastTarget = false;
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
}
