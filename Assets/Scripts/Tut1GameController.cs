using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Game Controller for the main scene where the question is solved.
 */
public class Tut1GameController : GameController
{    
    // other variables inherited from GameController
    public TutorialManager tutorialManager;
    public DialogueModuleManager dialogueModuleManager;
    public DialogueTrigger dialogueTrigger1;
    public DialogueTrigger dialogueTrigger2;
    public DialogueTrigger dialogueTrigger3;
    public DialogueTrigger dialogueTrigger4;
    public DialogueTrigger dialogueTrigger5;
    public DialogueTrigger dialogueTrigger6;
    public GameObject interactivePanel;

    private bool waitForFirstDrag;
    private bool waitForSecondDrag;
    private bool waitForThirdDrag;
    private bool waitForFourthDrag;
    private bool waitForFifthDrag;

    // Start is called before the first frame update
    void Start()
    {
        // get data from dataController
        dataController = FindObjectOfType<DataController>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        level = dataController.GetDifficulty();
        equation = dataController.GetCurrentEquationData(level);
        timeUsed = 0;
        
        // set up seesaw according to equation
        SetUpSeesaw();

        // set up tutorial dialogue
        isRoundActive = false;
        waitForFirstDrag = false;
        waitForSecondDrag = false;
        waitForThirdDrag = false;
        waitForFourthDrag = false;
        waitForFifthDrag = false;

        dialogueTrigger1.TriggerInitialDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForFirstDrag)
        {
            if (seesaw.GetComponent<Tut1SeesawController>().CheckOneDraggedUnbalanced())
            {
                waitForFirstDrag = false;
                DraggedToOneSide();
            }
        }

        if (waitForSecondDrag)
        {
            if (seesaw.GetComponent<Tut1SeesawController>().CheckAnotherDraggedBalanced())
            {
                waitForSecondDrag = false;
                DraggedToOtherSide();
            }
        }

        if (waitForThirdDrag)
        {
            if (seesaw.GetComponent<Tut1SeesawController>().CheckDraggedToToyBoxUnBalanced())
            {
                waitForThirdDrag = false;
                DraggedToToyBox();
            }
        }

        if (waitForFourthDrag)
        {
            if (seesaw.GetComponent<Tut1SeesawController>().CheckDraggedFromToyBoxBalanced())
            {
                waitForFourthDrag = false;
                DraggedFromToyBox();
            }
        }

        if (waitForFifthDrag)
        {
            if (seesaw.GetComponent<Tut1SeesawController>().CheckDraggedStillBalanced())
            {
                waitForFifthDrag = false;
                DraggedCorrectly();
            }
        }
        
        // no timer on tutorials
        // no tip over on tutorials
    }

    public void FinishedFirstDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
        waitForFirstDrag = true;
    }
    
    public void DraggedToOneSide()
    {
        Debug.Log("Completed First Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(false);
        dialogueTrigger2.TriggerDialogue();
    }

    public void FinishedSecondDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
        interactivePanel.transform.Find("Seesaw Arrow").localScale = new Vector2(0 - interactivePanel.transform.Find("Seesaw Arrow").localScale.x, interactivePanel.transform.Find("Seesaw Arrow").localScale.y);
        waitForSecondDrag = true;
    }

    public void DraggedToOtherSide()
    {
        Debug.Log("Completed Second Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(false);
        dialogueTrigger3.TriggerDialogue();
    }

    public void FinishedThirdDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
        waitForThirdDrag = true;
    }

    public void DraggedToToyBox()
    {
        Debug.Log("Completed Third Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(false);
        dialogueTrigger4.TriggerDialogue();
    }

    public void FinishedFourthDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
        interactivePanel.transform.Find("Seesaw Arrow 2").localScale = new Vector2(0 - interactivePanel.transform.Find("Seesaw Arrow 2").localScale.x, interactivePanel.transform.Find("Seesaw Arrow 2").localScale.y);
        waitForFourthDrag = true;
    }

    public void DraggedFromToyBox()
    {
        Debug.Log("Completed Fourth Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(false);
        dialogueTrigger5.TriggerDialogue(); 
    }

    public void FinishedFifthDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
        interactivePanel.transform.Find("Seesaw Arrow 2").localScale = new Vector2(0 - interactivePanel.transform.Find("Seesaw Arrow 2").localScale.x, interactivePanel.transform.Find("Seesaw Arrow 2").localScale.y);
        interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(true);
        
        waitForFifthDrag = true;
    }

    public void DraggedCorrectly()
    {
        Debug.Log("Completed Fourth Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(false);
        interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(false);
        dialogueTrigger6.TriggerDialogue();
    }

    public void FinishedSixthDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    // set up seesaw using GameController setup
    
}
