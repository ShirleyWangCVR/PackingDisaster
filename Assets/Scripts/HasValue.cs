using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasValue : MonoBehaviour
{
    
    // public for now to set manually
    [SerializeField]
    public int value;
    public Draggable.Slot typeOfItem = Draggable.Slot.Value;

    // Start is called before the first frame update
    void Start()
    {
        // GameController should set this value depending on equation being used
        // value = 3;
        if (typeOfItem == Draggable.Slot.Value)
        {
            value = 1;
        } else if (typeOfItem == Draggable.Slot.Dummy)
        {
            value = 0;
        }
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
