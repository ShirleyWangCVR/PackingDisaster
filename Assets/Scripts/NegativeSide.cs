using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NegativeSide : MonoBehaviour, IDropHandler
{
    
    public Draggable.Slot typeOfItems = Draggable.Slot.All;
    public GameObject seesaw;

    private float xposition;
    private float yposition;

    // Start is called before the first frame update
    void Start()
    {
        xposition = this.transform.position.x - seesaw.transform.position.x;
        yposition = this.transform.position.y - seesaw.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetXPosition()
    {
        return xposition;
    }

    public float GetYPosition()
    {
        return yposition;
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
