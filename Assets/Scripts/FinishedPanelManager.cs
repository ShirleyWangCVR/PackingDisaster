using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishedPanelManager : MonoBehaviour
{
    
    public GameObject finishedDisplay;
    public Text userMessage;
    public Text equalsText;
    public Button nextQuestion;
    public GameObject boxDisplay;
    public GameObject toyDisplay;
    public SimpleObjectPool boxPool;
    public SimpleObjectPool toyPool;

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

    public void DisplayCorrectlyBalanced(int correctValue, int playerScore)
    {
        finishedDisplay.SetActive(true);
        userMessage.text = "You Determined Correctly " + correctValue.ToString() + " in the Box!" + "\nYou Win!";
        nextQuestion.gameObject.SetActive(true);
        GameObject box = boxPool.GetObject();
        box.transform.SetParent(boxDisplay.transform);
        equalsText.text = "=";

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
