using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public BobController bobCtrl;
    public DialogueModuleManager dialogueModMgr;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("TutorialManager has Start()ed.");
    }

    public void Garbage() {
        Debug.Log("Garbage.");
    }

    public IEnumerator InitBobDialogue(string[] dialogueContents) {
        Debug.Log("InitBobDialogue() has been entered.");
        bobCtrl.dialogueEnterLeft();
        Debug.Log("About to wait for 2.2 seconds.");
        yield return new WaitForSeconds(2.2f);
        Debug.Log("Done waiting.");
        dialogueModMgr.InitDialogue("BOB", dialogueContents);
    }

    public void kickBob() {
        bobCtrl.dialogueExitLeft();
    }
}
