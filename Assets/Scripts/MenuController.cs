using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Controller for the Menu
 */
public class MenuController : MonoBehaviour
{
    // when start button is pressed
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
}
