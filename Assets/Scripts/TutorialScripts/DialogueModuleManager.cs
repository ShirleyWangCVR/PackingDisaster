using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueModuleManager : MonoBehaviour
{
    public Text speakerText;
    public Text dialogueText;
    public TutorialController tutController;
    public GameObject continueButton;

    private Queue<string> dialogueQueue;
    private string speaker;

    private Animator anim;
    private int batch;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueQueue = new Queue<string>();
        anim = GetComponent(typeof(Animator)) as Animator;
        tutController = FindObjectOfType<TutorialController>();
        continueButton.SetActive(true);
        batch = 1;
    }

    public int GetBatch()
    {
        return batch;
    }

    public void SetBatch(int newbatch)
    {
        batch = newbatch;
    }

    public void InitDialogue(string speaker, string[] dialogueSentences) {
        Debug.Log("Starting a dialogue.");
        continueButton.SetActive(true);
        
        dialogueQueue.Clear();
        foreach (string sentence in dialogueSentences) {
            dialogueQueue.Enqueue(sentence);
        }
        StartCoroutine(openDialogueBox(speaker, true));
        Debug.Log("displayDialogueBox done.");
    }

    public void ContinueDialogue(string speaker, string[] dialogueSentences) {
        Debug.Log("Starting a dialogue.");
        continueButton.SetActive(true);
        
        dialogueQueue.Clear();
        foreach (string sentence in dialogueSentences) {
            dialogueQueue.Enqueue(sentence);
        }
        StartCoroutine(openDialogueBox(speaker, false));
        Debug.Log("displayDialogueBox done.");
    }

    IEnumerator openDialogueBox(string speaker, bool moveBox) {

        if (moveBox)
        {
            anim.SetTrigger("moveOnscreen");
            yield return new WaitForSeconds(2f);
        }

        Debug.Log("Immediately before speakerText is manipulated, speaker == " + speaker);
        StartCoroutine(startSpeaking(speaker));
        Debug.Log("speakerText has been fully typed.");
        // speakerText.text = speaker;
        // anim.SetTrigger("enableContinue");
        // DisplayNextSentence();
    }
    public void DisplayNextSentence() {

        Debug.Log("Display next sentence");
        if (dialogueQueue.Count == 0) {
            return;
        }

        string nextSentence = dialogueQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(nextSentence));

        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
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
        continueButton.SetActive(false);

        if (batch == 1)
        {
            batch = 2;
            tutController.FinishedFirstDialogue();
        }
        else if (batch == 2)
        {
            batch = 3;
            tutController.FinishedSecondDialogue();
        }
        else if (batch == 3)
        {
            batch = 4;
            tutController.FinishedThirdDialogue();
        }
        else if (batch == 4)
        {
            batch = 5;
            tutController.FinishedFourthDialogue();
        }
        else if (batch == 5)
        {
            batch = 6;
            tutController.FinishedFifthDialogue();
        }
        else if (batch == 6)
        {
            batch = 7;
            tutController.FinishedSixthDialogue();
        }
        else if (batch == 7)
        {
            batch = 8;
            // tutController.FinishedSixthDialogue();
        }
        
    }
}
