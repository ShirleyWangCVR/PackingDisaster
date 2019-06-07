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
        }
    }

    // update the current numerical tilt representing how unbalanced the seesaw is
    public override void UpdateTilt()
    {
        // update current tilt
        double lhs = 0;
        double rhs = 0;

        lhs = leftHandSidePositive.GetComponent<T2PositiveSide>().TotalNumericalValue();
        lhs = lhs + leftHandSideNegative.GetComponent<T2NegativeSide>().TotalNumericalValue();

        rhs = rightHandSidePositive.GetComponent<T2PositiveSide>().TotalNumericalValue();
        rhs = rhs + rightHandSideNegative.GetComponent<T2NegativeSide>().TotalNumericalValue();

        tilt = lhs - rhs;
    }

    // get total numerical value of right hand side
    public override double GetRightHandSideValue()
    {
        return rightHandSidePositive.GetComponent<T2PositiveSide>().TotalNumericalValue() + rightHandSideNegative.GetComponent<T2NegativeSide>().TotalNumericalValue();
    }

    // get total numerical value of left hand side
    public override double GetLeftHandSideValue()
    {
        return leftHandSidePositive.GetComponent<T2PositiveSide>().TotalNumericalValue() + leftHandSideNegative.GetComponent<T2NegativeSide>().TotalNumericalValue();
    }

    public void AddBothSides(int num)
    {
        GameObject newObject = toyPool.GetObject();
        newObject.transform.Find("Coefficient").GetComponent<Coefficient>().SetValue(num);
        newObject.transform.SetParent(leftHandSidePositive.transform);

        GameObject new2Object = toyPool.GetObject();
        new2Object.transform.Find("Coefficient").GetComponent<Coefficient>().SetValue(num);
        new2Object.transform.SetParent(rightHandSidePositive.transform);
    }

    public void SubtractBothSides(int num)
    {
        GameObject newObject = toyPool.GetObject();
        newObject.transform.Find("Coefficient").GetComponent<Coefficient>().SetValue(0 - num);
        newObject.transform.SetParent(leftHandSideNegative.transform);
        newObject.transform.Find("Image").localScale = new Vector3(-1, -1, 1);
        newObject.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 255);

        GameObject new2Object = toyPool.GetObject();
        new2Object.transform.Find("Coefficient").GetComponent<Coefficient>().SetValue(0 - num);
        new2Object.transform.SetParent(rightHandSideNegative.transform);
        new2Object.transform.Find("Image").localScale = new Vector3(-1, -1, 1);
        new2Object.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
    }

    public void MultiplyBothSides(int num)
    {
        foreach(Transform child in leftHandSidePositive.transform)
        {
            Fraction value = child.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = num * value;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in leftHandSideNegative.transform)
        {
            Fraction value = child.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = num * value;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in rightHandSidePositive.transform)
        {
            Fraction value = child.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = num * value;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").GetComponent<Coefficient>().SetValue(newValue);
        }

        foreach(Transform child in rightHandSideNegative.transform)
        {
            Fraction value = child.Find("Coefficient").GetComponent<Coefficient>().GetFractionValue();
            Fraction newValue = num * value;
            Fraction.ReduceFraction(newValue);
            child.Find("Coefficient").GetComponent<Coefficient>().SetValue(newValue);
        }
    }

    public void DivideBothSides(int num)
    {
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
}
