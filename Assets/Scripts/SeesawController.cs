using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Text equationText;
    public AudioClip dangerSfx;

    protected bool currentlyDragging;
    protected double tilt;
    protected string originalSide; // original side of a thing being dragged
    protected bool roundActive;
    protected double prevTilt;

    protected AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // set initial tilt to 0
        tilt = 0;
        currentlyDragging = false;
        roundActive = true;
        audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.clip = dangerSfx;
        audioSource.volume = 2;
    }

    // Update is called once per frame
    void Update()
    {
        // update the seesaw's current tilt
        if (! currentlyDragging && roundActive)
        {
            UpdateTilt();
            UpdatePositions();
            UpdateCurrentEquation();
        }
    }

    public void SetRoundActive(bool active)
    {
        roundActive = active;
    }

    public void SetDragging(bool dragging, string side)
    {
        currentlyDragging = dragging;

        if (dragging)
        {
            originalSide = side;
        }

        // Allow things on other side to be allowed to be dropped onto
        bool right;
        bool left;
        if (originalSide == "right")
        {
            left = true;
            right = false;
        }
        else if (originalSide == "left")
        {
            left = false;
            right = true;
        }
        else
        {
            left = true;
            right = true;
        }

        if (left)
        {
            foreach(Transform child in leftHandSidePositive.transform)
            {
                child.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = ! dragging;
            }

            foreach(Transform child in leftHandSideNegative.transform)
            {
                child.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = ! dragging;
            }
        }

        if (right)
        {
            foreach(Transform child in rightHandSidePositive.transform)
            {
                child.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = ! dragging;
            }

            foreach(Transform child in rightHandSideNegative.transform)
            {
                child.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = ! dragging;
            }
        }

        /* // Play the danger music
        if (! dragging)
        {
            Debug.Log("Check audioSource");
            CheckTilt();
        } */
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
            if (currangle > 0.1 || currangle < -0.1)
            {
                if (this.transform.rotation.eulerAngles.z < 180)
                {
                    this.transform.Rotate(0, 0, -0.1f, Space.Self);
                } else
                {
                    this.transform.Rotate(0, 0, 0.1f, Space.Self);
                }
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0, 0, 0);
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

        prevTilt = tilt;
        tilt = lhs - rhs;

        if (tilt != prevTilt)
        {
            Debug.Log("Tilt changed");
            CheckTilt();
        }

        if (tilt < 0.05)
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }
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
    public virtual bool CheckIfComplete()
    {
        // check if there is only 1 variable on the left hand side
        if (leftHandSidePositive.transform.childCount == 1 && leftHandSidePositive.transform.GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable && leftHandSideNegative.transform.childCount == 0)
        {
            return rightHandSidePositive.GetComponent<SeesawSide>().NumVariables() == 0 && rightHandSideNegative.GetComponent<SeesawSide>().NumVariables() == 0;
        }

        if (rightHandSidePositive.transform.childCount == 1 && rightHandSidePositive.transform.GetChild(0).GetComponent<Draggable>().typeOfItem == Draggable.Slot.Variable && rightHandSideNegative.transform.childCount == 0)
        {
            return leftHandSidePositive.GetComponent<SeesawSide>().NumVariables() == 0 && leftHandSideNegative.GetComponent<SeesawSide>().NumVariables() == 0;
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
        return rightHandSidePositive.GetComponent<SeesawSide>().TotalNumericalValue() - rightHandSideNegative.GetComponent<SeesawSide>().TotalNumericalValue();
    }

    // get total numerical value of left hand side
    public virtual double GetLeftHandSideValue()
    {
        return leftHandSidePositive.GetComponent<SeesawSide>().TotalNumericalValue() - leftHandSideNegative.GetComponent<SeesawSide>().TotalNumericalValue();
    }

    public virtual void UpdateCurrentEquation()
    {
        string equation = "";
        string lside = "";
        string rside = "";

        int lhsPosVars = leftHandSidePositive.GetComponent<SeesawSide>().NumVariables();
        int lhsPosValues = leftHandSidePositive.GetComponent<SeesawSide>().NumValues();
        if (lhsPosVars > 0)
        {
            if (lhsPosVars == 1)
            {
                lside = lside + "x";
            }
            else
            {
                lside = lside + lhsPosVars.ToString() + "x";
            }
        }
        if (lhsPosValues > 0)
        {
            if (lhsPosVars > 0)
            {
                lside = lside + " + ";
            }
            lside = lside + lhsPosValues.ToString();
        }

        int lhsNegVars = leftHandSideNegative.GetComponent<SeesawSide>().NumVariables();
        int lhsNegValues = leftHandSideNegative.GetComponent<SeesawSide>().NumValues();
        if (lhsNegVars > 0)
        {
            if (lside.Length > 0)
            {
                lside = lside + " - ";
            }
            else
            {
                lside = lside + "-";
            }

            if (lhsNegVars > 1)
            {
                lside = lside + lhsNegVars.ToString();
            }
            lside = lside + "x";
        }
        if (lhsNegValues > 0)
        {
            if (lside.Length > 0)
            {
                lside = lside + " - ";
            }
            else
            {
                lside = lside + "-";
            }
            lside = lside + lhsNegValues.ToString();
        }

        if (lside.Length == 0)
        {
            lside = lside + "0";
        }

        // right hand side calculation
        int rhsPosVars = rightHandSidePositive.GetComponent<SeesawSide>().NumVariables();
        int rhsPosValues = rightHandSidePositive.GetComponent<SeesawSide>().NumValues();
        if (rhsPosVars > 0)
        {
            if (rhsPosVars > 1)
            {
                rside = rside + rhsPosVars.ToString();
            }
            rside = rside + "x";
        }
        if (rhsPosValues > 0)
        {
            if (rhsPosVars > 0)
            {
                rside = rside + " + ";
            }
            rside = rside + rhsPosValues.ToString();
        }

        int rhsNegVars = rightHandSideNegative.GetComponent<SeesawSide>().NumVariables();
        int rhsNegValues = rightHandSideNegative.GetComponent<SeesawSide>().NumValues();
        if (rhsNegVars > 0)
        {
            if (rside.Length > 0)
            {
                rside = rside + " - ";
            }
            else
            {
                rside = rside + "-";
            }

            if (rhsNegVars > 1)
            {
                rside = rside + rhsNegVars.ToString();
            }
            rside = rside + "x";
        }
        if (rhsNegValues > 0)
        {
            if (rside.Length > 0)
            {
                rside = rside + " - ";
            }
            else
            {
                rside = rside + "-";
            }
            rside = rside + rhsNegValues.ToString();
        }

        if (rside.Length == 0)
        {
            rside = rside + "0";
        }

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

    protected void CheckTilt()
    {
        if (tilt < 0.05 && tilt > -0.05)
        {
            Debug.Log("Safe");
            StopCoroutine(PlayDanger());
            audioSource.Stop();
        }
        else 
        {
            Debug.Log("Starting Danger");
            StartCoroutine(PlayDanger());
        }
    }

    protected IEnumerator PlayDanger()
    {
        yield return new WaitForSeconds(1.5f);
        audioSource.Play();
    }

}
