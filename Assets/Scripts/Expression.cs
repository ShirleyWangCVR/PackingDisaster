﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Expression
{
    public int numVars;
    public int numValues;
    public int numBrackets;
    public Expression[] bracketExpressions;
    public string representation;

    // possibly add in functions to update string representation and numvars and values
}