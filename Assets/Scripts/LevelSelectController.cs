using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// optimize this at a later date once we figure out levels

public class LevelSelectController : MonoBehaviour
{
    private DataController dataController;

    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
