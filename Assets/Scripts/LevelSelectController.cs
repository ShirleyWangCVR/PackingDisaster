using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// optimize this at a later date once we figure out levels

public class LevelSelectController : MonoBehaviour
{
    private DataController dataController;
    private int level;

    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();

        // eventually set up buttons according to how much they've completed.
    }

    public void StartLevel(int level)
    {
        dataController.SetDifficulty(level);
        
        if (level <= 5)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            SceneManager.LoadScene("T2Main");
        }
    }



    public void ToLevel0()
    {
        dataController.SetDifficulty(0);
        SceneManager.LoadScene("Main");
    }

    public void ToLevel8()
    {
        dataController.SetDifficulty(8);
        SceneManager.LoadScene("T2Main");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
