using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Controller for the Game Seesaw
 */
public class T2SeesawController : SeesawController
{
    // variables inherited from SeesawController

    // Start is called before the first frame update
    void Start()
    {
        // set initial tilt to 0
        tilt = 0;
        currentlyDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        // update the seesaw's current tilt
        if (! currentlyDragging)
        {
            UpdateTilt();
            UpdatePositions();
            UpdateCurrentEquation();
        }
    }

    // update the current numerical tilt representing how unbalanced the seesaw is
    public override void UpdateTilt()
    {
        // update current tilt
        double lhs = 0;
        double rhs = 0;

        lhs = leftHandSidePositive.GetComponent<SeesawSide>().TotalNumericalValue() + leftHandSideNegative.GetComponent<SeesawSide>().TotalNumericalValue();
        rhs = rightHandSidePositive.GetComponent<SeesawSide>().TotalNumericalValue() + rightHandSideNegative.GetComponent<SeesawSide>().TotalNumericalValue();

        prevTilt = tilt;
        tilt = lhs - rhs;
        if (tilt < 0.05 && tilt > -0.05)
        {
            // due to floating point arithmetic errors
            tilt = 0;
        }

        if (tilt != prevTilt)
        {
            CheckTilt();
        }
    }

    // get total numerical value of right hand side
    public override double GetRightHandSideValue()
    {
        return rightHandSidePositive.GetComponent<SeesawSide>().TotalNumericalValue() + rightHandSideNegative.GetComponent<SeesawSide>().TotalNumericalValue();
    }

    // get total numerical value of left hand side
    public override double GetLeftHandSideValue()
    {
        return leftHandSidePositive.GetComponent<SeesawSide>().TotalNumericalValue() + leftHandSideNegative.GetComponent<SeesawSide>().TotalNumericalValue();
    }

    // check if a variable is correctly isolated
    public override bool CheckIfComplete()
    {
        // check if there is only 1 variable on the left hand side with coefficient 1
        // and only one value on the other
        if (leftHandSidePositive.transform.childCount == 1 && leftHandSidePositive.transform.GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable && leftHandSideNegative.transform.childCount == 0)
        {
            if (leftHandSidePositive.transform.GetChild(0).Find("Coefficient").gameObject.GetComponent<Coefficient>().GetValue() == 1)
            {
                if (rightHandSidePositive.transform.childCount == 1 && rightHandSidePositive.transform.GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value && rightHandSideNegative.transform.childCount == 0)
                {
                    return true;
                }
                else if (rightHandSideNegative.transform.childCount == 1 && rightHandSideNegative.transform.GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value && rightHandSidePositive.transform.childCount == 0)
                {
                    return true;
                }
            }    
        }

        if (rightHandSidePositive.transform.childCount == 1 && rightHandSidePositive.transform.GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable && rightHandSideNegative.transform.childCount == 0)
        {
            if (rightHandSidePositive.transform.GetChild(0).Find("Coefficient").gameObject.GetComponent<Coefficient>().GetValue() == 1)
            {
                if (leftHandSidePositive.transform.childCount == 1 && leftHandSidePositive.transform.GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value && leftHandSideNegative.transform.childCount == 0)
                {
                    return true;
                }
                else if (leftHandSideNegative.transform.childCount == 1 && leftHandSideNegative.transform.GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value && leftHandSidePositive.transform.childCount == 0)
                {
                    return true;
                }
            }    
        }

        return false;
    }

    public void AddBothSides(int num)
    {
        if (leftHandSidePositive.GetComponent<SeesawSide>().CheckOverCapacity() || rightHandSidePositive.GetComponent<SeesawSide>().CheckOverCapacity())
        {
            leftHandSidePositive.GetComponent<SeesawSide>().OverCapacity();
        }
        else
        {        
            // TODO: set parent to return to

            GameObject newObject = toyPool.GetObject();
            newObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(num);
            newObject.transform.SetParent(leftHandSidePositive.transform);
            newObject.GetComponent<Draggable>().ShowOnPositiveSide();
            newObject.GetComponent<Draggable>().parentToReturnTo = leftHandSidePositive.transform;

            GameObject new2Object = toyPool.GetObject();
            new2Object.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(num);
            new2Object.transform.SetParent(rightHandSidePositive.transform);
            new2Object.GetComponent<Draggable>().ShowOnPositiveSide();
            new2Object.GetComponent<Draggable>().parentToReturnTo = rightHandSidePositive.transform;
        }
    }

