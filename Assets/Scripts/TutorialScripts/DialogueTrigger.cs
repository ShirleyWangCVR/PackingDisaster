using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea(3, 6)]
    public string[] dialogueSentences;

    private TutorialManager tutManager;

    void Start()
    {
        tutManager = FindObjectOfType<TutorialManager>();
    }

    public void TriggerInitialDialogue() {
        Debug.Log("TriggerDialogue() in DialogueTrigger entered.");
        StartCoroutine(tutManager.InitBobDialogue(dialogueSentences));
    }

    public void TriggerDialogue() {
        Debug.Log("TriggerDialogue() in DialogueTrigger entered.");
        StartCoroutine(tutManager.BobDialogue(dialogueSentences));
    }
}
