using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class testdrag: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentTransform;
    public void OnBeginDrag(PointerEventData data)
    {
        Debug.Log("OnBeginDrag");
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        parentTransform = transform.parent;
        transform.SetParent(transform.parent.parent);

    }
    public void OnDrag(PointerEventData data)
    {
        transform.position = data.position;
    }
    public void OnEndDrag(PointerEventData data)
    {
        Debug.Log("OnEndDrag");
        transform.SetParent(parentTransform);
        transform.position = parentTransform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}