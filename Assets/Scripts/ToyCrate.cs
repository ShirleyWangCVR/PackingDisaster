using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToyCrate : MonoBehaviour, IDropHandler
{
    
    public GameObject teddybearPrefab;
    public GameObject canvas;
    
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
        GameObject newbear = Instantiate(teddybearPrefab, this.transform.position, Quaternion.identity);
        newbear.transform.SetParent(canvas.transform, true);
    }

    // when a toy is dropped onto it it should disappear
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        if (eventData.pointerDrag.name.StartsWith("Teddy"))
        {
            Destroy(eventData.pointerDrag);
        }
    
    }
}
