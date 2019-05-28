using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{    
    public Text timeLeftText;
    public GameObject seesaw;
    public SimpleObjectPool variablePool;
    public SimpleObjectPool toyPool;
    // public GameObject finishedDisplay;
    public FinishedPanelManager finishedDisplayManager;

    private DataController equationController;
    private EquationData equation; // current equation being displayed
    
    private bool isRoundActive; 
    private float timeRemaining;
    private int difficultyLevel; // difficulty levels 0 to 5? 1 to 5? 0 being tutorial?
    private int equationsCompleted;
    private int playerScore;
    private List<GameObject> allValuesInPlay = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        equationController = FindObjectOfType<DataController>();
        equation = equationController.GetCurrentEquationData(0);
        difficultyLevel = equationController.GetDifficulty();
        equationsCompleted = equationController.GetEquationsCompleted();
        timeRemaining = equation.timeLimit;
        
        // set up seesaw according to equation
        SetUpSeesaw();
        timeLeftText.text = "Time Left: " + timeRemaining.ToString();

        // if we have tutorials then this isn't active just yet
        isRoundActive = true;
    }

    private void SetUpSeesaw()
    {
        RemovePreviousValues();

        if (equation.lhsVars > 0)
        {
            for (int i = 0; i < equation.lhsVars; i++)
            {
                Transform lhsPositive = seesaw.transform.Find("LHSPositive");
                GameObject newVar = variablePool.GetObject();
                newVar.transform.SetParent(lhsPositive);
                HasValue value = newVar.GetComponent<HasValue>();
                value.SetValue(equation.variableValue);
            }
        }
        else if (equation.lhsVars < 0)
        {
            for (int i = 0; i < 0 - equation.lhsVars; i++)
            {
                Transform lhsNegative = seesaw.transform.Find("LHSNegative");
                GameObject newVar = variablePool.GetObject();
                newVar.transform.SetParent(lhsNegative);
                newVar.GetComponent<HasValue>().SetValue(equation.variableValue);
                HasValue value = newVar.GetComponent<HasValue>();
                value.SetValue(equation.variableValue);
            }
        }

        if (equation.lhsValues > 0)
        {
            for (int i = 0; i < equation.lhsValues; i++)
            {
                Transform lhsPositive = seesaw.transform.Find("LHSPositive");
                GameObject newVar = toyPool.GetObject();
                newVar.transform.SetParent(lhsPositive);
            }
        }
        else if (equation.lhsValues < 0)
        {
            for (int i = 0; i < 0 - equation.lhsValues; i++)
            {
                Transform lhsNegative = seesaw.transform.Find("LHSNegative");
                GameObject newVar = toyPool.GetObject();
                newVar.transform.SetParent(lhsNegative);
            }
        }

        if (equation.rhsVars > 0)
        {
            for (int i = 0; i < equation.rhsVars; i++)
            {
                Transform rhsPositive = seesaw.transform.Find("RHSPositive");
                GameObject newVar = variablePool.GetObject();
                newVar.transform.SetParent(rhsPositive);
                newVar.GetComponent<HasValue>().SetValue(equation.variableValue);
                HasValue value = newVar.GetComponent<HasValue>();
                value.SetValue(equation.variableValue);
            }
        }
        else if (equation.rhsVars < 0)
        {
            for (int i = 0; i < 0 - equation.rhsVars; i++)
            {
                Transform rhsNegative = seesaw.transform.Find("RHSNegative");
                GameObject newVar = variablePool.GetObject();
                newVar.transform.SetParent(rhsNegative);
                newVar.GetComponent<HasValue>().SetValue(equation.variableValue);
                HasValue value = newVar.GetComponent<HasValue>();
                value.SetValue(equation.variableValue);
            }
        }

        if (equation.rhsValues > 0)
        {
            for (int i = 0; i < equation.rhsValues; i++)
            {
                Transform rhsPositive = seesaw.transform.Find("RHSPositive");
                GameObject newVar = toyPool.GetObject();
                newVar.transform.SetParent(rhsPositive);
            }
        }
        else if (equation.rhsValues < 0)
        {
            for (int i = 0; i < 0 - equation.rhsValues; i++)
            {
                Transform rhsNegative = seesaw.transform.Find("RHSNegative");
                GameObject newVar = toyPool.GetObject();
                newVar.transform.SetParent(rhsNegative);
            }
        }
    }

    private void UpdateTimeRemainingDisplay()
    {
        timeLeftText.text = "Time Left: " + Mathf.Round(timeRemaining).ToString();
    }

    private void RemovePreviousValues()
    {
        seesaw.GetComponent<SeesawController>().ClearAllValues();
        // also double check canvas, toybox, and boxpile
    }

    // Update is called once per frame
    void Update()
    {
        if (isRoundActive) 
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();

            if (timeRemaining <= 0f)
            {
                EndRound("Time Out");
            }
        }

        if (seesaw.GetComponent<SeesawController>().FellOver())
        {
            EndRound("Scale Tipped");
        }
    }

    public void EndRound(string howEnded)
    {
        isRoundActive = false;
        playerScore = (int) Mathf.Round(timeRemaining);
        equationController.SubmitNewPlayerScore(playerScore);
        int highestScore = equationController.GetHighestPlayerScore();

        if (howEnded == "Time Out")
        {
            finishedDisplayManager.DisplayTimeOut();
        } 
        else if (howEnded == "Finished Check") 
        {
            if (seesaw.GetComponent<SeesawController>().CheckIfComplete())
            {
                if (seesaw.GetComponent<SeesawController>().CorrectlyBalanced(equation.variableValue))
                {
                    finishedDisplayManager.DisplayCorrectlyBalanced(equation.variableValue, playerScore);
                } else {
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
}
