using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    public LevelSelectController levelSelectController;
    
    // Start is called before the first frame update
    void Start()
    {
        levelSelectController = FindObjectOfType<LevelSelectController>();
    }

    public void StartLevel()
    {
        int level = Int32.Parse(this.gameObject.name.Substring(5));
        levelSelectController.StartLevel(level);
    }
}
