using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    public Transform parentToReturnTo = null;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("BeginDragged");

        if (!this.transform.parent.name.StartsWith("Canvas"))
        {
            // Debug.Log(this.transform.parent.name);
            parentToReturnTo = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent);
            
        }
        GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("EndDragged");

        
        if (parentToReturnTo != null)
        {
            this.transform.SetParent(parentToReturnTo);
        }
        

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

}
