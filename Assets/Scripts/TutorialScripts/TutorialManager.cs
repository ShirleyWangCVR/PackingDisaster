using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Bob bob;
    public DialogueManager dialogueMgr;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("TutorialManager has Start()ed.");
         bob.dialogueEnterLeft();
    }

    public void kickBob() {
        bob.dialogueExitLeft();
    }
}
