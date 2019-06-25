﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* GameObjects with this class are Draggable, and can be dragged by
 * the mouse.  They can be Variables, Values, Brackets, or Dummys.
 */
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    public Transform parentToReturnTo;
    public enum Slot {Variable, Value, All, Dummy, Bracket};
    public Slot typeOfItem = Slot.Value;
    public GameController gameController;

    // to make dragging from side of equation look slightly nicer
    private GameObject placeholder = null;
    private SimpleObjectPool pool;
    private SoundEffectManager soundEffects;
    private int variableValue;

    public void Start()
    {
        parentToReturnTo = this.transform.parent;
        gameController = FindObjectOfType<GameController>();
        soundEffects = FindObjectOfType<SoundEffectManager>();
        variableValue = gameController.GetEquation().variableValue;

        if (typeOfItem == Slot.Value)
        {
            pool = GameObject.Find("Toy Pool").GetComponent<SimpleObjectPool>();
        } else if (typeOfItem == Slot.Variable)
        {
            pool = GameObject.Find("Box Pool").GetComponent<SimpleObjectPool>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetIsDragging(true);
        soundEffects.PlayPickUpSfx();

        // create gap when dragging object
        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        placeholder.AddComponent<CanvasGroup>();
        HasValue hasValue = placeholder.AddComponent<HasValue>();
        hasValue.typeOfItem = Slot.Dummy;

        Draggable draggable = placeholder.AddComponent<Draggable>();
        draggable.typeOfItem = Slot.Dummy;

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
        SetIsDragging(false);
        soundEffects.PlayPutDownSfx(this.typeOfItem);

        // set it to wherever it should go
        this.transform.SetParent(parentToReturnTo);
        this.transform.position = parentToReturnTo.position;

        // set it to return to where the placeholder is
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

        // reallow block Raycasts so that it can be dragged again
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(placeholder);
    }

    public void SetIsDragging(bool dragging)
    {
        string side;
        if (parentToReturnTo.name.StartsWith("R"))
        {
            side = "right";
        }
        else if (parentToReturnTo.name.StartsWith("L"))
        {
            side = "left";
        }
        else
        {
            side = "none";
        }

        gameController.SetDragging(dragging, side);
    }

    // when an item is dropped on it check if it's the same type
    // then cancel it out (T1) / combine its coefficient's values
    public void OnDrop(PointerEventData eventData)
    {
        SetIsDragging(false);

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
                    soundEffects.PlayDing();
                    
                    eventData.pointerDrag.gameObject.GetComponent<Draggable>().DestroyPlaceholder();
                    Debug.Log("Destroying");

                    pool.ReturnObject(eventData.pointerDrag);
                    pool.ReturnObject(this.gameObject);
                }

            } else {
                // has coefficient, T2 and above
                Debug.Log("Dragged On");

                // make sure same type of item
                if (eventData.pointerDrag.GetComponent<Draggable>().typeOfItem == this.gameObject.GetComponent<Draggable>().typeOfItem && this.gameObject.GetComponent<Draggable>().typeOfItem != Draggable.Slot.Bracket)
                {
                    // make sure can only combine if on same side
                    string draggedParent = eventData.pointerDrag.GetComponent<Draggable>().parentToReturnTo.name;
                    string thisParent = this.gameObject.GetComponent<Draggable>().parentToReturnTo.name;

                    if ((draggedParent.StartsWith("RHS") && thisParent.StartsWith("RHS")) || (draggedParent.StartsWith("LHS") && thisParent.StartsWith("LHS")))
                    {
                        soundEffects.PlayDing();
                        
                        Fraction droppedvalue = eventData.pointerDrag.transform.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
                        Fraction thisvalue = this.transform.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
                        Fraction newvalue = thisvalue + droppedvalue;
                        Fraction.ReduceFraction(newvalue);

                        // if you drag a larger thing onto a smaller thing it needs to go to the right parent
                        if ((int) newvalue.ToDouble() == 0)
                        {
                            eventData.pointerDrag.gameObject.GetComponent<Draggable>().DestroyPlaceholder();
                            pool.ReturnObject(eventData.pointerDrag);
                            pool.ReturnObject(this.gameObject);

                        } else if (newvalue.ToDouble() > 0) {
                            // new one on positive side
                            if (this.transform.parent.name.EndsWith("Positive"))
                            {
                                this.transform.Find("Coefficient").GetComponent<Coefficient>().SetValue(newvalue);
                                eventData.pointerDrag.gameObject.GetComponent<Draggable>().DestroyPlaceholder();
                                pool.ReturnObject(eventData.pointerDrag);
                            }
                            else
                            {
                                eventData.pointerDrag.transform.Find("Coefficient").GetComponent<Coefficient>().SetValue(newvalue);
                                pool.ReturnObject(this.gameObject);
                            }

                        } else {
                            // new one on negative side
                            if (this.transform.parent.name.EndsWith("Negative"))
                            {
                                this.transform.Find("Coefficient").GetComponent<Coefficient>().SetValue(newvalue);
                                pool.ReturnObject(eventData.pointerDrag);
                            }
                            else
                            {
                                eventData.pointerDrag.transform.Find("Coefficient").GetComponent<Coefficient>().SetValue(newvalue);
                                pool.ReturnObject(this.gameObject);
                            }
                        }
                    }
                }
                else 
                {
                    // play bzz
                    soundEffects.PlayBuzzer();
                }
            }
        }
    }

    // on double click in type 2 questions split
    // currently if you double click in the middle it glitches so fix that
    public void OnPointerClick(PointerEventData eventData)
    {
        Coefficient coef = this.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>();
        if (eventData.clickCount == 2 && coef != null && (typeOfItem == Draggable.Slot.Value || typeOfItem == Draggable.Slot.Variable))
        {
            if (coef.GetValue() > 1)
            {
                Debug.Log("Hooray");
                // split apart
                StartCoroutine(ShowSplit(coef.GetFractionValue() - 1));
            }
        }
    }

    public IEnumerator ShowSplit(Fraction newCoef)
    {
        Transform panel = this.transform.Find("SplitApart");
        panel.gameObject.SetActive(true);
        panel.Find("Leftover").gameObject.GetComponent<Text>().text = newCoef.ToString();
        panel.Find("One").gameObject.GetComponent<Text>().text = 1.ToString();

        yield return new WaitForSeconds(1f);

        // check that seesaw side isn't at max capacity
        SeesawSide parent = this.transform.parent.gameObject.GetComponent<SeesawSide>();
        panel.gameObject.SetActive(false);
        if (! parent.CheckOverCapacity())
        {
            this.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(newCoef);

            GameObject newObject = pool.GetObject();
            newObject.transform.SetParent(this.transform.parent);
            newObject.GetComponent<Draggable>().parentToReturnTo = this.transform.parent;
            newObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(1);

            if (typeOfItem == Draggable.Slot.Variable)
            {
                newObject.GetComponent<HasValue>().SetValue(variableValue);
            }

            // sound effect
        }
        else
        {
            parent.OverCapacity();
        }

    }

    public void DestroyPlaceholder()
    {
        Destroy(placeholder);
    }

    public void ShowOnPositiveSide()
    {
        if (typeOfItem == Slot.Value || typeOfItem == Slot.Variable)
        {
            this.gameObject.transform.Find("Image").localScale = new Vector3(1, 1, 1);
            this.gameObject.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else if (typeOfItem == Slot.Bracket)
        {
            this.gameObject.transform.Find("Image").localScale = new Vector3(1, 1, 1);
            this.gameObject.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 139);
        }
    }

    public void ShowOnNegativeSide()
    {        
        if (typeOfItem == Slot.Value || typeOfItem == Slot.Variable)
        {
            this.gameObject.transform.Find("Image").localScale = new Vector3(-1, -1, 1);
            this.gameObject.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }
        else if (typeOfItem == Slot.Bracket)
        {
            this.gameObject.transform.Find("Image").localScale = new Vector3(1, -1, 1);
            this.gameObject.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color32(255, 43, 43, 139);
        }
    }
}
