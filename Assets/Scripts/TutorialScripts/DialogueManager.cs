using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueTextBox;
    private Queue<string> dialogueQueue;

    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueQueue = new Queue<string>();
        anim = GetComponent(typeof(Animator)) as Animator;
    }

    public void StartDialogue(string[] dialogueSentences) {
        Debug.Log("Starting a dialogue.");
        
        dialogueQueue.Clear();

        foreach (string sentence in dialogueSentences) {
            dialogueQueue.Enqueue(sentence);
        }

        anim.SetTrigger("enableContinue");
        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (dialogueQueue.Count == 0) {
            EndDialogue();
            return;
        }

        string nextSentence = dialogueQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(nextSentence));
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueTextBox.text = "";
        foreach (char letter in sentence) {
            dialogueTextBox.text += letter;
            yield return null;
        }
    }

    void EndDialogue() {
        Debug.Log("A conversation has ended.");
    }
}
