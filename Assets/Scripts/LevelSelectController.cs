using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// optimize this at a later date once we figure out levels
public class LevelSelectController : MonoBehaviour
{
    public GameObject[] levelButtons;
    public Sprite[] stars;
    
    private DataController dataController;
    private int level;

    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        int levelsShow = dataController.GetLevelsCompleted();

        // eventually set up buttons according to how much they've completed.
        for (int i = 0; i < 25; i++)
        {
            levelButtons[i].transform.Find("Number").gameObject.GetComponent<Text>().text = (i + 1).ToString();

            int levelStars = dataController.GetStars(i + 1);
            levelButtons[i].transform.Find("Stars").gameObject.GetComponent<Image>().sprite = stars[levelStars];

            // check if i > levels completed, if so set not interactable and hide num and stars
            // uncomment this when actual people play, keep this comment for easy testing
            /* if (i > levelsShow)
            {
                levelButtons[i].GetComponent<Button>().interactable = false;
                levelButtons[i].transform.Find("Number").gameObject.SetActive(false);
                levelButtons[i].transform.Find("Stars").gameObject.SetActive(false);
            } */
        }
    }

    public void StartLevel(int level)
    {
        // eventually load tutorial scenes accordingly
        dataController.SetDifficulty(level);
        dataController.StartLevel(level);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
