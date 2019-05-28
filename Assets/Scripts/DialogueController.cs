using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public DialogueData dialogueData;
    public GameObject dialogueDisplay;
    public Text userText;

    private DataController dataController;
    private bool dialogueActive;
    private string[] currentDialogue;
    private int dialogueIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        dialogueData = dataController.dialogue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExecuteTutorialDialogue()
    {
        dialogueActive = true;
        dialogueDisplay.SetActive(true);
        currentDialogue = dialogueData.level0DialogueList;
        dialogueIndex = 0;
        ShowDialogue();
    }

    public void ShowDialogue()
    {
        userText.text = currentDialogue[dialogueIndex];
    }

    public void OnClick()
    {
        if (dialogueActive)
        {
            dialogueIndex++;
            if (dialogueIndex == currentDialogue.Length)
            {
                dialogueActive = false;
                dialogueDisplay.SetActive(false);
                return;
            }
            ShowDialogue();
        }
    }

    public bool FinishedDialogue()
    {
        return ! dialogueActive;
    }
}
