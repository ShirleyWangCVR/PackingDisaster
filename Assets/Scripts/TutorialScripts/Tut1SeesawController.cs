using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Controller for the Game Seesaw
 */
public class Tut1SeesawController : SeesawController
{
    // variables inherited from SeesawController

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
            UpdateCurrentEquation();
        }
    }

    // in tutorial seesaw tilts slower than usual
    protected override void UpdatePositions()
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

}
