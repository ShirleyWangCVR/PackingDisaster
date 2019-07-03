using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Controller for the Game Seesaw
 */
public class TutSeesawController : SeesawController
{
    // variables inherited from SeesawController

    // Start is called before the first frame update
    void Start()
    {
        // set initial tilt to 0
        currentlyDragging = false;
        tilt = 0;
        roundActive = true;
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // update the seesaw's current tilt
        if (! currentlyDragging && roundActive)
        {
            UpdateTilt();
            UpdatePositions();
            // UpdateCurrentEquation();
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
            peg.transform.Rotate(0, 0, 0.01f, Space.Self);
        }
        else if (tilt < 0)
        {
            this.transform.Rotate(0, 0, -0.01f, Space.Self);
            peg.transform.Rotate(0, 0, -0.01f, Space.Self);
        }
        else
        {   // tilt == 0
            // Unity doesn't move it by exact values so give it a slight bit of wiggle room when
            // returning to horizontal
            if (currangle >= 0.1 || currangle <= -0.1)
            {
                if (this.transform.rotation.eulerAngles.z < 180)
                {
                    this.transform.Rotate(0, 0, -0.05f, Space.Self);
                    peg.transform.Rotate(0, 0, -0.05f, Space.Self);
                } else
                {
                    this.transform.Rotate(0, 0, 0.05f, Space.Self);
                    peg.transform.Rotate(0, 0, 0.05f, Space.Self);
                }
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    // tutorial 1
    public bool CheckDraggedStillBalanced()
    {
        return leftHandSidePositive.GetComponent<SeesawSide>().NumVariables() == 1 && leftHandSidePositive.GetComponent<SeesawSide>().NumValues() == 1 && rightHandSidePositive.GetComponent<SeesawSide>().NumValues() == 4 && tilt == 0 && ! currentlyDragging;
    }

    // tutorial 2
    public bool CheckDraggedToPositive()
    {
        return rightHandSidePositive.GetComponent<SeesawSide>().NumVariables() == 1 && leftHandSideNegative.GetComponent<SeesawSide>().NumVariables() == 0;
    }

    public bool CheckDraggedToNegative()
    {
        return rightHandSidePositive.GetComponent<SeesawSide>().NumValues() == 0 && leftHandSideNegative.GetComponent<SeesawSide>().NumValues() == 1;
    }

}
