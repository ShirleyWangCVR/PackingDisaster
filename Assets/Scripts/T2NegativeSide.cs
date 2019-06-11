using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* The Negative part of an equation side.
 */
public class T2NegativeSide : NegativeSide, IDropHandler
{
    // if Draggable object dropped onto this. Assuming all items dropped on it are Draggable.
    public new void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
    
        Draggable dragged = eventData.pointerDrag.GetComponent<Draggable>();
        if (typeOfItems == dragged.typeOfItem || typeOfItems == Draggable.Slot.All)
        {
            dragged.parentToReturnTo = this.transform;
            
            // requires checking as integer due to floating point errors
            int check = (int) Mathf.Round(eventData.pointerDrag.transform.Find("Image").localScale.x);
            if (check == 1)
            {
                eventData.pointerDrag.transform.Find("Image").localScale = new Vector3(-1, -1, 1);
                eventData.pointerDrag.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                Coefficient coef = eventData.pointerDrag.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>();
                coef.NegativeCurrentValue();

            }
        }  
    }
    
    public override double NumericalVariables()
    {
        double num = 0;
        foreach(Transform child in this.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable)
            {
                num = num + child.gameObject.GetComponent<HasValue>().GetValue() * child.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetValue();
            }
        }
        return num;
    }

    public override double NumericalValues()
    {
        double num = 0;
        foreach(Transform child in this.transform)
        {
            if (child.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value)
            {
                num = num + child.gameObject.GetComponent<HasValue>().GetValue() * child.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetValue();
            }
        }
        return num;
    }
}
