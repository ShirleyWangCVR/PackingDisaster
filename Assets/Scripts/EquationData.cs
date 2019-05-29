using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* EquationData represents one equation that will be solved by the player
 * during the level.
 */
[System.Serializable]
public class EquationData
{
    public int timeLimit;
    public int difficulty;
    public int variableValue;
    public int lhsVars;
    public int lhsValues;
    public int rhsVars;
    public int rhsValues;
    public string equation;
}
