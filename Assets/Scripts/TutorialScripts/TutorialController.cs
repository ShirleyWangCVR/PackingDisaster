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

    private int tutorialLevel;
    private bool waitForFirstDrag;
    private bool waitForSecondDrag;
    private bool waitForThirdDrag;
    private bool waitForFourthDrag;
    private bool waitForFifthDrag;
    private bool finishedSecondDrag;
    
    // Start is called before the first frame update
    void Start()
    {
        tutorialLevel = FindObjectOfType<DataController>().GetDifficulty();

        waitForFirstDrag = false;
        waitForSecondDrag = false;
        waitForThirdDrag = false;
        waitForFourthDrag = false;
        waitForFifthDrag = false;
        finishedSecondDrag = false;

        if (tutorialLevel == 1)
        {
            dialogueTriggers[0].TriggerInitialDialogue();
        }
        else if (tutorialLevel == 2)
        {
            dialogueTriggers[6].TriggerInitialDialogue();
            interactivePanel.transform.Find("Seesaw Arrow 4").gameObject.SetActive(true);
        }
        else if (tutorialLevel == 3)
        {
            dialogueTriggers[9].TriggerInitialDialogue();
        }
        else if (tutorialLevel == 6)
        {
            dialogueTriggers[0].TriggerInitialDialogue();
        }
        else if (tutorialLevel == 11)
        {
            dialogueTriggers[1].TriggerInitialDialogue();
        }
        else if (tutorialLevel == 16)
        {
            GameObject bracket = seesaw.transform.Find("LHSPositive").GetChild(0).gameObject;
            Tut6Bracket check = bracket.AddComponent<Tut6Bracket>();
            dialogueTriggers[5].TriggerInitialDialogue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialLevel == 1)
        {
            if (waitForFirstDrag)
            {
                if (seesaw.GetComponent<TutSeesawController>().CheckOneDraggedUnbalanced())
                {
                    waitForFirstDrag = false;
                    DraggedToOneSide();
                }
            }

            if (waitForSecondDrag)
            {
                if (seesaw.GetComponent<TutSeesawController>().CheckAnotherDraggedBalanced())
                {
                    waitForSecondDrag = false;
                    DraggedToOtherSide();
                }
            }

            if (waitForThirdDrag)
            {
                if (seesaw.GetComponent<TutSeesawController>().CheckDraggedToToyBoxUnBalanced())
                {
                    waitForThirdDrag = false;
                    DraggedToToyBox();
                }
            }

            if (waitForFourthDrag)
            {
                if (seesaw.GetComponent<TutSeesawController>().CheckDraggedFromToyBoxBalanced())
                {
                    waitForFourthDrag = false;
                    DraggedFromToyBox();
                }
            }

            if (waitForFifthDrag)
            {
                if (seesaw.GetComponent<TutSeesawController>().CheckDraggedStillBalanced())
                {
                    waitForFifthDrag = false;
                    DraggedCorrectly();
                }
            }
        }
        else if (tutorialLevel == 2)
        {
            if (waitForFirstDrag)
            {
                if (seesaw.GetComponent<TutSeesawController>().CheckOneMoreBoxOnBothSides())
                {
                    waitForFirstDrag = false;
                    AddedBoxToBothSides();
                }
            }

            if (waitForSecondDrag)
            {
                if (seesaw.GetComponent<TutSeesawController>().CheckCancelledOut())
                {
                    waitForSecondDrag = false;
                    CancelledOutValues();
                }
            }
        }   
        else if (tutorialLevel == 3)
        {
            if (waitForFirstDrag)
            {
                if (seesaw.GetComponent<TutSeesawController>().CheckDraggedToNegative())
                {
                    waitForFirstDrag = false;
                    DraggedToyOver();
                }
            }
        } 
        else if (tutorialLevel == 16)
        {
            if (finishedSecondDrag)
            {
                finishedSecondDrag = false;
                Expanded();
            }
        }    
    }

    public virtual void FinishedFirstDialogue()
    {
        if (tutorialLevel == 1)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
            waitForFirstDrag = true;
        }
        else if (tutorialLevel == 2)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow 4").gameObject.SetActive(false);
            interactivePanel.transform.Find("Seesaw Arrow 5").gameObject.SetActive(true);
            interactivePanel.transform.Find("Seesaw Arrow 6").gameObject.SetActive(true);
            interactivePanel.transform.Find("Seesaw Arrow 7").gameObject.SetActive(true);
            waitForFirstDrag = true;
        }
        else if (tutorialLevel == 3)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow 9").gameObject.SetActive(true);
            waitForFirstDrag = true;
        }
        else if (tutorialLevel == 6)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.SetActive(false);
            StartCoroutine(tutorialManager.EndDialogue());
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

    public virtual void FinishedSecondDialogue()
    {  
        if (tutorialLevel == 1)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
            interactivePanel.transform.Find("Seesaw Arrow").localScale = new Vector2(0 - interactivePanel.transform.Find("Seesaw Arrow").localScale.x, interactivePanel.transform.Find("Seesaw Arrow").localScale.y);
            waitForSecondDrag = true;
        }
        else if (tutorialLevel == 2)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow 8").gameObject.SetActive(true);
            waitForSecondDrag = true;
        }
        else if (tutorialLevel == 3)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow 10").gameObject.SetActive(true);
            StartCoroutine(tutorialManager.EndDialogue());
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

    public virtual void FinishedThirdDialogue()
    {
        if (tutorialLevel == 1)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
            waitForThirdDrag = true;
        }
        else if (tutorialLevel == 2)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.SetActive(false);
            StartCoroutine(tutorialManager.EndDialogue());
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
        }
    }

    public virtual void FinishedFourthDialogue()
    {
        if (tutorialLevel == 1)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
            interactivePanel.transform.Find("Seesaw Arrow 2").localScale = new Vector2(0 - interactivePanel.transform.Find("Seesaw Arrow 2").localScale.x, interactivePanel.transform.Find("Seesaw Arrow 2").localScale.y);
            waitForFourthDrag = true;
        }
        else if (tutorialLevel == 11)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.SetActive(false);
            StartCoroutine(tutorialManager.EndDialogue());
        }
    }

    public virtual void FinishedFifthDialogue()
    {
        if (tutorialLevel == 1)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
            interactivePanel.transform.Find("Seesaw Arrow 2").localScale = new Vector2(0 - interactivePanel.transform.Find("Seesaw Arrow 2").localScale.x, interactivePanel.transform.Find("Seesaw Arrow 2").localScale.y);
            interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(true);
            
            waitForFifthDrag = true;
        }
    }

    public virtual void FinishedSixthDialogue()
    {
        if (tutorialLevel == 1)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(false);
            interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(false);
            interactivePanel.transform.Find("Seesaw Arrow 11").gameObject.SetActive(true);
            StartCoroutine(tutorialManager.EndDialogue());
        }
    }
    
    // tutorial 1
    public void DraggedToOneSide()
    {
        Debug.Log("Completed First Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(false);
        dialogueTriggers[1].TriggerDialogue();
    }

    public void DraggedToOtherSide()
    {
        Debug.Log("Completed Second Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(false);
        dialogueTriggers[2].TriggerDialogue();
    }

    public void DraggedToToyBox()
    {
        Debug.Log("Completed Third Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(false);
        dialogueTriggers[3].TriggerDialogue();
    }

    public void DraggedFromToyBox()
    {
        Debug.Log("Completed Fourth Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(false);
        dialogueTriggers[4].TriggerDialogue(); 
    }

    public void DraggedCorrectly()
    {
        Debug.Log("Completed Fifth Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 5").gameObject.SetActive(false);
        interactivePanel.transform.Find("Seesaw Arrow 6").gameObject.SetActive(false);
        dialogueTriggers[5].TriggerDialogue();
    }
    
    // tutorial 2
    public void AddedBoxToBothSides()
    {
        Debug.Log("Completed First Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 5").gameObject.SetActive(false);
        interactivePanel.transform.Find("Seesaw Arrow 6").gameObject.SetActive(false);
        interactivePanel.transform.Find("Seesaw Arrow 7").gameObject.SetActive(false);
        dialogueTriggers[7].TriggerDialogue();
    }

    public void CancelledOutValues()
    {
        Debug.Log("Completed Second Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 8").gameObject.SetActive(false);
        dialogueTriggers[8].TriggerDialogue();
    }
    
    // tutorial 3
    public void DraggedToyOver()
    {
        Debug.Log("Completed First Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 9").gameObject.SetActive(false);
        dialogueTriggers[10].TriggerDialogue();
    }

    // tutorial 5
    public void StartedBSO()
    {
        Debug.Log("Started Panel");
        
        if (waitForFirstDrag)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            dialogueTriggers[2].TriggerDialogue();
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

            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(false);
            dialogueTriggers[3].TriggerDialogue();
            waitForSecondDrag = false;
        }
    }

    public void StartedNumber()
    {
        if (waitForThirdDrag)
        {
            interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(false);
            dialogueTriggers[4].TriggerDialogue();
            waitForThirdDrag = false;
        }
    }

    // tutorial 6
    public void FirstDrop()
    {
        Debug.Log("First drop");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 4").gameObject.SetActive(false);
        interactivePanel.transform.Find("Seesaw Arrow 5").gameObject.SetActive(false);
        waitForFirstDrag = false;
        dialogueTriggers[6].TriggerDialogue();
    }

    public void SuccessfullyExpanded()
    {
        finishedSecondDrag = true;
    }

    public void Expanded()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 4").gameObject.SetActive(false);
        interactivePanel.transform.Find("Seesaw Arrow 6").gameObject.SetActive(false);
        waitForSecondDrag = false;
        dialogueTriggers[7].TriggerDialogue();
    }
    
}
