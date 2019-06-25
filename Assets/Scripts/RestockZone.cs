﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

/* A zone where you click on to spawn another type of item, and drop items on
 * to get rid of them.
 */
public class RestockZone : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // the object pool of the game object that this zone will restock
    public SimpleObjectPool objectPool;
    public Draggable.Slot typeOfItems;
    public GameController gameController;
    public GameObject newObject;
    // public AudioClip droppedSfx;

    private Draggable childScript;
    // private AudioSource audioSource;
    private SoundEffectManager soundEffects;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        soundEffects = FindObjectOfType<SoundEffectManager>();
        //         audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    public void OnBeginDrag(PointerEventData pointerDrag)
    {
        newObject = objectPool.GetObject(); 
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
            newObject.GetComponent<HasValue>().SetValue(gameController.GetEquation().variableValue);
        }

        if (newObject.transform.Find("Coefficient") != null)
        {
            Coefficient coef = newObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>();
            coef.SetValue(1);
        }

        childScript = newObject.GetComponent<Draggable>();
        childScript.Start();
        childScript.typeOfItem = typeOfItems;
        childScript.gameController = gameController;
        childScript.OnBeginDrag(pointerDrag);
        childScript.parentToReturnTo = this.gameObject.transform;
    }

    public void OnDrag(PointerEventData pointerDrag)
    {
        childScript.OnDrag(pointerDrag);

        // need to set this throughout entire drag so that it maintains to enddrag
        childScript.parentToReturnTo = this.gameObject.transform;
    }

    public void OnEndDrag(PointerEventData pointerDrag)
    {
        
        if (childScript.parentToReturnTo.name == this.transform.name)
        {
            childScript.OnEndDrag(pointerDrag);
            objectPool.ReturnObject(newObject);
            return;
        }
        
        childScript.OnEndDrag(pointerDrag);
        newObject = null;
        childScript = null;
    }

    // when an item is dropped on it get rid of it
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);
        // audioSource.PlayOneShot(droppedSfx, 5.0f);
        soundEffects.PlayRestocked();

        Draggable dragged = eventData.pointerDrag.GetComponent<Draggable>();
        if (typeOfItems == dragged.typeOfItem || typeOfItems == Draggable.Slot.All)
        {
            eventData.pointerDrag.GetComponent<Draggable>().SetIsDragging(false);
            objectPool.ReturnObject(eventData.pointerDrag);
        }
    
    }
}
