using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    public ScrollRect scrollRect;

    private void Awake()
    {
        scrollRect = transform.parent.parent.parent.GetComponent<ScrollRect>();
    }
    public void OnBeginDrag(PointerEventData e)
    {
        scrollRect.OnBeginDrag(e);
    }

    public void OnDrag(PointerEventData e)
    {
        scrollRect.OnDrag(e);
    }

    public void OnEndDrag(PointerEventData e)
    {
        scrollRect.OnEndDrag(e);
    }

    public void OnScroll(PointerEventData e)
    {
        scrollRect.OnScroll(e);
    }
}
