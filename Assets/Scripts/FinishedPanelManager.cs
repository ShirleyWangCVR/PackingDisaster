using System.Collections;
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
    public SimpleObjectPool boxPool;
    public SimpleObjectPool toyPool;

    // set finished display to if player lost by time out
    public void DisplayTimeOut()
    {
        finishedDisplay.SetActive(true);
        userMessage.text = "You Ran Out of Time\nYou Lose!";
    }

    // set finished display to if player wins
    public void DisplayCorrectlyBalanced(int correctValue, int playerScore)
    {
        finishedDisplay.SetActive(true);
        userMessage.text = "You Determined Correctly " + correctValue.ToString() + " in the Box!" + "\nYou Win!";
        nextQuestion.gameObject.SetActive(true);
        boxDisplay.SetActive(true);

        // also set playerScore and highScore at some point        

        if (correctValue > 0)
        {
            for (int i = 0; i < correctValue; i++)
            {
                GameObject toy = toyPool.GetObject();
                toy.transform.SetParent(toyDisplay.transform);

                int check = (int) Mathf.Round(toy.transform.localScale.x);
                if (check == -1)
                {
                    toy.transform.localScale = new Vector3(1, 1, 1);
                    toy.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }
            }
        }
        // also set negative orientation one   
        else if (correctValue < 0)
        {
            for (int i = 0; i < 0 - correctValue; i++)
            {
                GameObject toy = toyPool.GetObject();
                toy.transform.SetParent(toyDisplay.transform);

                int check = (int) Mathf.Round(toy.transform.localScale.x);
                if (check == 1)
                {
                    toy.transform.localScale = new Vector3(-1, -1, 1);
                    toy.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                }
            }
        }     

    }

    // set finished panel to if player lost by wrong answer
    public void DisplayWrongBalanced(int determined)
    {
        finishedDisplay.SetActive(true);
        userMessage.text = "You Determined Wrongly " + determined.ToString() + " in the Box!" + "\nYou Lose!";
    }

    // set finished panel to if player didn't fully isolate answer
    public void DisplayNotYetBalanced()
    {
        finishedDisplay.SetActive(true);
        userMessage.text = "You Didn't Properly Isolate the Box!\nYou Lose!";
    }

    // set finished panel to if player lost by too unbalanced
    public void DisplaySeesawTipped()
    {
        finishedDisplay.SetActive(true);
        userMessage.text = "The Seesaw Tipped Over!\nYou Lose!";
    }

    // pressed go back to main menu button
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // pressed try question again button
    public void TryAgain()
    {
        // restart scene with the same equation
        SceneManager.LoadScene("Main");
    }

    // move onto next question
    public void NextQuestion()
    {
        // tell DataController to move to next question and then load main scene again
        SceneManager.LoadScene("Main");
    }
}
