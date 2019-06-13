using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* A controller to process operations on both sides.
 */
public class BothSideOperations : MonoBehaviour
{
    public GameObject operationsPanel;
    public GameObject numbersPanel;
    public T2GameController gameController;
    public TutorialController tutController;

    private string operation;
    private int number;
    
    // Start is called before the first frame update
    void Start()
    {
        operationsPanel.SetActive(false);
        numbersPanel.SetActive(false);
        gameController = FindObjectOfType<T2GameController>();
        tutController = FindObjectOfType<TutorialController>();
    }

    // Show choose operation panel
    public virtual void InitiateBothSideOperations()
    {
        operationsPanel.SetActive(true);
        numbersPanel.SetActive(false);

        if (tutController != null)
        {
            tutController.StartedBSO();
        }
    }

    // Show choose number panel
    public virtual void ChooseNumber()
    {
        operationsPanel.SetActive(true);
        numbersPanel.SetActive(true);

        if (tutController != null)
        {
            tutController.PressedOperation();
        }
    }

    // Back to main play screen
    public void BackToMainScreen()
    {
        operationsPanel.SetActive(false);
        numbersPanel.SetActive(false);
    }

    // Process the operation chosen
    public virtual void ProcessOperation()
    {
        Debug.Log(operation);
        Debug.Log(number);

        if (gameController != null)
        {
            gameController.ProcessBothSideOperation(operation, number);
        } 
        
        if (tutController != null)
        {
            tutController.StartedNumber();
        }
        
        BackToMainScreen();        
    }

    // set the chosen operation
    public virtual void SetOperation(string op)
    {
        operation = op;
        ChooseNumber();
        // maybe make the chosen button glow too
    }

    // set the chosen number
    public virtual void SetNumber(int num)
    {
        number = num;
        ProcessOperation();
    }

}
