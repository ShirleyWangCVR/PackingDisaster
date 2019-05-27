using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class RestockZone : MonoBehaviour, IDropHandler
{
    
    public GameObject createdPrefab;
    public Draggable.Slot typeOfItems = Draggable.Slot.Value;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When the toy crate is clicked
    public void OnClick()
    {
        GameObject newbear = Instantiate(createdPrefab, this.transform.position, Quaternion.identity);
        newbear.transform.SetParent(this.transform, true);
    }

    // when a toy is dropped onto it it should disappear
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        Draggable dragged = eventData.pointerDrag.GetComponent<Draggable>();
        
        if (typeOfItems == dragged.typeOfItem || typeOfItems == Draggable.Slot.All)
        {
            Destroy(eventData.pointerDrag);
        }
    
    }
}
