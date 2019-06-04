﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    private Animator anim;

    void Start() {
        anim = GetComponent(typeof(Animator)) as Animator;
    }

    public void dialogueEnterLeft() {
        Debug.Log("About to set trigger: dialogueEnterLeft.");
        anim.SetTrigger("dialogueEnterLeft");
    }

    public void dialogueExitLeft() {
        Debug.Log("About to set trigger: dialogueExitLeft.");
        anim.SetTrigger("dialogueExitLeft");
    }
}
