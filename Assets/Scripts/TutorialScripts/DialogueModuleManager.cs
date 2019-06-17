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
    public AudioClip clicked;

    private Queue<string> dialogueQueue;
    private string speaker;

    private AudioSource audioSource;
    private Animator anim;
    private int batch;
    private bool firstDialogue;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueQueue = new Queue<string>();
        anim = GetComponent(typeof(Animator)) as Animator;
        tutController = FindObjectOfType<TutorialController>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        continueButton.SetActive(true);
        firstDialogue = false;
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

    public void ExitDialogueBox()
    {
        anim.SetTrigger("moveOffscreen");
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
    }
    public void DisplayNextSentence() {

        if (dialogueQueue.Count == 0) {
            return;
        }

        if (! firstDialogue)
        {
            audioSource.PlayOneShot(clicked, 3.0f);
        }

        // Debug.Log(dialogueQueue.Count);
        tutController.CurrentDialogue(dialogueQueue.Count);
        
        firstDialogue = false;
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
        firstDialogue = true;
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
        /* else if (batch == 5)
        {
            batch = 6;
            tutController.FinishedFifthDialogue();
        }
        else if (batch == 6)
        {
            batch = 7;
            tutController.FinishedSixthDialogue();
        } */
    }
}
