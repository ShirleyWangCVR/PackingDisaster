using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* GameObjects with this class are Draggable, and can be dragged by
 * the mouse.  They can be Variables, Values, or Dummys.
 */
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Transform parentToReturnTo;
    public enum Slot {Variable, Value, All, Dummy};
    public Slot typeOfItem = Slot.Value;

    // to make dragging from side of equation look slightly nicer
    GameObject placeholder = null;

    public void Start()
    {
        parentToReturnTo = this.transform.parent;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
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

        // set parent to return to so that if you let go while it's not on a valid side
        // it returns to its previous side
        parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);

        // set blockRaycasts to false while dragging so pointer can be detected 
        // so object can be detected on drop zones
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // set it to wherever it should go
        this.transform.SetParent(parentToReturnTo);
        this.transform.position = parentToReturnTo.position;

        // set it to return to where the placeholder is
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        
        // reallow block Raycasts so that it can be dragged again
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(placeholder);
    }

    public void DestroyPlaceholder()
    {
        Destroy(placeholder);
    }

    // when an item is dropped on it check if it's the same type
    // then cancel it out (T1) / combine its coefficient's values
    public void OnDrop(PointerEventData eventData)
    {
        Draggable dragged = eventData.pointerDrag.GetComponent<Draggable>();
        if (dragged != null)
        {
            Transform coef = eventData.pointerDrag.transform.Find("Coefficient");
            if (coef == null)
            {
                // still T1
                // check if opposite (u negative it positive or vice versa) then return both else do nothing
                int droppedorient = (int) Mathf.Round(eventData.pointerDrag.transform.Find("Image").localScale.x);
                int thisorient = (int) Mathf.Round(this.transform.Find("Image").localScale.x);
                if (droppedorient == 0 - thisorient)
                {
                    eventData.pointerDrag.gameObject.GetComponent<Draggable>().DestroyPlaceholder();
                    Debug.Log("Destroying");

                    // try and use the pool more efficient
                    // would need to search for the right pool
                    Destroy(eventData.pointerDrag);
                    Destroy(this.gameObject);
                }

            } else {
                // has coefficient, T2 and above
                // for now only doing whole numbers
                // handle fractions later
                Debug.Log("Dragged On");

                // make sure same type of item
                if (eventData.pointerDrag.GetComponent<HasValue>().typeOfItem == this.gameObject.GetComponent<HasValue>().typeOfItem)
                {
                    // make sure can only combine if on same side
                    string draggedParent = eventData.pointerDrag.GetComponent<Draggable>().parentToReturnTo.name;
                    string thisParent = this.gameObject.GetComponent<Draggable>().parentToReturnTo.name;

                    Debug.Log("Same Type");

                    if ((draggedParent.StartsWith("RHS") && thisParent.StartsWith("RHS")) || (draggedParent.StartsWith("LHS") && thisParent.StartsWith("LHS")))
                    {
                        Debug.Log("Same Side");
                        
                        Fraction droppedvalue = eventData.pointerDrag.transform.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
                        Fraction thisvalue = this.transform.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
                        Fraction newvalue = thisvalue + droppedvalue;
                        Fraction.ReduceFraction(newvalue);

                        // if you drag a larger thing onto a smaller thing it needs to go to the right parent
                        if ((int) newvalue.ToDouble() == 0)
                        {
                            eventData.pointerDrag.gameObject.GetComponent<Draggable>().DestroyPlaceholder();
                            Destroy(eventData.pointerDrag);
                            Destroy(this.gameObject);

                        } else if (newvalue.ToDouble() > 0) {
                            // new one on positive side
                            if (this.transform.parent.name.EndsWith("Positive"))
                            {
                                this.transform.Find("Coefficient").GetComponent<Coefficient>().SetValue(newvalue);
                                eventData.pointerDrag.gameObject.GetComponent<Draggable>().DestroyPlaceholder();
                                Destroy(eventData.pointerDrag);
                            } else {
                                eventData.pointerDrag.transform.Find("Coefficient").GetComponent<Coefficient>().SetValue(newvalue);
                                Destroy(this.gameObject);
                            }

                        } else {
                            // new one on negative side
                            if (this.transform.parent.name.EndsWith("Negative"))
                            {
                                this.transform.Find("Coefficient").GetComponent<Coefficient>().SetValue(newvalue);
                                Destroy(eventData.pointerDrag);
                            } else {
                                eventData.pointerDrag.transform.Find("Coefficient").GetComponent<Coefficient>().SetValue(newvalue);
                                Destroy(this.gameObject);
                            }

                        }


                    }
                    
                }                

            }
        }
    
    }

}
