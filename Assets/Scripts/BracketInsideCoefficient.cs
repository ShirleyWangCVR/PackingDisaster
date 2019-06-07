using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BracketInsideCoefficient : MonoBehaviour, IDropHandler
{
    public bool droppedOn;
    
    private Bracket bracket;
    
    // Start is called before the first frame update
    void Start()
    {
        droppedOn = false;

        bracket = this.gameObject.transform.parent.parent.parent.GetComponent<Bracket>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped On");

        if (! droppedOn)
        {
            droppedOn = true;
            bracket.TermDroppedOn();
            // StartCoroutine(bracket.TermDroppedOn());
            
            bracket.Invoke("CheckExpanded", 1f);
            Debug.Log("Waited");
        }
    }
}
