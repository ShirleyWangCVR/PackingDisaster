using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

/* Main Data Controller for game.
 * Contains and stores all data that needs to persist between scenes.
 */
public class DataController : MonoBehaviour
{
    // list of all equations from the loaded json. For now just 1.
    public EquationData[] allEquationsUsed;
    public DialogueData dialogue;

    private int currentDifficulty;
    private int equationsCompleted;
    // Player Progress used to store between sessions. Currently only in use for storing high scores.
    private PlayerProgress playerProgress;
    private string equationDataFileName = "equations.json";
    private string dialogueDataFileName = "dialogueData.json";
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadGameData();
        LoadDialogueData();
        LoadPlayerProgress();
        SceneManager.LoadScene("Menu");
        currentDifficulty = 0;
        equationsCompleted = 0;
    }

    // get current equation to show depending on provided difficulty
    public EquationData GetCurrentEquationData()
    {   
        if (currentDifficulty == 0)
        {
            return allEquationsUsed[0];
        }
        else if (currentDifficulty == 6) {
            // for testing
            return allEquationsUsed[1];
        }
        else {
            return allEquationsUsed[0];
        }
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
    

    // submit a new score and store it if it's the highest
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

    // load current player progress
    private void LoadPlayerProgress()
    {
        playerProgress = new PlayerProgress();

        if (PlayerPrefs.HasKey("highestScore"))
        {
            playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
        }
    }

    // save current player progress in player prefs
    private void SavePlayerProgress()
    {
        PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
    }

    // load game data from json
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

    // load dialogue data from json
    private void LoadDialogueData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, dialogueDataFileName);

        if (File.Exists(filePath))
        {
            string jsonDialogueData = File.ReadAllText(filePath);
            dialogue = JsonUtility.FromJson<DialogueData>(jsonDialogueData);

        } else {
            Debug.LogError("Cannot Load Dialogue Data");
        }
    }
}
