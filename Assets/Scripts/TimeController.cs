using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public Image star1;
    public Image star2;
    public Image star3;
    public GameObject coverTime;
    // public Sprite fullStar;
    public Sprite emptyStar;

    private bool active;
    private int currentStars;
    private float currentTime;
    private int currentLength;
    private DataController dataController;
    private GameController gameController;
    
    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        gameController = FindObjectOfType<GameController>();
        currentTime = 0;
        currentStars = 3;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            currentTime += Time.deltaTime;
            currentLength = (int) (Mathf.Round(currentTime) * 5 / 3);

            if (currentTime > 30 && currentStars == 3)
            {
                currentStars = 2;
                star3.sprite = emptyStar;
            }
            if (currentTime > 60 && currentStars == 2)
            {
                currentStars = 1;
                star2.sprite = emptyStar;
            }
            if (currentTime > 90 && currentStars == 1)
            {
                currentStars = 0;
                star1.sprite = emptyStar;
            }

            if (currentLength <= 200)
            {
                coverTime.GetComponent<RectTransform>().sizeDelta = new Vector2(currentLength, 30);
            }
        }
    }

    public int FinishedGameGetStars()
    {
        active = false;
        return currentStars;
    }
}
