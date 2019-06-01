﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GetOperation : MonoBehaviour
{
    public enum Slot {Operation, Number};
    public Slot type;

    private BothSideOperations operationController;

    public void Start()
    {
        operationController = FindObjectOfType<BothSideOperations>();
    }

    public void ProcessPress()
    {
        if (type == Slot.Operation)
        {
            operationController.SetOperation(this.gameObject.name);
        } else if (type == Slot.Number)
        {
            operationController.SetNumber(Int32.Parse(this.gameObject.name));
        }
    }
}