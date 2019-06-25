using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* The Negative part of an equation side.
 */
public class SeesawSide : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Equation sides can hold all types of draggable items.
    public Draggable.Slot typeOfItems = Draggable.Slot.All;
    public enum Slot {Positive, Negative};
    public Slot typeOfSide;
    public HintSystem hintSystem;

    private int capacity;
    private bool showColor;
    private bool firstTuts;

    void Start()
    {
        int level = FindObjectOfType<DataController>().GetDifficulty();
        if (level <= 5)
        {
            capacity = 8;
        }
        else
        {
            capacity = 5;
        }
        showColor = false;
        firstTuts = level <= 2;
    }

    // if Draggable object dropped onto this. Assuming all items dropped on it are Draggable.
    public void OnDrop(PointerEventData eventData)
    {
        int size = GetCurrentSize();
        
        if (size < capacity)
        {
            GameObject drop;
            if ( !(eventData.pointerDrag.name.EndsWith("(Clone)")))
            {
                // dragging from a restock zone then hopefully
                drop = eventData.pointerDrag.GetComponent<RestockZone>().newObject;
            } 
            else
            {
                drop = eventData.pointerDrag;
            }
            
            Debug.Log(drop.name + " was dropped on " + gameObject.name);

            Draggable dragged = drop.GetComponent<Draggable>();
            dragged.parentToReturnTo = this.transform;

            if (typeOfSide == Slot.Positive)
            {
                dragged.ShowOnPositiveSide();
                Transform coefficient = drop.transform.Find("Coefficient");
                if (coefficient != null)
                {
                    Coefficient coef = coefficient.gameObject.GetComponent<Coefficient>();
                    if (coef.GetValue() < 0)
                    {
                        coef.NegativeCurrentValue();
                    }
                }
            }
            else if (typeOfSide == Slot.Negative)
            {
                dragged.ShowOnNegativeSide();
                Transform coefficient = drop.transform.Find("Coefficient");
                if (coefficient != null)
                {
                    Coefficient coef = coefficient.gameObject.GetComponent<Coefficient>();
                    if (coef.GetValue() > 0)
                    {
                        coef.NegativeCurrentValue();
                    }
                }
            }
        }
        else
        {
            OverCapacity();
        }
    }

    // returns how much is currently on this side
    public int GetCurrentSize()
    {
        return NumValues() + NumVariables() + (int) (NumBrackets() * 2.5);
    }

    public bool CheckOverCapacity()
    {
        return GetCurrentSize() >= capacity;
    }

    public bool CheckOverCapacity(double add)
    {
        return GetCurrentSize() + add > capacity;
    }

    public void OverCapacity()
    {
        Debug.Log("Over Capacity");
        if (hintSystem != null)
        {
            hintSystem.SeesawSideOverflow();
            // TODO: play some bad sound effect
        }
    }

    // get the number of variables currently on this side
    public int NumVariables()
    {
        int num = 0;
        foreach(Transform child in this.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable)
            {
                num++;
            }
        }
        return num;
    }

    // get the number of values currently on this side
    public int NumValues()
    {
        int num = 0;
        foreach(Transform child in this.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value)
            {
                num++;
            }
        }
        return num;
    }

    public int NumBrackets()
    {
        int num = 0;
        foreach(Transform child in this.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Bracket)
            {
                num++;
            }
        }
        return num;
    }

    // get the total value of all variables on this side
    public double NumericalVariables()
    {
        double num = 0;
        foreach(Transform child in this.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable)
            {
                Transform coefficient = child.Find("Coefficient");
                double coef;
                if (coefficient != null)
                {
                    coef = coefficient.gameObject.GetComponent<Coefficient>().GetValue();
                } else {
                    coef = 1;
                }
                num = num + child.gameObject.GetComponent<HasValue>().GetValue() * coef;
            }
        }
        return num;
    }

    // get the total value of all values on this side
    public double NumericalValues()
    {
        double num = 0;
        foreach(Transform child in this.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value)
            {
                Transform coefficient = child.Find("Coefficient");
                double coef;
                if (coefficient != null)
                {
                    coef = coefficient.gameObject.GetComponent<Coefficient>().GetValue();
                } else {
                    coef = 1;
                }
                num = num + child.gameObject.GetComponent<HasValue>().GetValue() * coef;
            }
        }
        return num;
    }

    public double NumericalBrackets()
    {
        double num = 0;
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Bracket)
            {
                num = num + child.gameObject.GetComponent<Bracket>().GetValue();
            }
        }
        return num;
    }

    public double TotalNumericalValue()
    {
        return NumericalValues() + NumericalVariables() + NumericalBrackets();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // if not tutorial and if dragging then glow panel
        bool currDragging = this.transform.parent.gameObject.GetComponent<SeesawController>().GetDragging();
        if (currDragging && ! firstTuts)
        {
            if (this.typeOfSide == SeesawSide.Slot.Positive)
            {
                this.gameObject.GetComponent<Image>().color = new Color32(0, 255, 243, 128);
            }
            else
            {
                this.gameObject.GetComponent<Image>().color = new Color32(255, 16, 0, 100);
            }
            
            showColor = true;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // if not tutorial and if panel glowing then stop glow
        if (! firstTuts && showColor)
        {
            this.gameObject.GetComponent<Image>().color = new Color32(255, 16, 0, 0);
            showColor = false;
        }
    }
}
