using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    public Transform parentToReturnTo = null;
    public enum Slot {Variable, Value, All};
    public Slot typeOfItem = Slot.Value;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("BeginDragged");


        // Debug.Log(this.transform.parent.name);
        parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);


        // don't allow thing to block pointer so it can be detected once its dropped
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
        
        // reallow 
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

}
