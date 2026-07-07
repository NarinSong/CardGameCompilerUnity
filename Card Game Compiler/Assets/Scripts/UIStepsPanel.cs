using UnityEngine;
using UnityEngine.EventSystems;

/*
 * UIStepsPanel
 * Attach this to your Steps GameObject.
 * Also make sure Steps has an Image component with Raycast Target ON.
 *
 * This receives blocks dropped from the palette and handles
 * repositioning blocks that are already inside it.
 */
public class UIStepsPanel : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var x = eventData.pointerDrag?.GetComponent<UIDraggableBlock>();

        // Palette originals handle their own drop — ignore them here
        if (x == null || x.isPaletteItem) return;

        // Keep the block at exactly where it was dropped
        Vector3 dropPosition = x.transform.position;
        x.transform.SetParent(transform);
        x.transform.position = dropPosition;
        x.transform.localScale = Vector3.one;
    }
}
