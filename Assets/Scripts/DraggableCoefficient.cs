using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* Draggable coefficient on brackets
 */
public class DraggableCoefficient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private SoundEffectManager soundEffects;
    
    // Start is called before the first frame update
    void Start()
    {
        soundEffects = FindObjectOfType<SoundEffectManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        soundEffects.PlayPickUpCoefSfx();

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
        this.transform.position = this.transform.parent.position + new Vector3(-55, 0, 0);

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.transform.parent.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;

        foreach (Transform child in this.transform.parent.Find("TermsInBracket"))
        {
            child.gameObject.GetComponent<CanvasGroup>().ignoreParentGroups = false;
        }
    }
}
