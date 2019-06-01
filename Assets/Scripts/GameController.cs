using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Game Controller for the main scene where the question is solved.
 */
public class GameController : MonoBehaviour
{    
    public Text timeUsedText;
    public GameObject seesaw;
    public SimpleObjectPool variablePool;
    public SimpleObjectPool toyPool;
    public FinishedPanelManager finishedDisplayManager;
    public DialogueController dialogueController;

    private DataController dataController;
    private EquationData equation; // current equation being displayed
    
    private bool isRoundActive; 
    private float timeUsed;
    private int difficultyLevel; // difficulty levels 0 to 5? 1 to 5? 0 being tutorial?
    private int equationsCompleted; // currently not being used
    private int playerScore; // currently not being used
    private bool dialogueActive;
    private bool currentlyDragging;

    // Start is called before the first frame update
    void Start()
    {
        // get data from dataController
        dataController = FindObjectOfType<DataController>();
        equation = dataController.GetCurrentEquationData();
        difficultyLevel = dataController.GetDifficulty();
        equationsCompleted = dataController.GetEquationsCompleted();
        timeUsed = 0;
        
        // set up seesaw according to equation
        SetUpSeesaw();
        timeUsedText.text = "Time Used: " + timeUsed.ToString();

        // set up tutorial dialogue
        if (difficultyLevel == 0)
        {
            isRoundActive = false;
            dialogueActive = true;
            dialogueController.ExecuteTutorialDialogue();
        }
    }

    // set up the seesaw according to the equation data
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
    }

    private void UpdateTimeUsedDisplay()
    {
        timeUsedText.text = "Time Used: " + Mathf.Round(timeUsed).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // if round active count down time display
        if (isRoundActive) 
        {
            timeUsed += Time.deltaTime;
            UpdateTimeUsedDisplay();
        }

        // if seesaw fell over end game
        if (seesaw.GetComponent<SeesawController>().FellOver())
        {
            EndRound("Scale Tipped");
        }

        // if dialogue currently active check until dialogue no longer active
        if (dialogueActive)
        {
            isRoundActive = dialogueController.FinishedDialogue();
            if (isRoundActive)
            {
                dialogueActive = false;
            }
        }
    }

    // end the current round
    public void EndRound(string howEnded)
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
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void FinishedCheck()
    {
        EndRound("Finished Check");
    }

    public void SetDragging(bool dragging)
    {
        currentlyDragging = dragging;
        seesaw.GetComponent<SeesawController>().SetDragging(dragging);
    }
}
