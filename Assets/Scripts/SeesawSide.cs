using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* The Negative part of an equation side.
 */
public class SeesawSide : MonoBehaviour, IDropHandler
{
    // Equation sides can hold all types of draggable items.
    public Draggable.Slot typeOfItems = Draggable.Slot.All;
    public enum Slot {Positive, Negative};
    public Slot typeOfSide;

    // if Draggable object dropped onto this. Assuming all items dropped on it are Draggable.
    public void OnDrop(PointerEventData eventData)
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
        if (typeOfItems == dragged.typeOfItem || typeOfItems == Draggable.Slot.All)
        {
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

}
