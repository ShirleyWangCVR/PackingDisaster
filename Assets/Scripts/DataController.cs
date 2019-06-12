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
    public GameObject bearPrefab;
    public GameObject boxPrefab;
    public GameObject bearCoefPrefab;
    public GameObject boxCoefPrefab;
    public GameObject bracketPrefab;

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

        // For some reason hardcoding the size at the start fixes resizing
        // issue between computers. It may look useless but removing the
        // sizeDelta setting will cause different resolution screens to render
        // the toys differently.
        // Setting localScale is so the toy scales with the canvas.
        Vector3 scale = FindObjectOfType<Canvas>().gameObject.transform.localScale;
        bearPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        bearPrefab.transform.localScale = scale;
        boxPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        boxPrefab.transform.localScale = scale;

        bearCoefPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(65, 40);
        bearCoefPrefab.transform.localScale = scale;
        boxCoefPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(65, 40);
        boxCoefPrefab.transform.localScale = scale;

        bracketPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 60);
        bracketPrefab.transform.localScale = scale;
    }

    // get current equation to show depending on provided level
    // eventually have just allEquationsUsed[level - 1] once we have 25 equations in the json
    public EquationData GetCurrentEquationData(int level)
    {
        return allEquationsUsed[level - 1];
    }

    public void StartLevel(int level)
    {
        SetDifficulty(level);
        if (level == 1)
        {
            SceneManager.LoadScene(6); // Tut Stage 1
        }
        else if (level == 2)
        {
            SceneManager.LoadScene(7); // Tut Stage 2
        }
        else if (level == 3)
        {
            SceneManager.LoadScene(8); // Tut Stage 3
        }
        else if (level <= 5)
        {
            SceneManager.LoadScene("Main"); // levels 4 or 5
        }
        else if (level == 6)
        {
            SceneManager.LoadScene(9); // Tut Stage 4, coefficients
        }
        else if (level > 6 && level < 11)
        {
            SceneManager.LoadScene("T2Main"); // levels 7 to 10
        }
        else if (level == 11)
        {
            SceneManager.LoadScene(10); // Tut Stage 5, both side operations
        }
        else if (level > 11 && level < 16)
        {
            SceneManager.LoadScene("T2Main"); // levels 12 to 15
        }
        else if (level == 16)
        {
            SceneManager.LoadScene(11); // Tut Stage 6, expanding brackets
        }
        else if (level > 16)
        {
            SceneManager.LoadScene("T3Main"); // levels 17 and above
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
