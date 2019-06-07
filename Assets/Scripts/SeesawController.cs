using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Controller for the game seesaw.
 * Used for type 1 questions.
 */
public class SeesawController : MonoBehaviour
{
    public GameObject leftHandSidePositive;
    public GameObject rightHandSidePositive;
    public GameObject leftHandSideNegative;
    public GameObject rightHandSideNegative;
    public SimpleObjectPool toyPool;
    public SimpleObjectPool variablePool;

    protected bool currentlyDragging;
    protected double tilt;

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

    public void SetDragging(bool dragging)
    {
        currentlyDragging = dragging;
    }

    // make the seesaw tilt if it needs to
    protected virtual void UpdatePositions()
    {
        // tilt seesaw ominously
        float currangle = this.transform.rotation.eulerAngles.z;
        if (currangle > 180)
        {
            currangle = this.transform.rotation.eulerAngles.z - 360;
        }

        if (tilt > 0)
        {
            this.transform.Rotate(0, 0, 0.05f, Space.Self);
        }
        else if (tilt < 0)
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
    public virtual void UpdateTilt()
    {
        // update current tilt
        int lhs = 0;
        int rhs = 0;

        foreach(Transform child in leftHandSidePositive.transform)
        {
            lhs = lhs + child.gameObject.GetComponent<HasValue>().GetValue();
        }

        foreach(Transform child in leftHandSideNegative.transform)
        {
            lhs = lhs - child.gameObject.GetComponent<HasValue>().GetValue();
        }

        foreach(Transform child in rightHandSidePositive.transform)
        {
            rhs = rhs + child.gameObject.GetComponent<HasValue>().GetValue();
        }

        foreach(Transform child in rightHandSideNegative.transform)
        {
            rhs = rhs - child.gameObject.GetComponent<HasValue>().GetValue();
        }

        tilt = lhs - rhs;
    }

    // in case tilt isn't working debug this by invokerepeating in start
    public void DebugTilt()
    {
        float currangle = this.transform.rotation.eulerAngles.z;
        if (currangle > 180)
        {
            currangle = this.transform.rotation.eulerAngles.z - 360;
        }
        Debug.Log(currangle);
        Debug.Log(tilt);
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
            return rightHandSidePositive.GetComponent<PositiveSide>().NumVariables() == 0 && rightHandSideNegative.GetComponent<NegativeSide>().NumVariables() == 0;
        }

        if (rightHandSidePositive.transform.childCount == 1 && rightHandSidePositive.transform.GetChild(0).GetComponent<HasValue>().typeOfItem == Draggable.Slot.Variable && rightHandSideNegative.transform.childCount == 0)
        {
            return leftHandSidePositive.GetComponent<PositiveSide>().NumVariables() == 0 && leftHandSideNegative.GetComponent<NegativeSide>().NumVariables() == 0;
        }

        return false;
    }

    // check if both sides of equation are equal
    public bool CorrectlyBalanced()
    {
        return tilt == 0;
    }

    // get total numerical value of right hand side
    public virtual double GetRightHandSideValue()
    {
        return rightHandSidePositive.GetComponent<PositiveSide>().TotalNumericalValue() - rightHandSideNegative.GetComponent<NegativeSide>().TotalNumericalValue();
    }

    // get total numerical value of left hand side
    public virtual double GetLeftHandSideValue()
    {
        return leftHandSidePositive.GetComponent<PositiveSide>().TotalNumericalValue() - leftHandSideNegative.GetComponent<NegativeSide>().TotalNumericalValue();
    }

}
