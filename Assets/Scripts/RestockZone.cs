using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class RestockZone : MonoBehaviour, IDropHandler
{
    
    // public GameObject createdPrefab;
    public SimpleObjectPool objectPool;
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
        GameObject newObject = objectPool.GetObject(); // Instantiate(createdPrefab, this.transform.position, Quaternion.identity);
        newObject.transform.position = this.transform.position;
        newObject.transform.SetParent(this.transform, true);

        int check = (int) Mathf.Round(newObject.transform.localScale.x);
        if (check == -1)
        {
            newObject.transform.localScale = new Vector3(1, 1, 1);
            newObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    // when a toy is dropped onto it it should disappear
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        Draggable dragged = eventData.pointerDrag.GetComponent<Draggable>();
        if (typeOfItems == dragged.typeOfItem || typeOfItems == Draggable.Slot.All)
        {
            objectPool.ReturnObject(eventData.pointerDrag);
        }
    
    }
}
