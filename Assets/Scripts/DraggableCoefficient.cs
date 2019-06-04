using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableCoefficient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo;
    public Vector3 coefPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        parentToReturnTo = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        coefPosition = this.transform.position;

        GetComponent<CanvasGroup>().blocksRaycasts = false;
        this.transform.parent.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        // this.transform.parent.gameObject.GetComponent<CanvasGroup>().interactable = false;

        foreach (Transform child in this.transform.parent.Find("TermsInBracket"))
        {
            child.gameObject.GetComponent<CanvasGroup>().ignoreParentGroups = true;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position + new Vector2(-10, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.position = coefPosition;

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.transform.parent.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;

        foreach (Transform child in this.transform.parent.Find("TermsInBracket"))
        {
            child.gameObject.GetComponent<CanvasGroup>().ignoreParentGroups = false;
        }
    }

    
}
