using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Game Controller for the main scene where the question is solved.
 */
public class Tut1GameController : GameController
{    
    // public GameObject seesaw;
    public SimpleObjectPool variablePool;
    public SimpleObjectPool toyPool;
    // public FinishedPanelManager finishedDisplayManager;
    // public DialogueController dialogueController;
    public TutorialManager tutorialManager;
    public DialogueModuleManager dialogueModuleManager;
    public DialogueTrigger dialogueTrigger1;
    public DialogueTrigger dialogueTrigger2;
    public DialogueTrigger dialogueTrigger3;
    public DialogueTrigger dialogueTrigger4;
    public DialogueTrigger dialogueTrigger5;
    public DialogueTrigger dialogueTrigger6;
    public GameObject interactivePanel;

    // DataController dataController;
    // EquationData equation; // current equation being displayed
    
    private bool isRoundActive; 
    private float timeUsed;
    private int difficultyLevel; // difficulty levels 0 to 5? 1 to 5? 0 being tutorial?
    private int equationsCompleted; // currently not being used
    private int playerScore; // currently not being used
    private bool dialogueActive;
    private bool currentlyDragging;
    private int dialogueBatch;
    private bool waitForFirstDrag;
    private bool waitForSecondDrag;
    private bool waitForThirdDrag;
    private bool waitForFourthDrag;
    private bool waitForFifthDrag;

    // Start is called before the first frame update
    void Start()
    {
        // get data from dataController
        dataController = FindObjectOfType<DataController>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        equation = dataController.GetCurrentEquationData();
        difficultyLevel = dataController.GetDifficulty();
        equationsCompleted = dataController.GetEquationsCompleted();
        timeUsed = 0;

        dialogueBatch = 1;
        
        // set up seesaw according to equation
        SetUpSeesaw();

        // set up tutorial dialogue
        
        isRoundActive = false;
        dialogueActive = true;
        waitForFirstDrag = false;
        waitForSecondDrag = false;
        waitForThirdDrag = false;
        waitForFourthDrag = false;
        waitForFifthDrag = false;

        dialogueTrigger1.TriggerInitialDialogue();
    }

        // Update is called once per frame
    void Update()
    {
        if (waitForFirstDrag)
        {
            if (seesaw.GetComponent<Tut1SeesawController>().CheckOneDraggedUnbalanced())
            {
                waitForFirstDrag = false;
                DraggedToOneSide();
            }
        }

        if (waitForSecondDrag)
        {
            if (seesaw.GetComponent<Tut1SeesawController>().CheckAnotherDraggedBalanced())
            {
                waitForSecondDrag = false;
                DraggedToOtherSide();
            }
        }

        if (waitForThirdDrag)
        {
            if (seesaw.GetComponent<Tut1SeesawController>().CheckDraggedToToyBoxUnBalanced())
            {
                waitForThirdDrag = false;
                DraggedToToyBox();
            }
        }

        if (waitForFourthDrag)
        {
            if (seesaw.GetComponent<Tut1SeesawController>().CheckDraggedFromToyBoxBalanced())
            {
                waitForFourthDrag = false;
                DraggedFromToyBox();
            }
        }

        if (waitForFifthDrag)
        {
            if (seesaw.GetComponent<Tut1SeesawController>().CheckDraggedStillBalanced())
            {
                waitForFifthDrag = false;
                DraggedCorrectly();
            }
        }
        
        
        // if round active count down time display
        if (isRoundActive) 
        {
            // timeUsed += Time.deltaTime;
            // UpdateTimeUsedDisplay();
        }

        // if seesaw fell over end game
        /* if (seesaw.GetComponent<SeesawController>().FellOver())
        {
            // EndRound("Scale Tipped");
        } */

        // if dialogue currently active check until dialogue no longer active
        if (dialogueActive)
        {
            // isRoundActive = dialogueController.FinishedDialogue();
            if (isRoundActive)
            {
                dialogueActive = false;
            }
        }
    }

    public void SetDragging(bool dragging)
    {
        Debug.Log("Set Dragging in Tut1");
        
        currentlyDragging = dragging;
        seesaw.GetComponent<Tut1SeesawController>().SetDragging(dragging);
    }


