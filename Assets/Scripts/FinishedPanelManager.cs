﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Controller for the Finished Panel at the end of every level.
 */
public class FinishedPanelManager : MonoBehaviour
{
    public GameObject finishedDisplay;
    public Text userMessage;
    public Button nextQuestion;
    public GameObject boxDisplay;
    public GameObject toyDisplay;
    public SimpleObjectPool toyPool;
    public DataController dataController;
    public AudioClip youWinSfx;
    public AudioClip youLoseSfx;
    public Image[] starsDisplay;
    public Sprite fullStar;

    private AudioSource audioSource;

    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // set finished display to if player lost by time out
    /* public void DisplayTimeOut()
    {
        audioSource.PlayOneShot(youLoseSfx, 1.0f);
        
        finishedDisplay.SetActive(true);
        userMessage.text = "You Ran Out of Time\nYou Lose!";
    } */

    // set finished display to if player wins
    public void DisplayCorrectlyBalanced(int correctValue, int stars)
    {
        audioSource.PlayOneShot(youWinSfx, 1.0f);
        
        finishedDisplay.SetActive(true);
        userMessage.text = "You Determined Correctly " + correctValue.ToString() + " in the Box!" + "You Win!";
        nextQuestion.gameObject.SetActive(true);
        // boxDisplay.SetActive(true);
        finishedDisplay.transform.Find("Question").gameObject.SetActive(false);

        for (int i = 0; i < stars; i++)
        {
            starsDisplay[i].sprite = fullStar;
        }

        if (dataController.GetDifficulty() < 6)
        {        
            if (correctValue > 0)
            {
                for (int i = 0; i < correctValue; i++)
                {
                    GameObject toy = toyPool.GetObject();
                    toy.transform.SetParent(toyDisplay.transform);
                    toy.GetComponent<Draggable>().ShowOnPositiveSide();
                }
            }  
            else if (correctValue < 0)
            {
                for (int i = 0; i < 0 - correctValue; i++)
                {
                    GameObject toy = toyPool.GetObject();
                    toy.transform.SetParent(toyDisplay.transform);

                    toy.GetComponent<Draggable>().ShowOnNegativeSide();
                }
            }     
        } 
        else
        {
            Destroy(toyDisplay.GetComponent<GridLayoutGroup>());
            
            GameObject toy = toyPool.GetObject();
            toy.transform.localScale = 2 * toy.transform.localScale;
            toy.transform.SetParent(toyDisplay.transform);
            toy.transform.position = toyDisplay.transform.position;
            toy.transform.Find("Coefficient").gameObject.GetComponent<Coefficient>().SetValue(correctValue);
            if (correctValue > 0)
            {
                toy.GetComponent<Draggable>().ShowOnPositiveSide();
            }
            else 
            {
                toy.GetComponent<Draggable>().ShowOnNegativeSide();
            }
        }
    }

    // set finished panel to if player lost by wrong answer
    public void DisplayWrongBalanced(int determined)
    {
        audioSource.PlayOneShot(youLoseSfx, 1.0f);
        
        finishedDisplay.SetActive(true);
        userMessage.text = "You Didn't Determine the Right Number of Toys. Try Again!";
    }

    // set finished panel to if player didn't fully isolate answer
    public void DisplayNotYetBalanced()
    {
        audioSource.PlayOneShot(youLoseSfx, 1.0f);
        
        finishedDisplay.SetActive(true);
        userMessage.text = "You Didn't Fully Isolate One Box. Try Again!";
    }

    // set finished panel to if player lost by too unbalanced
    public void DisplaySeesawTipped()
    {
        audioSource.PlayOneShot(youLoseSfx, 1.0f);
        
        finishedDisplay.SetActive(true);
        userMessage.text = "The Seesaw Tipped Over! Try Again!";
    }

}
