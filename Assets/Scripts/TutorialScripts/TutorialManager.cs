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

    }

    public IEnumerator InitBobDialogue(string[] dialogueContents) {
        Debug.Log("InitBobDialogue() has been entered.");
        bobCtrl.dialogueEnterLeft();
        Debug.Log("About to wait for 2.2 seconds.");
        yield return new WaitForSeconds(2.2f);
        Debug.Log("Done waiting.");
        dialogueModMgr.InitDialogue("BOB", dialogueContents);
    }

    public IEnumerator BobDialogue(string[] dialogueContents) {
        dialogueModMgr.ContinueDialogue("BOB", dialogueContents);
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator EndDialogue()
    {
        Debug.Log("Here");
        yield return new WaitForSeconds(2f);
        dialogueModMgr.ExitDialogueBox();
    }
    public void EndDialogueNow()
    {
        // yield return new WaitForSeconds(2f);
        dialogueModMgr.ExitDialogueBox();
    }

    public void KickBob() {
        bobCtrl.dialogueExitLeft();
    }
}
