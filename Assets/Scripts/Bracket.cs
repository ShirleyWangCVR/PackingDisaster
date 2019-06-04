using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bracket : MonoBehaviour
{
    public Expression expression;
    public int numTerms;
    public int numDroppedOn;
    
    // Start is called before the first frame update
    void Start()
    {
        numDroppedOn = 0;
        numTerms = this.gameObject.transform.Find("TermsInBracket").childCount;

        foreach (Transform child in this.gameObject.transform.Find("TermsInBracket"))
        {
            child.gameObject.GetComponent<Draggable>().SetBracketStatus(true);
            child.gameObject.transform.Find("Coefficient").gameObject.AddComponent<BracketInsideCoefficient>();
        }

        // for testing
        // erase this on finish
        this.gameObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(3);
        foreach (Transform child in this.gameObject.transform.Find("TermsInBracket"))
        {
            child.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TermDroppedOn()
    {
        numDroppedOn++;
        Debug.Log(numDroppedOn);        

        if (numDroppedOn == numTerms)
        {
            // we have successfully expanded the bracket
            Debug.Log("Expanded");

            /* for(int i = 0; i < this.gameobject.transform.GetChildCount(); i++)
            {
            GameObject Go = this.gameobject.transform.GetChild(i);
            } */

            int i = 0;
            int numChildren = this.gameObject.transform.Find("TermsInBracket").childCount;
            
            while (i < numChildren)
            {
                Transform child = this.gameObject.transform.Find("TermsInBracket").GetChild(0);

                // multiply out coefficients
                Coefficient coef = child.Find("Coefficient").gameObject.GetComponent<Coefficient>();
                Fraction newCoef = coef.GetFractionValue() * this.gameObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetFractionValue();
                coef.SetValue(newCoef);

                // reset as draggable
                child.gameObject.GetComponent<Draggable>().SetBracketStatus(false);
                Destroy(child.gameObject.transform.Find("Coefficient").GetComponent<BracketInsideCoefficient>());
            
                // TODO: put it where it should be
                // need to put negative values onto the negative sides and positive values onto the positive sides.
                // for now just do the positive one
                if (newCoef > 0) // and current side is positive one
                {
                    // Transform parent = this.gameObject.transform.parent;
                    child.SetParent(this.gameObject.transform.parent);
                }

                i++;
            }

            // destroy bracket since no longer needed
            Destroy(this.gameObject);

        }
    }

}
