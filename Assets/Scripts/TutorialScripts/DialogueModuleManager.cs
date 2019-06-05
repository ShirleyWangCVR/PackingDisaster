using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueModuleManager : MonoBehaviour
{
    public Text speakerText;
    public Text dialogueText;

    private Queue<string> dialogueQueue;
    private string speaker;

    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueQueue = new Queue<string>();
        anim = GetComponent(typeof(Animator)) as Animator;
    }

    public void InitDialogue(string speaker, string[] dialogueSentences) {
        Debug.Log("Starting a dialogue.");
        
        dialogueQueue.Clear();
        foreach (string sentence in dialogueSentences) {
            dialogueQueue.Enqueue(sentence);
        }
        StartCoroutine(openDialogueBox(speaker));
        Debug.Log("displayDialogueBox done.");
    }

    IEnumerator openDialogueBox(string speaker) {
        anim.SetTrigger("moveOnscreen");

        yield return new WaitForSeconds(2f);

        Debug.Log("Immediately before speakerText is manipulated, speaker == " + speaker);
        StartCoroutine(startSpeaking(speaker));
        Debug.Log("speakerText has been fully typed.");
        // speakerText.text = speaker;
        // anim.SetTrigger("enableContinue");
        // DisplayNextSentence();
    }
    public void DisplayNextSentence() {
        if (dialogueQueue.Count == 0) {
            EndDialogue();
            return;
        }

        string nextSentence = dialogueQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(nextSentence));
    }

    IEnumerator TypeDialogue(string inputString) {
        dialogueText.text = "";
        foreach (char letter in inputString) {
            dialogueText.text += letter;
            yield return null;
        }
    }

    IEnumerator startSpeaking(string inputString) {
        speakerText.text = "";
        foreach (char letter in inputString) {
            speakerText.text += letter;
            yield return new WaitForSeconds(0.4f);
        }
        anim.SetTrigger("enableContinue");
        DisplayNextSentence();
    }

    void EndDialogue() {
        Debug.Log("A conversation has ended.");
    }
}
