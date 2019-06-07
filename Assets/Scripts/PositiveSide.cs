﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* The Positive part of an equation side
*/
public class PositiveSide : MonoBehaviour, IDropHandler
{
    public Draggable.Slot typeOfItems = Draggable.Slot.All;

    // When Draggable object is dropped onto it
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
    
        Draggable dragged = eventData.pointerDrag.GetComponent<Draggable>();
        if (typeOfItems == dragged.typeOfItem || typeOfItems == Draggable.Slot.All)
        {
            dragged.parentToReturnTo = this.transform;

            dragged.ShowOnPositiveSide();

            /* // requires checking as integer due to floating point errors
            int check = (int) Mathf.Round(eventData.pointerDrag.transform.Find("Image").localScale.x);
            if (check == -1)
            {
                eventData.pointerDrag.transform.Find("Image").localScale = new Vector3(1, 1, 1);
                eventData.pointerDrag.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            } */
        }
    }

    // get number of variables on this side
    public int NumVariables()
    {
        int num = 0;
        foreach(Transform child in this.transform)
        {
            if (child.gameObject.GetComponent<HasValue>().typeOfItem == Draggable.Slot.Variable)
            {
                num++;
            }
        }
        return num;
    }

    // get number of values on this side
    public int NumValues()
    {
        int num = 0;
        foreach(Transform child in this.transform)
        {
            if (child.gameObject.GetComponent<HasValue>().typeOfItem == Draggable.Slot.Value)
            {
                num++;
            }
        }
        return num;
    }
    
    // get the total value of all variables on this side
    public virtual double NumericalVariables()
    {
        double num = 0;
        foreach(Transform child in this.transform)
        {
            if (child.gameObject.GetComponent<HasValue>().typeOfItem == Draggable.Slot.Variable)
            {
                num = num + child.gameObject.GetComponent<HasValue>().GetValue();
            }
        }
        return num;
    }

    // get the total value of all values on this side
    public virtual double NumericalValues()
    {
        double num = 0;
        foreach(Transform child in this.transform)
        {
            if (child.gameObject.GetComponent<HasValue>().typeOfItem == Draggable.Slot.Value)
            {
                num = num + child.gameObject.GetComponent<HasValue>().GetValue();
            }
        }
        return num;
    }

    // get the total value of everything on this side
    public virtual double TotalNumericalValue()
    {
        return NumericalValues() + NumericalVariables();
    }
}
