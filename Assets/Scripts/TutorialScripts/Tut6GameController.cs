using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Game Controller for the main scene where the question is solved.
 */
public class Tut6GameController : TutGameController
{    
    // variables all inherited from TutGameController
    public DialogueTrigger dialogueTrigger1;
    public DialogueTrigger dialogueTrigger2;
    public DialogueTrigger dialogueTrigger3;
    public GameObject bracketPrefab;
    // public DialogueTrigger dialogueTrigger4;

    private bool waitForFirstDrag;
    private bool waitForSecondDrag;
    private bool finishedSecondDrag;

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

        GameObject bracket = seesaw.transform.Find("LHSPositive").GetChild(0).gameObject;
        Tut6Bracket check = bracket.AddComponent<Tut6Bracket>();

        isRoundActive = false;
        waitForFirstDrag = false;
        waitForSecondDrag = false;
        finishedSecondDrag = false;

        dialogueTrigger1.TriggerInitialDialogue();
        Debug.Log("Triggering initial dialogue");
    }

    // Update is called once per frame
    void Update()
    {
        if (finishedSecondDrag)
        {
            finishedSecondDrag = false;
            Expanded();
        }
    }

    public override void FinishedFirstDialogue()
    {
        Debug.Log("Finished first dialogue");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
        waitForFirstDrag = true;
    }

    public void FirstDrop()
    {
        Debug.Log("First drop");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(false);
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(false);
        waitForFirstDrag = false;
        dialogueTrigger2.TriggerDialogue();
    }

    public override void FinishedSecondDialogue()
    {
        Debug.Log("Finished second dialogue");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
        interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(true);
        waitForSecondDrag = true;
    }

    public void SuccessfullyExpanded()
    {
        finishedSecondDrag = true;
        Debug.Log("Expanded in Game Controller");
    }

    public void Expanded()
    {
        Debug.Log("Expanded in Game Controller part 2");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(false);
        interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(false);
        waitForSecondDrag = false;
        dialogueTrigger3.TriggerDialogue();
    }

    public override void FinishedThirdDialogue()
    {
        Debug.Log("Finished third dialogue");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        // interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
        // interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(true);
        // waitForSecondDrag = true;
    }

    protected override void SetUpSeesaw()
    {
        Debug.Log("Setting up in T3");
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

        for (int i = 0; i < lhs.numBrackets; i++)
        {
            int coefficient = lhs.bracketCoefficients[i];
            Expression expression = lhs.bracketExpressions[i];

            if (coefficient > 0)
            {
                GameObject newObject = (GameObject) Instantiate(bracketPrefab);
                newObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(coefficient);
                newObject.transform.SetParent(seesaw.transform.Find("LHSPositive"));

                if (expression.numVars > 0)
                {
                    SetUpCoefficient(variablePool, newObject.transform.Find("TermsInBracket"), expression.numVars, true);
                    
                }
                else if (expression.numVars < 0)
                {
                    // change the color and upside down
                    SetUpCoefficient(variablePool, newObject.transform.Find("TermsInBracket"), expression.numVars, true);
                    
                }

                if (expression.numValues > 0)
                {
                    SetUpCoefficient(toyPool, newObject.transform.Find("TermsInBracket"), expression.numValues, false);
                    
                }
                else if (expression.numValues < 0)
                {
                    
                    // change the color and upside down
                    SetUpCoefficient(toyPool, newObject.transform.Find("TermsInBracket"), expression.numValues, false);

                }
            } 
            else if (coefficient < 0)
            {
                GameObject newObject = (GameObject) Instantiate(bracketPrefab);
                newObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(coefficient);
                newObject.transform.SetParent(seesaw.transform.Find("LHSNegative"));
                // also maybe change the color of the brackets too

                if (expression.numVars > 0)
                {
                    SetUpCoefficient(variablePool, newObject.transform.Find("TermsInBracket"), expression.numVars, true);
                    
                }
                else if (expression.numVars < 0)
                {
                    // change the color and upside down
                    SetUpCoefficient(variablePool, newObject.transform.Find("TermsInBracket"), expression.numVars, true);
                    
                }

                if (expression.numValues > 0)
                {
                    SetUpCoefficient(toyPool, newObject.transform.Find("TermsInBracket"), expression.numValues, false);
                    
                }
                else if (expression.numValues < 0)
                {
                    
                    // change the color and upside down
                    SetUpCoefficient(toyPool, newObject.transform.Find("TermsInBracket"), expression.numValues, false);

                }
            }
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

        for (int i = 0; i < rhs.numBrackets; i++)
        {
            int coefficient = rhs.bracketCoefficients[i];
            Expression expression = rhs.bracketExpressions[i];

            if (coefficient > 0)
            {
                GameObject newObject = (GameObject) Instantiate(bracketPrefab);
                newObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(coefficient);
                newObject.transform.SetParent(seesaw.transform.Find("RHSPositive"));

                if (expression.numVars > 0)
                {
                    SetUpCoefficient(variablePool, newObject.transform.Find("TermsInBracket"), expression.numVars, true);
                    
                }
                else if (expression.numVars < 0)
                {
                    // change the color and upside down
                    SetUpCoefficient(variablePool, newObject.transform.Find("TermsInBracket"), expression.numVars, true);
                    
                }

                if (expression.numValues > 0)
                {
                    SetUpCoefficient(toyPool, newObject.transform.Find("TermsInBracket"), expression.numValues, false);
                    
                }
                else if (expression.numValues < 0)
                {
                    
                    // change the color and upside down
                    SetUpCoefficient(toyPool, newObject.transform.Find("TermsInBracket"), expression.numValues, false);

                }
            } 
            else if (coefficient < 0)
            {
                GameObject newObject = (GameObject) Instantiate(bracketPrefab);
                newObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(coefficient);
                newObject.transform.SetParent(seesaw.transform.Find("RHSNegative"));
                // also maybe change the color of the brackets too

                if (expression.numVars > 0)
                {
                    SetUpCoefficient(variablePool, newObject.transform.Find("TermsInBracket"), expression.numVars, true);
                    
                }
                else if (expression.numVars < 0)
                {
                    // change the color and upside down
                    SetUpCoefficient(variablePool, newObject.transform.Find("TermsInBracket"), expression.numVars, true);
                    
                }

                if (expression.numValues > 0)
                {
                    SetUpCoefficient(toyPool, newObject.transform.Find("TermsInBracket"), expression.numValues, false);
                    
                }
                else if (expression.numValues < 0)
                {
                    
                    // change the color and upside down
                    SetUpCoefficient(toyPool, newObject.transform.Find("TermsInBracket"), expression.numValues, false);

                }
            }
        }
    }

/* 
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
 */



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
            if (seesaw.GetComponent<T3SeesawController>().CheckIfComplete())
            {
                if (seesaw.GetComponent<T3SeesawController>().CorrectlyBalanced())
                {
                    finishedDisplayManager.DisplayCorrectlyBalanced(equation.variableValue);
                } 
                else 
                {
                    // lost because wrong answer, get whatever they answered
                    int side = (int) seesaw.GetComponent<T3SeesawController>().GetLeftHandSideValue();
                    if (equation.variableValue != side)
                    {
                        finishedDisplayManager.DisplayWrongBalanced(side);
                    } else {
                        side = (int) seesaw.GetComponent<T3SeesawController>().GetRightHandSideValue();
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
        Debug.Log("Processed both side operations");
        
        if (operation == "Addition")
        {
            seesaw.GetComponent<T3SeesawController>().AddBothSides(number);
        } 
        else if (operation == "Subtraction")
        {
            seesaw.GetComponent<T3SeesawController>().SubtractBothSides(number);
        } 
        else if (operation == "Multiplication")
        {
            seesaw.GetComponent<T3SeesawController>().MultiplyBothSides(number);
        } 
        else if (operation == "Division")
        {
            seesaw.GetComponent<T3SeesawController>().DivideBothSides(number);
        }
    }

}
