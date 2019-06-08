using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Game Controller for the main scene where the question is solved.
 */
public class Tut3GameController : TutGameController
{    
    // other variables inherited from GameController
/*     public TutorialManager tutorialManager; */
    // public DialogueModuleManager dialogueModuleManager;
    public DialogueTrigger dialogueTrigger1;
    public DialogueTrigger dialogueTrigger2;
    public DialogueTrigger dialogueTrigger3;
    // public DialogueTrigger dialogueTrigger4;
    // public DialogueTrigger dialogueTrigger5;
    // public DialogueTrigger dialogueTrigger6;
    // public DialogueTrigger dialogueTrigger7;
/*     public GameObject interactivePanel; */

    private bool waitForFirstDrag;
    // private bool waitForSecondDrag;
    // private bool waitForThirdDrag;
    // private bool waitForFourthDrag;
    // private bool waitForFifthDrag;

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
        // waitForSecondDrag = false;
        // waitForThirdDrag = false;
        // waitForFourthDrag = false;
        // waitForFifthDrag = false;

        dialogueTrigger1.TriggerInitialDialogue();
        // interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForFirstDrag)
        {
            if (seesaw.GetComponent<Tut3SeesawController>().CheckDraggedToNegative())
            {
                waitForFirstDrag = false;
                DraggedToyOver();
            }
        }

    }

    public override void FinishedFirstDialogue()
    {
        // Debug.Log("Finished first dialogue");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(true);
        // interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
        // interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(true);
        // interactivePanel.transform.Find("Seesaw Arrow 4").gameObject.SetActive(true);
        waitForFirstDrag = true;
    }

    public void DraggedToyOver()
    {
        Debug.Log("Completed First Task");
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        interactivePanel.transform.Find("Seesaw Arrow").gameObject.SetActive(false);
        // interactivePanel.transform.Find("Seesaw Arrow 3").gameObject.SetActive(false);
        // interactivePanel.transform.Find("Seesaw Arrow 4").gameObject.SetActive(false);
        dialogueTrigger2.TriggerDialogue();
    }

    public override void FinishedSecondDialogue()
    {
        interactivePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        interactivePanel.transform.Find("Seesaw Arrow 2").gameObject.SetActive(true);
        // waitForSecondDrag = true;
    }
    
    // set up seesaw using GameController setup
    public override void EndRound(string howEnded)
    {
        // deactivate game logic
        isRoundActive = false;
        // playerScore = (int) Mathf.Round(timeRemaining);
        // dataController.SubmitNewPlayerScore(playerScore);
        // int highestScore = dataController.GetHighestPlayerScore();

        if (howEnded == "Finished Check") 
        {
            if (seesaw.GetComponent<SeesawController>().CheckIfComplete())
            {
                if (seesaw.GetComponent<SeesawController>().CorrectlyBalanced())
                {
                    finishedDisplayManager.DisplayCorrectlyBalanced(equation.variableValue);
                    // dialogueTrigger7.TriggerDialogue();
                } 
                else 
                {
                    // lost because wrong answer, get whatever they answered
                    int side = (int) seesaw.GetComponent<SeesawController>().GetLeftHandSideValue();
                    if (equation.variableValue != side)
                    {
                        finishedDisplayManager.DisplayWrongBalanced(side);
                    } else {
                        side = (int) seesaw.GetComponent<SeesawController>().GetRightHandSideValue();
                        finishedDisplayManager.DisplayWrongBalanced(side);
                    }
                }
            }
            else 
            {
                finishedDisplayManager.DisplayNotYetBalanced();
            }
        } else if (howEnded == "Scale Tipped") 
        {
            finishedDisplayManager.DisplaySeesawTipped();
        }
    }
    
}
