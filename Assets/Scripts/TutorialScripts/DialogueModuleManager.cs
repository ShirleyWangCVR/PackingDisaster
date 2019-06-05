﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueModuleManager : MonoBehaviour
{
    public Text speakerText;
    public Text dialogueText;
    public Tut1GameController gameController;

    private Queue<string> dialogueQueue;
    private string speaker;

    private Animator anim;
    private int batch;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueQueue = new Queue<string>();
        anim = GetComponent(typeof(Animator)) as Animator;
        gameController = FindObjectOfType<Tut1GameController>();
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
        
        dialogueQueue.Clear();
        foreach (string sentence in dialogueSentences) {
            dialogueQueue.Enqueue(sentence);
        }
        StartCoroutine(openDialogueBox(speaker, true));
        Debug.Log("displayDialogueBox done.");
    }

    public void ContinueDialogue(string speaker, string[] dialogueSentences) {
        Debug.Log("Starting a dialogue.");
        
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

        if (batch == 1)
        {
            batch = 2;
            gameController.FinishedFirstDialogue();
        }
        else if (batch == 2)
        {
            batch = 3;
            gameController.FinishedSecondDialogue();
        }
        else if (batch == 3)
        {
            batch = 4;
            gameController.FinishedThirdDialogue();
        }
        else if (batch == 4)
        {
            batch = 5;
            gameController.FinishedFourthDialogue();
        }
        else if (batch == 5)
        {
            batch = 6;
            gameController.FinishedFifthDialogue();
        }
        else if (batch == 6)
        {
            batch = 7;
            gameController.FinishedSixthDialogue();
        }
        
    }
}