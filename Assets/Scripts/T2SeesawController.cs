using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    
    
    // change to player has to cancel it out themselves
    
    // if there are values to be cancelled out on either side then cancel them out
    /* public void CancelOutValues()
    {
        if (leftHandSidePositive.transform.childCount > 0 && leftHandSideNegative.transform.childCount > 0)
        {
            if (leftHandSidePositive.GetComponent<PositiveSide>().NumVariables() > 0 && leftHandSideNegative.GetComponent<NegativeSide>().NumVariables() > 0)
            {
                CancelOutSide(leftHandSidePositive, leftHandSideNegative, Draggable.Slot.Variable, variablePool);
            }
            if (leftHandSidePositive.GetComponent<PositiveSide>().NumValues() > 0 && leftHandSideNegative.GetComponent<NegativeSide>().NumValues() > 0)
            {
                CancelOutSide(leftHandSidePositive, leftHandSideNegative, Draggable.Slot.Value, toyPool);
            }
        }

        if (rightHandSidePositive.transform.childCount > 0 && rightHandSideNegative.transform.childCount > 0)
        {
            if (rightHandSidePositive.GetComponent<PositiveSide>().NumVariables() > 0 && rightHandSideNegative.GetComponent<NegativeSide>().NumVariables() > 0)
            {
                CancelOutSide(rightHandSidePositive, rightHandSideNegative, Draggable.Slot.Variable, variablePool);
            }
            if (rightHandSidePositive.GetComponent<PositiveSide>().NumValues() > 0 && rightHandSideNegative.GetComponent<NegativeSide>().NumValues() > 0)
            {
                CancelOutSide(rightHandSidePositive, rightHandSideNegative, Draggable.Slot.Value, toyPool);
            }
        }
    } */

    // cancels out from the positive and negative side a certain type of value
    /* private void CancelOutSide(GameObject positiveSide, GameObject negativeSide, Draggable.Slot slot, SimpleObjectPool pool)
    {
        GameObject top = null;
        GameObject bottom = null;

        int num = Mathf.Min(positiveSide.GetComponent<PositiveSide>().NumValues(), negativeSide.GetComponent<NegativeSide>().NumValues());
        for (int i = 0; i < num; i++)
        {
            foreach(Transform child in positiveSide.transform)
            {
                if (child.GetComponent<HasValue>().typeOfItem == slot)
                {
                    top = child.gameObject;
                    break;
                }
            }
            foreach(Transform child in negativeSide.transform)
            {
                if (child.GetComponent<HasValue>().typeOfItem == slot)
                {
                    bottom = child.gameObject;
                    break;
                }
            }
            pool.ReturnObject(top);
            pool.ReturnObject(bottom);
        }
    } */

    public void AddBothSides(int num)
    {

    }

    public void SubtractBothSides(int num)
    {

    }

    public void MultiplyBothSides(int num)
    {

    }

    public void DivideBothSides(int num)
    {
        
    }
}
