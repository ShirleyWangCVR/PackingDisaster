using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coefficient : MonoBehaviour
{
    public Text numeratorText;
    public Text denominatorText;
    public Text fractionLineText;
    public Text numberText;

    private int numerator;
    private int denominator;
    private int wholeNumber;
    private double exactNumber;
    private bool isWholeNumber;
    
    // Start is called before the first frame update
    void Start()
    {
        isWholeNumber = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public double GetValue()
    {
        if (isWholeNumber)
        {
            return wholeNumber;
        } else {
            return exactNumber;
        }
    }
    
    public void NegativeCurrentValue()
    {
        if (isWholeNumber)
        {
            SetIntValue(0 - wholeNumber);
        } else {
            SetFractionValue(0 - numerator, denominator);
        }
    }

    public void SetIntValue(int newValue)
    {
        Debug.Log(wholeNumber);
        Debug.Log(newValue);
        isWholeNumber = true;
        wholeNumber = newValue;
        numberText.gameObject.SetActive(true);
        numberText.text = wholeNumber.ToString();

        numeratorText.gameObject.SetActive(false);
        denominatorText.gameObject.SetActive(false);
        fractionLineText.gameObject.SetActive(false);
    }

    public void SetFractionValue(int num, int denom)
    {
        if (num % denom == 0)
        {
            isWholeNumber = true;
            wholeNumber = (int) num / denom;
            numberText.gameObject.SetActive(true);
            numberText.text = wholeNumber.ToString();

            numeratorText.gameObject.SetActive(false);
            fractionLineText.gameObject.SetActive(false);
            denominatorText.gameObject.SetActive(false);
        } 
        else
        {
            isWholeNumber = false;
            exactNumber = (double) num / (double) denom;
            numerator = num;
            denominator = denom;

            numberText.gameObject.SetActive(false);

            numeratorText.gameObject.SetActive(true);
            numeratorText.text = numerator.ToString();

            fractionLineText.gameObject.SetActive(true);

            denominatorText.gameObject.SetActive(true);
            denominatorText.text = denominator.ToString();
        }
    }

}
