using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

/* A zone where you click on to spawn another type of item, and drop items on
 * to get rid of them.
 */
public class RestockZone : MonoBehaviour, IDropHandler
{
    // the object pool of the game object that this zone will restock
    public SimpleObjectPool objectPool;
    public Draggable.Slot typeOfItems;
    private DataController dataController;
    private int variableValue;

    public void Start()
    {
        dataController = FindObjectOfType<DataController>();
        variableValue = dataController.GetCurrentEquationData().variableValue;
    }

    // When clicked on create another object
    public void OnClick()
    {
        GameObject newObject = objectPool.GetObject(); 
        newObject.transform.position = this.transform.position + new Vector3(30, -30, 0);
        newObject.transform.SetParent(this.transform, true);

        // make sure orientation is correct
        int check = (int) Mathf.Round(newObject.transform.localScale.x);
        if (check == -1)
        {
            newObject.transform.Find("Image").localScale = new Vector3(1, 1, 1);
            newObject.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        if (typeOfItems == Draggable.Slot.Value)
        {
            newObject.GetComponent<HasValue>().SetValue(1);
        } 
        else if (typeOfItems == Draggable.Slot.Variable)
        {
            newObject.GetComponent<HasValue>().SetValue(variableValue);
        }

        if (newObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>() != null)
        {
            Coefficient coef = newObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>();
            coef.SetValue(1);
        }
    }

    // when an item is dropped on it get rid of it
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        Draggable dragged = eventData.pointerDrag.GetComponent<Draggable>();
        if (typeOfItems == dragged.typeOfItem || typeOfItems == Draggable.Slot.All)
        {
            eventData.pointerDrag.GetComponent<Draggable>().SetIsDragging(false);
            objectPool.ReturnObject(eventData.pointerDrag);
        }
    
    }
}
