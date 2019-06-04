using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [TextArea(3, 6)]
    public string[] dialogueSentences;

    public void TriggerDialogue() {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogueSentences);
    }
}
