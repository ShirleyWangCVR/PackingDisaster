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
    public T3GameController gameController3;

    private string operation;
    private int number;
    
    // Start is called before the first frame update
    void Start()
    {
        operationsPanel.SetActive(false);
        numbersPanel.SetActive(false);
        gameController = FindObjectOfType<T2GameController>();
        gameController3 = FindObjectOfType<T3GameController>();
    }

    public void InitiateBothSideOperations()
    {
        operationsPanel.SetActive(true);
        numbersPanel.SetActive(false);
    }

    public void ChooseNumber()
    {
        operationsPanel.SetActive(true);
        numbersPanel.SetActive(true);
    }

    public void BackToMainScreen()
    {
        operationsPanel.SetActive(false);
        numbersPanel.SetActive(false);
    }

    public void ProcessOperation()
    {
        Debug.Log(operation);
        Debug.Log(number);

        if (gameController != null)
        {
            gameController.ProcessBothSideOperation(operation, number);
        } else if (gameController3 != null)
        {
            gameController3.ProcessBothSideOperation(operation, number);
        }
        // gameController.ProcessBothSideOperation(operation, number);

        BackToMainScreen();        
    }

    public void SetOperation(string op)
    {
        operation = op;
        ChooseNumber();
        // maybe make the chosen button glow too
    }

    public void SetNumber(int num)
    {
        number = num;
        ProcessOperation();
    }

}
