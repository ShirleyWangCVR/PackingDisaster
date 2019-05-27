using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquationController : MonoBehaviour
{
    
    public EquationData[] allEquationsUsed;
    private int currentDifficulty;
    private int equationsCompleted;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Menu");
        currentDifficulty = 0;
        equationsCompleted = 0;
    }

    public EquationData GetCurrentEquationData(int difficulty)
    {
        return allEquationsUsed[0];
    }

    public int GetDifficulty()
    {
        return currentDifficulty;
    }

    public void SetDifficulty(int difficulty)
    {
        currentDifficulty = difficulty;
    }

    public int GetEquationsCompleted()
    {
        return equationsCompleted;
    }

    public void SetEquationsCompleted(int newNum)
    {
        equationsCompleted = newNum;
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
