using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class DataController : MonoBehaviour
{
    
    public EquationData[] allEquationsUsed;

    private int currentDifficulty;
    private int equationsCompleted;
    private PlayerProgress playerProgress;
    private string equationDataFileName = "equations.json";
    // private string dialogueDataFileName = "dialogueData.json";
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadGameData();
        LoadPlayerProgress();
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
    

    public void SubmitNewPlayerScore(int newScore)
    {
        if (newScore > playerProgress.highestScore)
        {
            playerProgress.highestScore = newScore;
            SavePlayerProgress();
        }
    }

    public int GetHighestPlayerScore()
    {
        return playerProgress.highestScore;
    }

    private void LoadPlayerProgress()
    {
        playerProgress = new PlayerProgress();

        if (PlayerPrefs.HasKey("highestScore"))
        {
            playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
        }
    }

    private void SavePlayerProgress()
    {
        PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
    }

    private void LoadGameData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, equationDataFileName);

        if (File.Exists(filePath))
        {
            string jsonGameData = File.ReadAllText(filePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(jsonGameData);

            allEquationsUsed = loadedData.equationData;
        } else {
            Debug.LogError("Cannot Load Game Data");
        }
    }
}
