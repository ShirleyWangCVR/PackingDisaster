using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquationController : MonoBehaviour
{
    
    public EquationData[] allEquationsUsed;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Menu");
    }

    public EquationData GetCurrentEquationData()
    {
        return allEquationsUsed[0];
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
