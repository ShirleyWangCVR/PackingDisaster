using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeesawController : MonoBehaviour
{
    
    public GameObject leftHandSidePositive;
    public GameObject rightHandSidePositive;
    public GameObject leftHandSideNegative;
    public GameObject rightHandSideNegative;
    public SimpleObjectPool toyPool;
    public SimpleObjectPool variablePool;
    
    private int tilt;
    private float degreetilt = 5f; // tilt by 5 for every 1 over
    private int interval = 5; 
    private float nextTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        tilt = 0;
        // InvokeRepeating("CancelOutValues", 0, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTilt();
        UpdatePositions();

        
        if (Time.time >= nextTime) {
            // check if any positive and negative values cancel each other out.
            CancelOutValues();
            nextTime += interval; 
            Debug.Log("Check for Cancel out");
        }
        
    }

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
            if (currangle < tilt * degreetilt + 0.03)
            {
                this.transform.Rotate(0, 0, 0.05f, Space.Self);
            }
            else if (currangle > tilt * degreetilt + 0.03)
            {
                this.transform.Rotate(0, 0, -0.05f, Space.Self);
            }

        }
        else if (tilt < 0)
        {
            if (currangle > tilt * degreetilt + 0.03)
            {
                this.transform.Rotate(0, 0, -0.05f, Space.Self);
            }
            else if (currangle < tilt * degreetilt + 0.03)
            {
                this.transform.Rotate(0, 0, 0.05f, Space.Self);
            }
        } else { // tilt == 0
            // Unity doesn't move it by exact values so give it a slight bit of wiggle room when
            // returning to horizontal
            if (currangle > 0.03 || currangle < -0.03)
            {
                if (this.transform.rotation.eulerAngles.z < 180)
                {
                    this.transform.Rotate(0, 0, -0.05f, Space.Self);
                } else {
                    this.transform.Rotate(0, 0, 0.05f, Space.Self);
                }
            }
        }
    
    }

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

    public void ClearAllValues()
    {
        foreach(Transform child in leftHandSidePositive.transform)
        {
            if (child.gameObject.GetComponent<HasValue>().typeOfItem == Draggable.Slot.Value)
            {
                toyPool.ReturnObject(child.gameObject);
            }
           else if (child.gameObject.GetComponent<HasValue>().typeOfItem == Draggable.Slot.Variable)
            {
                variablePool.ReturnObject(child.gameObject);
            }
        }

        foreach(Transform child in leftHandSideNegative.transform)
        {
            if (child.gameObject.GetComponent<HasValue>().typeOfItem == Draggable.Slot.Value)
            {
                toyPool.ReturnObject(child.gameObject);
            }
           else if (child.gameObject.GetComponent<HasValue>().typeOfItem == Draggable.Slot.Variable)
            {
                variablePool.ReturnObject(child.gameObject);
            }
        }

        foreach(Transform child in rightHandSidePositive.transform)
        {
            if (child.gameObject.GetComponent<HasValue>().typeOfItem == Draggable.Slot.Value)
            {
                toyPool.ReturnObject(child.gameObject);
            }
           else if (child.gameObject.GetComponent<HasValue>().typeOfItem == Draggable.Slot.Variable)
            {
                variablePool.ReturnObject(child.gameObject);
            }
        }

        foreach(Transform child in rightHandSideNegative.transform)
        {
            if (child.gameObject.GetComponent<HasValue>().typeOfItem == Draggable.Slot.Value)
            {
                toyPool.ReturnObject(child.gameObject);
            }
           else if (child.gameObject.GetComponent<HasValue>().typeOfItem == Draggable.Slot.Variable)
            {
                variablePool.ReturnObject(child.gameObject);
            }
        }
    }

    private double DegreeToRadian(double angle)
    {
    return Mathf.PI * angle / 180.0;
    }

    void DebugTilt()
    {
        float currangle = this.transform.rotation.eulerAngles.z;
        if (currangle > 180)
        {
            currangle = this.transform.rotation.eulerAngles.z - 360;
        }
        Debug.Log(currangle);
        Debug.Log(leftHandSideNegative.transform.localRotation);
        Debug.Log(this.transform.localRotation);
        Debug.Log(tilt * degreetilt);
    }

    // if it's tipped over more than 45 then the seesaw it too tipped over and they lose
    public bool FellOver()
    {
        float currangle = this.transform.rotation.eulerAngles.z;
        if (currangle > 180)
        {
            currangle = 360 - this.transform.rotation.eulerAngles.z;
        }

        return currangle > 45;
    }

    public bool CheckIfComplete()
    {
        // check that a variable is singled out on one of the positive sides
        if (leftHandSidePositive.transform.childCount == 1 && leftHandSidePositive.transform.GetChild(0).GetComponent<HasValue>().typeOfItem == Draggable.Slot.Variable && leftHandSideNegative.transform.childCount == 0) {
            bool allvalues = true;
            
            foreach(Transform child in rightHandSidePositive.transform)
            {
                if (child.gameObject.GetComponent<HasValue>().typeOfItem != Draggable.Slot.Value)
                {
                    allvalues = false;
                }
            }

            foreach(Transform child in rightHandSideNegative.transform)
            {
                if (child.gameObject.GetComponent<HasValue>().typeOfItem != Draggable.Slot.Value)
                {
                    allvalues = false;
                }
            }
            return allvalues;
        }
        
        if (rightHandSidePositive.transform.childCount == 1 && rightHandSidePositive.transform.GetChild(0).GetComponent<HasValue>().typeOfItem == Draggable.Slot.Variable && rightHandSideNegative.transform.childCount == 0) {
            bool allvalues = true;
            
            foreach(Transform child in leftHandSidePositive.transform)
            {
                if (child.gameObject.GetComponent<HasValue>().typeOfItem != Draggable.Slot.Value)
                {
                    allvalues = false;
                }
            }

            foreach(Transform child in leftHandSideNegative.transform)
            {
                if (child.gameObject.GetComponent<HasValue>().typeOfItem != Draggable.Slot.Value)
                {
                    allvalues = false;
                }
            }
            return allvalues;
        }
        
        return false;
    }

    public bool CorrectlyBalanced(int correctValue)
    {
        // assumes called when it's complete
        if (leftHandSidePositive.transform.childCount == 1 && leftHandSidePositive.transform.GetChild(0).GetComponent<HasValue>().typeOfItem == Draggable.Slot.Variable && leftHandSideNegative.transform.childCount == 0)
        {
            return GetRightHandSideValue() == correctValue;
        }
        
        if (rightHandSidePositive.transform.childCount == 1 && rightHandSidePositive.transform.GetChild(0).GetComponent<HasValue>().typeOfItem == Draggable.Slot.Variable && rightHandSideNegative.transform.childCount == 0)
        {
            return GetLeftHandSideValue() == correctValue;
        }

        return false;
    }

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


    private void CancelOutSide(GameObject positiveSide, GameObject negativeSide, Draggable.Slot slot, SimpleObjectPool pool)
    {
        // cancel out extra
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
    }

}
