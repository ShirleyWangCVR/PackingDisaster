using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Controller for the Game Seesaw
 */
public class T2SeesawController : MonoBehaviour
{
    public GameObject leftHandSidePositive;
    public GameObject rightHandSidePositive;
    public GameObject leftHandSideNegative;
    public GameObject rightHandSideNegative;
    public SimpleObjectPool toyPool;
    public SimpleObjectPool variablePool;

    private double tilt;

    // Start is called before the first frame update
    void Start()
    {
        // set initial tilt to 0
        tilt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // update the seesaw's current tilt
        UpdateTilt();
        UpdatePositions();

    }

    // make the seesaw tilt if it needs to
    void UpdatePositions()
    {
        // tilt seesaw ominously
        float currangle = this.transform.rotation.eulerAngles.z;
        if (currangle > 180)
        {
            currangle = this.transform.rotation.eulerAngles.z - 360;
        }

        if (tilt > 0.05)
        {
            this.transform.Rotate(0, 0, 0.05f, Space.Self);
        }
        else if (tilt < 0 - 0.05)
        {
            this.transform.Rotate(0, 0, -0.05f, Space.Self);
        }
        else
        {   // tilt == 0
            // Unity doesn't move it by exact values so give it a slight bit of wiggle room when
            // returning to horizontal
            if (currangle > 0.05 || currangle < -0.05)
            {
                if (this.transform.rotation.eulerAngles.z < 180)
                {
                    this.transform.Rotate(0, 0, -0.1f, Space.Self);
                } else
                {
                    this.transform.Rotate(0, 0, 0.1f, Space.Self);
                }
            }
        }
    }

    // update the current numerical tilt representing how unbalanced the seesaw is
    void UpdateTilt()
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

    // in case tilt isn't working debug this by invokerepeating in start
    void DebugTilt()
    {
        float currangle = this.transform.rotation.eulerAngles.z;
        if (currangle > 180)
        {
            currangle = this.transform.rotation.eulerAngles.z - 360;
        }
        Debug.Log(currangle);
        Debug.Log(this.transform.localRotation);
    }

    // if it's tipped over more than 40 then the seesaw it too tipped over and they lose
    public bool FellOver()
    {
        float currangle = this.transform.rotation.eulerAngles.z;
        if (currangle > 180)
        {
            currangle = 360 - this.transform.rotation.eulerAngles.z;
        }

        return currangle > 40;
    }

    // check if a variable is correctly isolated
    public bool CheckIfComplete()
    {
        // check if there is only 1 variable on the left hand side
        if (leftHandSidePositive.transform.childCount == 1 && leftHandSidePositive.transform.GetChild(0).GetComponent<HasValue>().typeOfItem == Draggable.Slot.Variable && leftHandSideNegative.transform.childCount == 0)
        {
            return rightHandSidePositive.GetComponent<T2PositiveSide>().NumVariables() == 0 && rightHandSideNegative.GetComponent<T2NegativeSide>().NumVariables() == 0;
        }

        if (rightHandSidePositive.transform.childCount == 1 && rightHandSidePositive.transform.GetChild(0).GetComponent<HasValue>().typeOfItem == Draggable.Slot.Variable && rightHandSideNegative.transform.childCount == 0)
        {
            return leftHandSidePositive.GetComponent<T2PositiveSide>().NumVariables() == 0 && leftHandSideNegative.GetComponent<T2NegativeSide>().NumVariables() == 0;
        }

        return false;
    }

    // check if both sides of equation are equal
    public bool CorrectlyBalanced()
    {
        return tilt == 0;
    }

    // get total numerical value of right hand side
    public double GetRightHandSideValue()
    {
        return rightHandSidePositive.GetComponent<T2PositiveSide>().TotalNumericalValue() - rightHandSideNegative.GetComponent<T2NegativeSide>().TotalNumericalValue();
    }

    // get total numerical value of left hand side
    public double GetLeftHandSideValue()
    {
        return leftHandSidePositive.GetComponent<T2PositiveSide>().TotalNumericalValue() - leftHandSideNegative.GetComponent<T2NegativeSide>().TotalNumericalValue();
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
