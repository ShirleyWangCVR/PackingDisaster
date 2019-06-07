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
    // list of all equations from the loaded json. 
    public EquationData[] allEquationsUsed;
    public DialogueData dialogue;

    private int currentLevel; // current level clicked on level select screen
    
    private int levelsCompleted; // use this to set how many levels available on level select
    
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
        currentLevel = 1;
        levelsCompleted = 0;
    }

    // get current equation to show depending on provided level
    // eventually have just allEquationsUsed[level] once we have 25 equations in the json
    public EquationData GetCurrentEquationData(int level)
    {   
        if (level == 0)
        {
            return allEquationsUsed[0];
        }
        else if (level == 6) {
            // for testing
            return allEquationsUsed[1];
        }
        else if (level == 16) {
            return allEquationsUsed[2];
        }
        else {
            return allEquationsUsed[0];
        }
    }

    public int GetDifficulty()
    {
        return currentLevel;
    }

    public void SetDifficulty(int difficulty)
    {
        currentLevel = difficulty;
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

    
    // methods past this point work but currently not in use

    public int GetLevelsCompleted()
    {
        return levelsCompleted;
    }

    public void SetLevelsCompleted(int newNum)
    {
        levelsCompleted = newNum;
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

    // save current player progress in player prefs
    private void SavePlayerProgress()
    {
        PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
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
}
