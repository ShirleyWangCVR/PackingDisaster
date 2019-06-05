using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea(3, 6)]
    public string[] dialogueSentences;

    public void TriggerInitialDialogue() {
        Debug.Log("TriggerDialogue() in DialogueTrigger entered.");
        // FindObjectOfType<TutorialManager>().InitBobDialogue(dialogueSentences);
        StartCoroutine(FindObjectOfType<TutorialManager>().InitBobDialogue(dialogueSentences));
    }

    public void TriggerDialogue() {
        Debug.Log("TriggerDialogue() in DialogueTrigger entered.");
        // FindObjectOfType<TutorialManager>().InitBobDialogue(dialogueSentences);
        StartCoroutine(FindObjectOfType<TutorialManager>().BobDialogue(dialogueSentences));
    }
}
