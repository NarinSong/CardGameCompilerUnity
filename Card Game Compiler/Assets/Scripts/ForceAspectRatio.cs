using UnityEngine;

public class ForceAspectRatio : MonoBehaviour
{
    void Start()
    {
        // Set the desired aspect ratio (16:9)
        float targetAspect = 16.0f / 9.0f;

        // Determine the current window aspect ratio
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // Current viewport scaling factor
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = GetComponent<Camera>();

        // If window is taller than our target aspect, use letterboxing
        if (scaleHeight < 1.0f)
        {  
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            
            camera.rect = rect;
        }
        else // If window is wider than our target aspect, use pillarboxing
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
