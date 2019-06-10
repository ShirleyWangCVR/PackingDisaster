using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Game Controller for the main scene where the question is solved.
 */
public class Tut5GameController : TutGameController
{    
    // variables all inherited from TutGameController
    public DialogueTrigger dialogueTrigger1;
    public DialogueTrigger dialogueTrigger2;
    public DialogueTrigger dialogueTrigger3;
    public DialogueTrigger dialogueTrigger4;

    private bool waitForFirstDrag;
    private bool waitForSecondDrag;
    private bool waitForThirdDrag;

    // Start is called before the first frame update
    void Start()
    {
        // get data from dataController
        dataController = FindObjectOfType<DataController>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        level = dataController.GetDifficulty();
        equation = dataController.GetCurrentEquationData(level);
        timeUsed = 0;
        
        // set up seesaw according to equation
        SetUpSeesaw();
        // timeUsedText.text = "Time Used: " + timeUsed.ToString();

        isRoundActive = false;
        waitForFirstDrag = false;
        waitForSecondDrag = false;
        waitForThirdDrag = false;

        dialogueTrigger1.TriggerInitialDialogue();
        Debug.Log("Triggering initial dialogue");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void FinishedFirstDialogue()
    {
        Debug.Log("Finished first dialogue");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
        waitForFirstDrag = true;
    }

    public void StartedBSO()
    {
        Debug.Log("Started Panel");
        
        if (waitForFirstDrag)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            dialogueTrigger2.TriggerDialogue();
            interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(false);
            waitForFirstDrag = false;
            Debug.Log("Did the thing");
        }
    }

    public override void FinishedSecondDialogue()
    {
        Debug.Log("Finished Second Dialogue");
        
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
        waitForSecondDrag = true;
    }

    public void PressedOperation()
    {
        Debug.Log("Pressed Operation");
        
        if (waitForSecondDrag)
        {
            Debug.Log("Pressed Operation");

            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(false);
            dialogueTrigger3.TriggerDialogue();
            waitForSecondDrag = false;
        }
    }

    public override void FinishedThirdDialogue()
    {
        Debug.Log("Finished Third Dialogue");

        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(true);
        waitForThirdDrag = true;
    }

    public void StartedNumber()
    {
        if (waitForThirdDrag)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(false);
            dialogueTrigger4.TriggerDialogue();
            waitForThirdDrag = false;
        }
    }

    public override void FinishedFourthDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    // set up the seesaw according to the equation data
    protected override void SetUpSeesaw()
    {
        Expression lhs = equation.lhs;
        Expression rhs = equation.rhs;
        
        if (lhs.numVars > 0)
        {
            SetUpCoefficient(variablePool, seesaw.transform.Find("LHSPositive"), lhs.numVars, true);
            
        }
        else if (lhs.numVars < 0)
        {
            SetUpCoefficient(variablePool, seesaw.transform.Find("LHSNegative"), lhs.numVars, true);
            
        }

        if (lhs.numValues > 0)
        {
            SetUpCoefficient(toyPool, seesaw.transform.Find("LHSPositive"), lhs.numValues, false);
            
        }
        else if (lhs.numValues < 0)
        {
            SetUpCoefficient(toyPool, seesaw.transform.Find("LHSNegative"), lhs.numValues, false);

        }

        if (rhs.numVars > 0)
        {
            SetUpCoefficient(variablePool, seesaw.transform.Find("RHSPositive"), rhs.numVars, true);

        }
        else if (rhs.numVars < 0)
        {
            SetUpCoefficient(variablePool, seesaw.transform.Find("RHSNegative"), rhs.numVars, true);

        }

        if (rhs.numValues > 0)
        {
            SetUpCoefficient(toyPool, seesaw.transform.Find("RHSPositive"), rhs.numValues, false);
            
        }
        else if (rhs.numValues < 0)
        {
            SetUpCoefficient(toyPool, seesaw.transform.Find("RHSNegative"), rhs.numValues, false);
        }
    }

    protected void SetUpCoefficient(SimpleObjectPool pool, Transform side, int number, bool isVar)
    {
        GameObject newVar = pool.GetObject();
        newVar.transform.SetParent(side);

        if (isVar)
        {
            newVar.GetComponent<HasValue>().SetValue(equation.variableValue);
        }

        // currently defaulting initial value is whole number
        newVar.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(number);
    }

    // end the current round
    public override void EndRound(string howEnded)
    {
        // deactivate game logic
        isRoundActive = false;
        // playerScore = (int) Mathf.Round(timeRemaining);
        // dataController.SubmitNewPlayerScore(playerScore);
        // int highestScore = dataController.GetHighestPlayerScore();

        if (howEnded == "Time Out")
        {
            finishedDisplayManager.DisplayTimeOut();
        } 
        else if (howEnded == "Finished Check") 
        {
            if (seesaw.GetComponent<T2SeesawController>().CheckIfComplete())
            {
                if (seesaw.GetComponent<T2SeesawController>().CorrectlyBalanced())
                {
                    finishedDisplayManager.DisplayCorrectlyBalanced(equation.variableValue);
                } 
                else 
                {
                    // lost because wrong answer, get whatever they answered
                    int side = (int) seesaw.GetComponent<T2SeesawController>().GetLeftHandSideValue();
                    if (equation.variableValue != side)
                    {
                        finishedDisplayManager.DisplayWrongBalanced(side);
                    } else {
                        side = (int) seesaw.GetComponent<T2SeesawController>().GetRightHandSideValue();
                        finishedDisplayManager.DisplayWrongBalanced(side);
                    }
                }
            }
            else 
            {
                finishedDisplayManager.DisplayNotYetBalanced();
            }
        } else if (howEnded == "Scale Tipped") 
        {
            finishedDisplayManager.DisplaySeesawTipped();
        }
    }

    public void ProcessBothSideOperation(string operation, int number)
    {
        if (operation == "Addition")
        {
            seesaw.GetComponent<T2SeesawController>().AddBothSides(number);
        } 
        else if (operation == "Subtraction")
        {
            seesaw.GetComponent<T2SeesawController>().SubtractBothSides(number);
        } 
        else if (operation == "Multiplication")
        {
            seesaw.GetComponent<T2SeesawController>().MultiplyBothSides(number);
        } 
        else if (operation == "Division")
        {
            seesaw.GetComponent<T2SeesawController>().DivideBothSides(number);
        }
    }

}
