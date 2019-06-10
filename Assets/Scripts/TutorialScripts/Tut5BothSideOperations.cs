using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* A controller to process operations on both sides.
 */
public class Tut5BothSideOperations : BothSideOperations
{
    // public GameObject operationsPanel;
    // public GameObject numbersPanel;
    public Tut5GameController tutGameController;
    // public T3GameController gameController3;

    public string operation;
    public int number;
    
    // Start is called before the first frame update
    void Start()
    {
        operationsPanel.SetActive(false);
        numbersPanel.SetActive(false);
        tutGameController = FindObjectOfType<Tut5GameController>();
        // gameController3 = FindObjectOfType<T3GameController>();
    }

    // Show choose operation panel
    public override void InitiateBothSideOperations()
    {
        operationsPanel.SetActive(true);
        numbersPanel.SetActive(false);

        tutGameController.StartedBSO();
        // Debug.Log("Initiated panel");
    }

    // Show choose number panel
    public override void ChooseNumber()
    {
        operationsPanel.SetActive(true);
        numbersPanel.SetActive(true);

        tutGameController.PressedOperation();
    }

    // Back to main play screen
    /* public override void BackToMainScreen()
    {
        operationsPanel.SetActive(false);
        numbersPanel.SetActive(false);
    }
 */
    // Process the operation chosen
    public override void ProcessOperation()
    {
        tutGameController.StartedNumber();
        
        Debug.Log(operation);
        Debug.Log(number);

        tutGameController.ProcessBothSideOperation(operation, number);

        /* if (gameController != null)
        {
            gameController.ProcessBothSideOperation(operation, number);
        } else if (gameController3 != null)
        {
            gameController3.ProcessBothSideOperation(operation, number);
        } */
        
        BackToMainScreen();        
    }

    // set the chosen operation
    public override void SetOperation(string op)
    {
        operation = op;
        ChooseNumber();
        Debug.Log("Chose Operation");
        // maybe make the chosen button glow too
    }

    // set the chosen number
    public override void SetNumber(int num)
    {
        number = num;
        ProcessOperation();
        Debug.Log("Chose Number");
    }
}
