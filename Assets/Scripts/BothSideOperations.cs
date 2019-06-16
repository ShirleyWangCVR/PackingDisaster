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
    public Text LHS;
    public Text RHS;
    public Text operationText;
    public AudioClip clickedSfx;
    public AudioClip completedSfx;

    private string operation;
    private int number;
    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        operationsPanel.SetActive(false);
        numbersPanel.SetActive(false);
        gameController = FindObjectOfType<T2GameController>();
        tutController = FindObjectOfType<TutorialController>();
        LHS.gameObject.SetActive(false);
        RHS.gameObject.SetActive(false);
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Show choose operation panel
    public void InitiateBothSideOperations()
    {
        audioSource.PlayOneShot(clickedSfx, 5.0f);
        operationsPanel.SetActive(true);
        numbersPanel.SetActive(false);

        if (tutController != null)
        {
            tutController.StartedBSO();
        }
    }

    // Show choose number panel
    public void ChooseNumber()
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
        audioSource.PlayOneShot(clickedSfx, 5.0f);
        operationsPanel.SetActive(false);
        numbersPanel.SetActive(false);
    }

    // Process the operation chosen
    public IEnumerator ProcessOperation()
    {
        Debug.Log(operation);
        Debug.Log(number);
        yield return new WaitForSeconds(1f);

        if (gameController != null)
        {
            gameController.ProcessBothSideOperation(operation, number);
        } 
        
        if (tutController != null)
        {
            tutController.StartedNumber();
        }
        LHS.gameObject.SetActive(false);
        RHS.gameObject.SetActive(false);
        audioSource.PlayOneShot(completedSfx, 5.0f);
        operationsPanel.SetActive(false);
        numbersPanel.SetActive(false);        
    }

    // set the chosen operation
    public void SetOperation(string op)
    {
        audioSource.PlayOneShot(clickedSfx, 5.0f);
        
        operation = op;
        LHS.gameObject.SetActive(true);
        RHS.gameObject.SetActive(true);
        ChooseNumber();

        if (op == "Addition")
        {
            LHS.text = "+";
            RHS.text = "+";
            operationText.text = "+";
        }
        else if (op == "Subtraction")
        {
            LHS.text = "-";
            RHS.text = "-";
            operationText.text = "-";
        }
        else if (op == "Multiplication")
        {
            LHS.text = "x";
            RHS.text = "x";
            operationText.text = "x";
        }
        else if (op == "Division")
        {
            LHS.text = "÷";
            RHS.text = "÷";
            operationText.text = "÷";
        }

        // maybe make the chosen button glow too
    }

    // set the chosen number
    public void SetNumber(int num)
    {
        audioSource.PlayOneShot(clickedSfx, 5.0f);
        
        number = num;
        LHS.text = LHS.text + num.ToString();
        RHS.text = RHS.text + num.ToString();
        operationText.text = operationText.text + num.ToString();

        // wait one second then do it
        StartCoroutine(ProcessOperation());
    }

}
