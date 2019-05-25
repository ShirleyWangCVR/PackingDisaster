using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variable : MonoBehaviour
{
    
    // public for now to set manually
    [SerializeField]
    public int value;

    // Start is called before the first frame update
    void Start()
    {
        // GameController should set this value depending on equation being used
        // value = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(int num)
    {
        value = num;
    }

    public int GetValue()
    {
        return value;
    }

}
