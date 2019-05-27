using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    public Transform parentToReturnTo = null;
    public enum Slot {Variable, Value, All, Dummy};
    public Slot typeOfItem = Slot.Value;

    GameObject placeholder = null;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("BeginDragged");

        // create gap when dragging object
        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        HasValue hasValue = placeholder.AddComponent<HasValue>();
        hasValue.typeOfItem = Slot.Dummy;

        placeholder.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y);
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

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

        
       
        this.transform.SetParent(parentToReturnTo);
        

        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        
        // reallow 
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(placeholder);
    }

}
