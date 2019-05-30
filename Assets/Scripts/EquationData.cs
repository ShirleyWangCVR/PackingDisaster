using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* EquationData represents one equation that will be solved by the player
 * during the level.
 */
[System.Serializable]
public class EquationData
{
    // public int timeLimit;
    public int difficulty;
    public int variableValue;
    public Expression lhs;
    public Expression rhs;
    public string equation;
    
    // possibly add in function to update string equation representation
}
