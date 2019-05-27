using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeesawController : MonoBehaviour
{
    
    public GameObject leftHandSidePositive;
    public GameObject rightHandSidePositive;
    public GameObject leftHandSideNegative;
    public GameObject rightHandSideNegative;
    
    private int tilt;
    private float degreetilt = 5f; // tilt by 5 for every 1 over

    // Start is called before the first frame update
    void Start()
    {
        tilt = 0;
        // InvokeRepeating("DebugTilt", 0, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTilt();
        UpdatePositions();
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
            if (currangle < tilt * degreetilt)
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
            else if (currangle < tilt * degreetilt)
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
}
