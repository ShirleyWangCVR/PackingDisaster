using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bracket : MonoBehaviour
{
    public Expression expression;
    public int numTerms;
    public int numDroppedOn;
    public Sprite solidArrow;
    public Sprite dashedArrow;
    public Image arrow1;
    public Image arrow2;
    public GameObject arrow1Text;
    public GameObject arrow2Text;
    private bool expanded;
    
    // Start is called before the first frame update
    void Start()
    {
        expanded = false;
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

    public void CheckExpanded()
    {
        if (expanded)
        {
            Destroy(this.gameObject);
        }
    }


    public void TermDroppedOn()
    {
        numDroppedOn++;
        Debug.Log(numDroppedOn);        

        if (numDroppedOn == numTerms)
        {
            // we have successfully expanded the bracket
            Debug.Log("Expanded");

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

                    // also need to set parentToReturnTo in Draggable
                }
                i++;
            }

            Coefficient coeff = this.gameObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>();

            arrow1.gameObject.SetActive(true);
            arrow1.sprite = solidArrow;
            arrow1Text.SetActive(true);
            arrow1Text.transform.Find("Text").gameObject.GetComponent<Text>().text = coeff.GetValue().ToString() + "x";

            arrow2.gameObject.SetActive(true);
            arrow2.gameObject.GetComponent<Image>().sprite = solidArrow;
            arrow2Text.SetActive(true);
            arrow2Text.transform.Find("Text").gameObject.GetComponent<Text>().text = coeff.GetValue().ToString() + "x";

            // destroy bracket since no longer needed
            // yield return new WaitForSeconds(1);

            Debug.Log("Destroying");
            expanded = true;

        } 
        else // first drop
        {
            int coef = (int) this.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetValue();

            if (this.transform.Find("TermsInBracket").GetChild(0).Find("Coefficient").gameObject.GetComponent<BracketInsideCoefficient>().droppedOn)
            {
                // first item in bracket was dropped on
                arrow1.gameObject.SetActive(true);
                arrow1.sprite = solidArrow;
                arrow1Text.SetActive(true);
                arrow1Text.transform.Find("Text").gameObject.GetComponent<Text>().text = coef.ToString() + "x";

                arrow2.gameObject.SetActive(true);
                arrow2.gameObject.GetComponent<Image>().sprite = dashedArrow;
            } else 
            {
                arrow2.gameObject.SetActive(true);
                arrow2.sprite = solidArrow;
                arrow2Text.SetActive(true);
                arrow2Text.transform.Find("Text").gameObject.GetComponent<Text>().text = coef.ToString() + "x";

                arrow1.gameObject.SetActive(true);
                arrow1.gameObject.GetComponent<Image>().sprite = dashedArrow;
            }
            
            // this.transform.Find("Arrow 1");
        }
    }

    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }


    public double GetValue()
    {
        double coefficient = this.gameObject.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetValue();

        double value = 0;
        foreach (Transform child in this.gameObject.transform.Find("TermsInBracket"))
        {
            value = value + child.Find("Coefficient").gameObject.GetComponent<Coefficient>().GetValue() * child.gameObject.GetComponent<HasValue>().GetValue();
        }

        return coefficient * value;
    }
}
