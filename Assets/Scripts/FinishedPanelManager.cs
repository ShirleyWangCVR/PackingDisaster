using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishedPanelManager : MonoBehaviour
{
    
    public GameObject finishedDisplay;
    public Text userMessage;
    public Button nextQuestion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayTimeOut()
    {
        finishedDisplay.SetActive(true);
        userMessage.text = "You Ran Out of Time\nYou Lose!";
    }

    public void DisplayCorrectlyBalanced(int correctValue)
    {
        finishedDisplay.SetActive(true);
        userMessage.text = "You Determined Correctly " + correctValue.ToString() + " in the Box!" + "\nYou Win!";
        nextQuestion.gameObject.SetActive(true);
    }

    public void DisplayWrongBalanced(int determined)
    {
        finishedDisplay.SetActive(true);
        userMessage.text = "You Determined Wrongly " + determined.ToString() + " in the Box!" + "\nYou Lose!";
    }

    public void DisplayNotYetBalanced()
    {
        finishedDisplay.SetActive(true);
        userMessage.text = "You Didn't Properly Isolate the Box!\nYou Lose!";
    }

    public void DisplaySeesawTipped()
    {
        finishedDisplay.SetActive(true);
        userMessage.text = "The Seesaw Tipped Over!\nYou Lose!";
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void TryAgain()
    {
        // restart scene with the same equation
        SceneManager.LoadScene("Main");
    }

    public void NextQuestion()
    {
        // restart scene, tell EquationController to move to next question
        SceneManager.LoadScene("Main");
    }
}
