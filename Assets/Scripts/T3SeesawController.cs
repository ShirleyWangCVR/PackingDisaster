using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Controller for the Game Seesaw
 */
public class T3SeesawController : T2SeesawController
{
    // variables inherited from T2SeesawController

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

        lhs = leftHandSidePositive.GetComponent<T3PositiveSide>().TotalNumericalValue() + leftHandSideNegative.GetComponent<T3NegativeSide>().TotalNumericalValue();
        rhs = rightHandSidePositive.GetComponent<T3PositiveSide>().TotalNumericalValue() + rightHandSideNegative.GetComponent<T3NegativeSide>().TotalNumericalValue();

        tilt = lhs - rhs;
    }

    // get total numerical value of right hand side
    public new double GetRightHandSideValue()
    {
        return rightHandSidePositive.GetComponent<T3PositiveSide>().TotalNumericalValue() + rightHandSideNegative.GetComponent<T3NegativeSide>().TotalNumericalValue();
    }

    // get total numerical value of left hand side
    public new double GetLeftHandSideValue()
    {
        return leftHandSidePositive.GetComponent<T3PositiveSide>().TotalNumericalValue() + leftHandSideNegative.GetComponent<T3NegativeSide>().TotalNumericalValue();
    }
    
}
