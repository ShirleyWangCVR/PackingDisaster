using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Controller for the Menu
 */
public class MenuController : MonoBehaviour
{
    private DataController dataController;

    public void Start()
    {
        dataController = FindObjectOfType<DataController>();
    }
    
    // when start button is pressed
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("Level Select");
    }
}
