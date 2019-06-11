using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Game Controller for the main scene where the question is solved.
 */
public class T3GameController : GameController
{    
    public GameObject bracketPrefab;
    // other variables inherited from GameController

    // Start is called before the first frame update
    void Start()
    {
        // get data from dataController
        dataController = FindObjectOfType<DataController>();
        level = dataController.GetDifficulty();
        equation = dataController.GetCurrentEquationData(level);
        levelText.text = "Level " + level.ToString();
        timeUsed = 0;
        currentlyDragging = false;
        isRoundActive = true;
        
        // set up seesaw according to equation
        SetUpSeesaw();
        timeUsedText.text = "Time Used: " + timeUsed.ToString();
    }

    // set up the seesaw according to the equation data
    // probably create another method to make this less lengthy at some point
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
        if (seesaw.GetComponent<T3SeesawController>().FellOver())
        {
            EndRound("Scale Tipped");
        }
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
