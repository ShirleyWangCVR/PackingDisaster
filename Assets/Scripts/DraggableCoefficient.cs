using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* Draggable coefficient on brackets
 */
public class DraggableCoefficient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo;
    public Vector3 coefPosition;
    public AudioClip pickUpSfx;

    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        parentToReturnTo = this.transform.parent;
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        audioSource.PlayOneShot(pickUpSfx, 7.0f);
        
        coefPosition = this.transform.position;

        GetComponent<CanvasGroup>().blocksRaycasts = false;
        this.transform.parent.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;

        // make sure bracket coefficients can still be dropped onto
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
