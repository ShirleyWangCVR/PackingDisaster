using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The controller of the interactive elements of the tutorials
public class TutorialController : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public DialogueModuleManager dialogueModuleManager;
    public GameObject interactivePanel;
    public DialogueTrigger[] dialogueTriggers;
    public GameObject seesaw;
    public GameObject skipButton;
    public Transform checkLHSPositive;

    private DataController dataController;
    private int tutorialLevel;
    private int dialogueNum;
    private bool waitForFirstDrag;
    private bool waitForSecondDrag;
    private bool waitForThirdDrag;
    private bool finishedSecondDrag;
    private bool startingUp;
    private bool doneTutorial;

    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        tutorialLevel = dataController.GetDifficulty();

        checkLHSPositive = seesaw.transform.Find("LHSPositive");

        if (dataController.GetTriedTutorial(tutorialLevel) > 0)
        {
            skipButton.SetActive(true);
        }

        dataController.SetTriedTutorial(tutorialLevel, 1);

        dialogueNum = 1;
        waitForFirstDrag = false;
        waitForSecondDrag = false;
        waitForThirdDrag = false;
        finishedSecondDrag = false;
        doneTutorial = false;

        if (tutorialLevel == 1)
        {
            dialogueTriggers[0].TriggerInitialDialogue();
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (tutorialLevel == 2)
        {
            dialogueTriggers[2].TriggerInitialDialogue();
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (tutorialLevel == 6)
        {
            dialogueTriggers[0].TriggerInitialDialogue();
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (tutorialLevel == 11)
        {
            dialogueTriggers[3].TriggerInitialDialogue();
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (tutorialLevel == 16)
        {
            startingUp = true;
            interactivePanel.transform.Find("Seesaw Arrow 11").gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialLevel == 1)
        {
            if (waitForFirstDrag)
            {
                if (seesaw.GetComponent<TutSeesawController>().CheckDraggedStillBalanced())
                {
                    waitForFirstDrag = false;
                    DraggedCorrectly();
                    interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }
        }
        else if (tutorialLevel == 2)
        {
            if (waitForFirstDrag)
            {
                if (seesaw.GetComponent<TutSeesawController>().CheckDraggedToPositive())
                {
                    waitForFirstDrag = false;
                    DraggedToyOver();
                }
            }

            if (waitForSecondDrag)
            {
                if (seesaw.GetComponent<TutSeesawController>().CheckDraggedToNegative())
                {
                    waitForSecondDrag = false;
                    DraggedToyOver2();
                }
            }
        }
        else if (tutorialLevel == 6)
        {
            if (waitForFirstDrag)
            {
                if (seesaw.transform.Find("RHSPositive").GetChild(0).childCount == 2)
                {
                    waitForFirstDrag = false;
                    DoubleClicked();
                }
            }

            if (waitForSecondDrag)
            {
                if (seesaw.transform.Find("RHSPositive").GetChild(0).childCount == 1)
                {
                    waitForSecondDrag = false;
                    Combined();
                }
            }
        }
        else if (tutorialLevel == 16)
        {
            if (startingUp)
            {
                startingUp = false;

                Debug.Log(checkLHSPositive.GetChild(0).childCount);
            
                GameObject bracket = checkLHSPositive.GetChild(0).GetChild(0).gameObject;
                Tut6Bracket check = bracket.AddComponent<Tut6Bracket>();
                dialogueTriggers[7].TriggerInitialDialogue();
                interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            
            if (finishedSecondDrag)
            {
                finishedSecondDrag = false;
                Expanded();
            }
        }
    }

    public void FinishedFirstDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (tutorialLevel == 1)
        {
            interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
            interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(true);
            waitForFirstDrag = true;
        }
        else if (tutorialLevel == 2)
        {
            waitForFirstDrag = true;
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else if (tutorialLevel == 6)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow 7").gameObject.SetActive(true);
            interactivePanel.transform.Find("Seesaw Arrow 8").gameObject.SetActive(true);
            waitForFirstDrag = true;
        }
        else if (tutorialLevel == 11)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
            waitForFirstDrag = true;
        }
        else if (tutorialLevel == 16)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow 4").gameObject.SetActive(true);
            interactivePanel.transform.Find("Seesaw Arrow 5").gameObject.SetActive(true);
            waitForFirstDrag = true;
        }
    }

    public void FinishedSecondDialogue()
    {
        if (tutorialLevel == 1)
        {
            interactivePanel.transform.Find("Seesaw Arrow 11").gameObject.SetActive(true);
            StartCoroutine(tutorialManager.EndDialogue());
            doneTutorial = true;
        }
        else if (tutorialLevel == 2)
        {
            interactivePanel.transform.Find("Seesaw Arrow 6").gameObject.SetActive(true);
            waitForSecondDrag = true;
        }
        else if (tutorialLevel == 6)
        {
            interactivePanel.transform.Find("Seesaw Arrow 9").gameObject.SetActive(true);
            waitForSecondDrag = true;
        }
        else if (tutorialLevel == 11)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
            waitForSecondDrag = true;
        }
        else if (tutorialLevel == 16)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow 4").gameObject.SetActive(true);
            interactivePanel.transform.Find("Seesaw Arrow 6").gameObject.SetActive(true);
            waitForSecondDrag = true;
        }
    }

    public void FinishedThirdDialogue()
    {
        if (tutorialLevel == 2)
        {
            interactivePanel.transform.Find("Seesaw Arrow 8").gameObject.SetActive(false);
            StartCoroutine(tutorialManager.EndDialogue());
            doneTutorial = true;
        }
        else if (tutorialLevel == 6)
        {
            interactivePanel.SetActive(false);
            StartCoroutine(tutorialManager.EndDialogue());
            doneTutorial = true;
        }
        else if (tutorialLevel == 11)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(true);
            waitForThirdDrag = true;
        }
        else if (tutorialLevel == 16)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.SetActive(false);
            StartCoroutine(tutorialManager.EndDialogue());
            doneTutorial = true;
        }
    }

    public virtual void FinishedFourthDialogue()
    {
        if (tutorialLevel == 11)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.SetActive(false);
            StartCoroutine(tutorialManager.EndDialogue());
            doneTutorial = true;
        }
    }

    public void CurrentDialogue(int linesLeft)
    {
        if (tutorialLevel == 1)
        {
            if (dialogueNum == 1)
            {
                if (linesLeft == 4)
                {
                    interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
                }
                else if (linesLeft == 3)
                {
                    interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
                    interactivePanel.transform.Find("Seesaw Arrow").localScale = new Vector2(0 - interactivePanel.transform.Find("Seesaw Arrow").localScale.x, interactivePanel.transform.Find("Seesaw Arrow").localScale.y);
                }
                else if (linesLeft == 2)
                {
                    interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(false);

                    interactivePanel.transform.Find("Seesaw Arrow 7").gameObject.SetActive(true);
                    interactivePanel.transform.Find("Seesaw Arrow 9").gameObject.SetActive(true);
                }
                else if (linesLeft == 1)
                {               
                    interactivePanel.transform.Find("Seesaw Arrow 7").gameObject.SetActive(false);
                    interactivePanel.transform.Find("Seesaw Arrow 9").gameObject.SetActive(false);
                }
            }
            else if (dialogueNum == 2)
            {
                if (linesLeft == 2)
                {
                    interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
            }
        }
        else if (tutorialLevel == 2)
        {
            if (dialogueNum == 1)
            {
                if (linesLeft == 5)
                {
                    interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    interactivePanel.transform.Find("Seesaw Arrow 4").gameObject.SetActive(true);
                }
                else if (linesLeft == 3)
                {
                    interactivePanel.transform.Find("Seesaw Arrow 4").gameObject.SetActive(false);
                    interactivePanel.transform.Find("Seesaw Arrow 5").gameObject.SetActive(true);
                }
                
            }

            if (dialogueNum == 2)
            {
                if (linesLeft == 1)
                {
                    interactivePanel.transform.Find("Seesaw Arrow 5").gameObject.SetActive(false);
                    interactivePanel.transform.Find("Seesaw Arrow 6").gameObject.SetActive(true);
                }
            }

            if (dialogueNum == 3)
            {
                if (linesLeft == 2)
                {
                    interactivePanel.transform.Find("Seesaw Arrow 8").gameObject.SetActive(true);
                }
            }
        }
        else if (tutorialLevel == 6)
        {
            if (dialogueNum == 1)
            {
                if (linesLeft == 3)
                {
                    interactivePanel.transform.Find("Seesaw Arrow 10").gameObject.SetActive(true);
                }
                else if (linesLeft == 1)
                {
                    interactivePanel.transform.Find("Seesaw Arrow 10").gameObject.SetActive(false);
                }
            }
        }
        else if (tutorialLevel == 16)
        {
            if (dialogueNum == 1)
            {
                if (linesLeft == 4)
                {
                    interactivePanel.transform.Find("Seesaw Arrow 11").gameObject.SetActive(false);
                    interactivePanel.transform.Find("Seesaw Arrow 4").gameObject.SetActive(true);
                }
                if (linesLeft == 2)
                {
                    interactivePanel.transform.Find("Seesaw Arrow 4").gameObject.SetActive(false);
                }
            }
        }
    }

    // tutorial 1
    public void DraggedCorrectly()
    {
        Debug.Log("Completed First Task");
        // interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(false);
        interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(false);
        dialogueNum = 2;
        dialogueTriggers[1].TriggerDialogue();
    }

    // tutorial 2
    public void DraggedToyOver()
    {
        Debug.Log("Completed First Task");
        // interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 5").gameObject.SetActive(false);
        dialogueNum = 2;
        dialogueTriggers[3].TriggerDialogue();
    }

    public void DraggedToyOver2()
    {
        Debug.Log("Completed Second Task");
        // interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 6").gameObject.SetActive(false);
        dialogueNum = 3;
        dialogueTriggers[4].TriggerDialogue();
    }

    // tutorial 6
    public void DoubleClicked()
    {
        dialogueTriggers[1].TriggerDialogue();
        interactivePanel.transform.Find("Seesaw Arrow 7").gameObject.SetActive(false);
        interactivePanel.transform.Find("Seesaw Arrow 8").gameObject.SetActive(false);
    }

    public void Combined()
    {
        dialogueTriggers[2].TriggerDialogue();
        interactivePanel.transform.Find("Seesaw Arrow 9").gameObject.SetActive(false);
    }

    // tutorial 11
    public void StartedBSO()
    {
        Debug.Log("Started Panel");

        if (waitForFirstDrag)
        {
            // interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            dialogueTriggers[4].TriggerDialogue();
            interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(false);
            waitForFirstDrag = false;
            Debug.Log("Did the thing");
        }
    }

    public void PressedOperation()
    {
        Debug.Log("Pressed Operation");

        if (waitForSecondDrag)
        {
            Debug.Log("Pressed Operation");

            // interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(false);
            dialogueTriggers[5].TriggerDialogue();
            waitForSecondDrag = false;
        }
    }

    public void StartedNumber()
    {
        if (waitForThirdDrag)
        {
            // interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(false);
            dialogueTriggers[6].TriggerDialogue();
            waitForThirdDrag = false;
        }
    }

    // tutorial 16
    public void FirstDrop()
    {
        Debug.Log("First drop");
        // interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 4").gameObject.SetActive(false);
        interactivePanel.transform.Find("Seesaw Arrow 5").gameObject.SetActive(false);
        waitForFirstDrag = false;
        dialogueTriggers[8].TriggerDialogue();
    }

    public void SuccessfullyExpanded()
    {
        finishedSecondDrag = true;
    }

    public void Expanded()
    {
        // interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 4").gameObject.SetActive(false);
        interactivePanel.transform.Find("Seesaw Arrow 6").gameObject.SetActive(false);
        waitForSecondDrag = false;
        dialogueTriggers[9].TriggerDialogue();
    }

    public void SkipTutorial()
    {
        if (! doneTutorial)
        {
            interactivePanel.SetActive(false);
            tutorialManager.EndDialogueNow();
            skipButton.SetActive(false);
            doneTutorial = true;
        }
    }

}
