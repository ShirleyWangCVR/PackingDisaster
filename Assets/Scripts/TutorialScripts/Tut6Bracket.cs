using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tut6Bracket : MonoBehaviour
{
    public int numDroppedOn;
    public Tut6GameController gameController;
    
    // Start is called before the first frame update
    void Start()
    {
        numDroppedOn = 0;   
        gameController = FindObjectOfType<Tut6GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (numDroppedOn != this.gameObject.GetComponent<Bracket>().numDroppedOn && this.gameObject.GetComponent<Bracket>().numDroppedOn == 1)
        {
            numDroppedOn = this.gameObject.GetComponent<Bracket>().numDroppedOn;
            gameController.FirstDrop();
        }
        else if (numDroppedOn != this.gameObject.GetComponent<Bracket>().numDroppedOn && this.gameObject.GetComponent<Bracket>().numDroppedOn == 2)
        {
            Debug.Log("Reached tut6bracket");
            numDroppedOn = this.gameObject.GetComponent<Bracket>().numDroppedOn;
            gameController.SuccessfullyExpanded();
        }
        
        

    }
}