    public void SubtractBothSides(int num)
    {
        if (leftHandSideNegative.GetComponent<SeesawSide>().CheckOverCapacity() || rightHandSideNegative.GetComponent<SeesawSide>().CheckOverCapacity())
        {
            leftHandSideNegative.GetComponent<SeesawSide>().OverCapacity();
        }
        else
        {        
            //TODO: set parent to return to

            GameObject newObject = toyPool.GetObject();
            newObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(0 - num);
            newObject.transform.SetParent(leftHandSideNegative.transform);
            newObject.GetComponent<Draggable>().ShowOnNegativeSide();
            newObject.GetComponent<Draggable>().parentToReturnTo = leftHandSideNegative.transform;

            GameObject new2Object = toyPool.GetObject();
            new2Object.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(0 - num);
            new2Object.transform.SetParent(rightHandSideNegative.transform);
            new2Object.GetComponent<Draggable>().ShowOnNegativeSide();
            new2Object.GetComponent<Draggable>().parentToReturnTo = rightHandSideNegative.transform;
        }
    }

    public void MultiplyBothSides(int num)
    {
        foreach(Transform child in leftHandSidePositive.transform)
        {
            Fraction value = child.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = num * value;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in leftHandSideNegative.transform)
        {
            Fraction value = child.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = num * value;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in rightHandSidePositive.transform)
        {
            Fraction value = child.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = num * value;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in rightHandSideNegative.transform)
        {
            Fraction value = child.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = num * value;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(newValue);
        }
    }

    public void DivideBothSides(int num)
    {
        Debug.Log("Reached seesaw controller");

        foreach(Transform child in leftHandSidePositive.transform)
        {
            Fraction value = child.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = value / num;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in leftHandSideNegative.transform)
        {
            Fraction value = child.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = value / num;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in rightHandSidePositive.transform)
        {
            Fraction value = child.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = value / num;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in rightHandSideNegative.transform)
        {
            Fraction value = child.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = value / num;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").GetComponent<Coefficient>().SetValue(newValue);
        }
    }

    public override void UpdateCurrentEquation()
    {
        string lside = "";
        string rside = "";

        foreach(Transform child in leftHandSidePositive.transform)
        {
            if (lside.Length > 0)
            {
                lside = lside + " + ";
            }
            lside = lside + StringTerm(child);
        }

        foreach(Transform child in leftHandSideNegative.transform)
        {
            if (lside.Length > 0)
            {
                lside = lside + " - ";
            }
            else {
                lside = lside + "-";
            }
            lside = lside + StringTerm(child);
        }

        if (lside.Length == 0)
        {
            lside = lside + "0";
        }

        foreach(Transform child in rightHandSidePositive.transform)
        {
            if (rside.Length > 0)
            {
                rside = rside + " + ";
            }
            rside = rside + StringTerm(child);
        }

        foreach(Transform child in rightHandSideNegative.transform)
        {
            if (rside.Length > 0)
            {
                rside = rside + " - ";
            }
            else {
                rside = rside + "-";
            }
            rside = rside + StringTerm(child);
        }

        if (rside.Length == 0)
        {
            rside = rside + "0";
        }

        string equation = "";
        if (tilt == 0)
        {
            equation = lside + " = " + rside;
        }
        else if (tilt > 0)
        {
            equation = lside + " > " + rside;
        }
        else if (tilt < 0)
        {
            equation = lside + " < " + rside;
        }

        equationText.text = equation;
    }

    public string StringTerm(Transform term)
    {
        string termString = "";

        if (term.Find("Coefficient") != null)
        {
            Fraction value = term.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();   
            
            if (value < 0)
            {
                value = -value;
            }

            if (term.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable)
            {
                if (value != 1)
                {
                    termString = termString + value.ToString();
                }
                termString = termString + "x";
            }
            else if (term.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value)
            {
                termString = termString + value.ToString();
            }
            else if (term.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Bracket)
            {
                if (value != 1)
                {
                    termString = termString + value.ToString();
                }
                termString = termString + "(";

                string bracket = "";
                foreach(Transform kid in term.Find("TermsInBracket"))
                {
                    Fraction val = kid.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();

                    if (bracket.Length > 0)
                    {
                        bracket = bracket + " + ";
                    }

                    if (kid.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable)
                    {
                        if (val != 1)
                        {
                            bracket = bracket + val.ToString();
                        }
                        bracket = bracket + "x";
                    }
                    else if (kid.gameObject.GetComponent<Draggable>().typeOfItem == Draggable.Slot.Value)
                    {
                        bracket = bracket + val.ToString();
                    }
                }
                termString = termString + bracket + ")";
            }

            return termString;
        }
        else
        {
            return "";
        }

        
    }
}
