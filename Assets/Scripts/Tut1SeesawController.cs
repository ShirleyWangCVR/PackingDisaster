using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Controller for the Game Seesaw
 */
public class Tut1SeesawController : SeesawController
{
    // public GameObject leftHandSidePositive;
    // public GameObject rightHandSidePositive;
    // public GameObject leftHandSideNegative;
    // public GameObject rightHandSideNegative;
    // public SimpleObjectPool toyPool;
    // public SimpleObjectPool variablePool;

    private int tilt;
    private float nextTime = 0;
    // private bool currentlyDragging;

    // Start is called before the first frame update
    void Start()
    {
        // set initial tilt to 0
        currentlyDragging = false;
        tilt = 0;
        // InvokeRepeating("DebugTilt", 0, 3.0f);
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
    void UpdatePositions()
    {
        // tilt seesaw ominously
        float currangle = this.transform.rotation.eulerAngles.z;
        if (currangle > 180)
        {
            currangle = this.transform.rotation.eulerAngles.z - 360;
        }

        if (tilt > 0)
        {
            this.transform.Rotate(0, 0, 0.01f, Space.Self);
        }
        else if (tilt < 0)
        {
            this.transform.Rotate(0, 0, -0.01f, Space.Self);
        }
        else
        {   // tilt == 0
            // Unity doesn't move it by exact values so give it a slight bit of wiggle room when
            // returning to horizontal
            if (currangle >= 0.03 || currangle <= -0.03)
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

    public bool CheckOneDraggedUnbalanced()
    {
        // this assumes that they're dragging a toy fix this
        return (tilt == 2 || tilt == -2) && currentlyDragging == false;
    }

    public bool CheckAnotherDraggedBalanced()
    {
        return (tilt == 0 && currentlyDragging == false);
    }

    public bool CheckDraggedToToyBoxUnBalanced()
    {
        return (tilt == 1 || tilt == -1) && currentlyDragging == false;
    }

    public bool CheckDraggedFromToyBoxBalanced()
    {
        // not very thorough checking
        return (tilt == 0 && currentlyDragging == false);
    }

    public bool CheckDraggedStillBalanced()
    {
        return leftHandSidePositive.GetComponent<PositiveSide>().NumVariables() == 1 && leftHandSidePositive.GetComponent<PositiveSide>().NumValues() == 1 && rightHandSidePositive.GetComponent<PositiveSide>().NumValues() == 4 && tilt == 0 && ! currentlyDragging;
    }

    // update the current numerical tilt representing how unbalanced the seesaw is
    void UpdateTilt()
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
    void DebugTilt()
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
    public int GetRightHandSideValue()
    {
        int rhs = 0;
        foreach(Transform child in rightHandSidePositive.transform)
            {
                rhs = rhs + child.gameObject.GetComponent<HasValue>().GetValue();

            }

        foreach(Transform child in rightHandSideNegative.transform)
            {
                rhs = rhs - child.gameObject.GetComponent<HasValue>().GetValue();
            }

        return rhs;
    }

    // get total numerical value of left hand side
    public int GetLeftHandSideValue()
    {
        int lhs = 0;
        foreach(Transform child in leftHandSidePositive.transform)
            {
                lhs = lhs + child.gameObject.GetComponent<HasValue>().GetValue();

            }

        foreach(Transform child in leftHandSideNegative.transform)
            {
                lhs = lhs - child.gameObject.GetComponent<HasValue>().GetValue();
            }

        return lhs;
    }
/* 
    // if there are values to be cancelled out on either side then cancel them out
    public void CancelOutValues()
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
    }

    // cancels out from the positive and negative side a certain type of value
    private void CancelOutSide(GameObject positiveSide, GameObject negativeSide, Draggable.Slot slot, SimpleObjectPool pool)
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
}
