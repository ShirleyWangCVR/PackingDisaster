using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            }
        }
            
    }
}
