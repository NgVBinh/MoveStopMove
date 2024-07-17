using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutlineController : MonoBehaviour,IPointerDownHandler
{
    private static OutlineController currentSelectedImage;

    private Outline outline;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Disable the outline of the previously selected image if there is one
        if (currentSelectedImage != null && currentSelectedImage != this)
        {
            currentSelectedImage.ToggleOutline(false);
        }

        // Enable the outline for the newly selected image
        ToggleOutline(true);
        currentSelectedImage = this;
    }

    private void ToggleOutline(bool enable)
    {
        outline = GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = enable;
        }
    }

    public void DisplayOutlineFirstEqup()
    {
        if (currentSelectedImage != null && currentSelectedImage != this)
        {
            currentSelectedImage.ToggleOutline(false);
        }

        // Enable the outline for the newly selected image
        ToggleOutline(true);
        currentSelectedImage = this;
    }
}
