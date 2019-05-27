using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquationData
{
    public int timeLimit = 60;
    public int difficulty = 1;
    public int variableValue = 3;
    public int lhsVars = 1;
    public int lhsValues = 2;
    public int rhsVars = 0;
    public int rhsValues = 5;
    public string equation = "x + 2 = 5";
}