    public void FinishedFirstDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
        waitForFirstDrag = true;
    }

    
    public void DraggedToOneSide()
    {
        Debug.Log("Completed First Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(false);
        dialogueTrigger2.TriggerDialogue();
    }

    public void FinishedSecondDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
        interactivePanel.transform.Find("Seesaw Arrow").localScale = new Vector2(0 - interactivePanel.transform.Find("Seesaw Arrow").localScale.x, interactivePanel.transform.Find("Seesaw Arrow").localScale.y);
        waitForSecondDrag = true;
    }

    public void DraggedToOtherSide()
    {
        Debug.Log("Completed Second Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(false);
        dialogueTrigger3.TriggerDialogue();
    }

    public void FinishedThirdDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
        waitForThirdDrag = true;
    }

    public void DraggedToToyBox()
    {
        Debug.Log("Completed Third Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(false);
        dialogueTrigger4.TriggerDialogue();
    }

    public void FinishedFourthDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
        interactivePanel.transform.Find("Seesaw Arrow 2").localScale = new Vector2(0 - interactivePanel.transform.Find("Seesaw Arrow 2").localScale.x, interactivePanel.transform.Find("Seesaw Arrow 2").localScale.y);
        waitForFourthDrag = true;
    }

    public void DraggedFromToyBox()
    {
        Debug.Log("Completed Fourth Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(false);
        dialogueTrigger5.TriggerDialogue(); 
    }

    public void FinishedFifthDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
        interactivePanel.transform.Find("Seesaw Arrow 2").localScale = new Vector2(0 - interactivePanel.transform.Find("Seesaw Arrow 2").localScale.x, interactivePanel.transform.Find("Seesaw Arrow 2").localScale.y);
        interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(true);
        
        waitForFifthDrag = true;
    }

    public void DraggedCorrectly()
    {
        Debug.Log("Completed Fourth Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(false);
        interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(false);
        dialogueTrigger6.TriggerDialogue();
    }

    public void FinishedSixthDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

/*     // set up the seesaw according to the equation data
    private void SetUpSeesaw()
    {
        Expression lhs = equation.lhs;
        Expression rhs = equation.rhs;
        
        
        if (lhs.numVars > 0)
        {
            for (int i = 0; i < lhs.numVars; i++)
            {
                Transform lhsPositive = seesaw.transform.Find("LHSPositive");
                GameObject newVar = variablePool.GetObject();
                newVar.transform.SetParent(lhsPositive);
                newVar.GetComponent<HasValue>().SetValue(equation.variableValue);
            }
        }
        else if (lhs.numVars < 0)
        {
            for (int i = 0; i < 0 - lhs.numVars; i++)
            {
                Transform lhsNegative = seesaw.transform.Find("LHSNegative");
                GameObject newVar = variablePool.GetObject();
                newVar.transform.SetParent(lhsNegative);
                newVar.GetComponent<HasValue>().SetValue(equation.variableValue);
            }
        }

        if (lhs.numValues > 0)
        {
            for (int i = 0; i < lhs.numValues; i++)
            {
                Transform lhsPositive = seesaw.transform.Find("LHSPositive");
                GameObject newVar = toyPool.GetObject();
                newVar.transform.SetParent(lhsPositive);
            }
        }
        else if (lhs.numValues < 0)
        {
            for (int i = 0; i < 0 - lhs.numValues; i++)
            {
                Transform lhsNegative = seesaw.transform.Find("LHSNegative");
                GameObject newVar = toyPool.GetObject();
                newVar.transform.SetParent(lhsNegative);
            }
        }

        if (rhs.numVars > 0)
        {
            for (int i = 0; i < rhs.numVars; i++)
            {
                Transform rhsPositive = seesaw.transform.Find("RHSPositive");
                GameObject newVar = variablePool.GetObject();
                newVar.transform.SetParent(rhsPositive);
                newVar.GetComponent<HasValue>().SetValue(equation.variableValue);
            }
        }
        else if (rhs.numVars < 0)
        {
            for (int i = 0; i < 0 - rhs.numVars; i++)
            {
                Transform rhsNegative = seesaw.transform.Find("RHSNegative");
                GameObject newVar = variablePool.GetObject();
                newVar.transform.SetParent(rhsNegative);
                newVar.GetComponent<HasValue>().SetValue(equation.variableValue);
            }
        }

        if (rhs.numValues > 0)
        {
            for (int i = 0; i < rhs.numValues; i++)
            {
                Transform rhsPositive = seesaw.transform.Find("RHSPositive");
                GameObject newVar = toyPool.GetObject();
                newVar.transform.SetParent(rhsPositive);
            }
        }
        else if (rhs.numValues < 0)
        {
            for (int i = 0; i < 0 - rhs.numValues; i++)
            {
                Transform rhsNegative = seesaw.transform.Find("RHSNegative");
                GameObject newVar = toyPool.GetObject();
                newVar.transform.SetParent(rhsNegative);
            }
        }
    } */

    /* private void UpdateTimeUsedDisplay()
    {
        timeUsedText.text = "Time Used: " + Mathf.Round(timeUsed).ToString();
    } */



    // end the current round
    /* public void EndRound(string howEnded)
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
            if (seesaw.GetComponent<SeesawController>().CheckIfComplete())
            {
                if (seesaw.GetComponent<SeesawController>().CorrectlyBalanced())
                {
                    finishedDisplayManager.DisplayCorrectlyBalanced(equation.variableValue);
                } 
                else 
                {
                    // lost because wrong answer, get whatever they answered
                    int side = seesaw.GetComponent<SeesawController>().GetLeftHandSideValue();
                    if (equation.variableValue != side)
                    {
                        finishedDisplayManager.DisplayWrongBalanced(side);
                    } else {
                        side = seesaw.GetComponent<SeesawController>().GetRightHandSideValue();
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
    } */

    /* public void FinishedCheck()
    {
        EndRound("Finished Check");
    } */

    
}
