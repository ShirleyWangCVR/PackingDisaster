using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Game Controller for the main scene where the question is solved.
 */
public class T2GameController : MonoBehaviour
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
    private int playerScore; // currently not being used
    private bool dialogueActive;

    // Start is called before the first frame update
    void Start()
    {
        // get data from dataController
        dataController = FindObjectOfType<DataController>();
        equation = dataController.GetCurrentEquationData();
        difficultyLevel = dataController.GetDifficulty();
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
        else if (difficultyLevel == 8)
        {
            isRoundActive = true;
            dialogueActive = false;
        }
    }

    // set up the seesaw according to the equation data
    private void SetUpSeesaw()
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

    private void SetUpCoefficient(SimpleObjectPool pool, Transform side, int number, bool isVar)
    {
        // Transform rhsPositive = seesaw.transform.Find("RHSPositive");
        GameObject newVar = pool.GetObject();
        newVar.transform.SetParent(side);

        if (isVar)
        {
            newVar.GetComponent<HasValue>().SetValue(equation.variableValue);
        }

        // currently defaulting initial value is whole number
        newVar.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetIntValue(number);
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
        if (seesaw.GetComponent<T2SeesawController>().FellOver())
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
}
