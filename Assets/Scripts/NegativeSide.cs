using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NegativeSide : MonoBehaviour, IDropHandler
{
    
    public Draggable.Slot typeOfItems = Draggable.Slot.All;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
    
        Draggable dragged = eventData.pointerDrag.GetComponent<Draggable>();
        
        if (typeOfItems == dragged.typeOfItem || typeOfItems == Draggable.Slot.All)
        {
            dragged.parentToReturnTo = this.transform;
            
            // requires checking as integer due to floating point errors
            int check = (int) Mathf.Round(eventData.pointerDrag.transform.localScale.x);
            if (check == 1)
            {
                eventData.pointerDrag.transform.localScale = new Vector3(-1, -1, 1);
                eventData.pointerDrag.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            }
        }  
    }

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
}
