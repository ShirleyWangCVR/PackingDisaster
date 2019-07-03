using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    // public Image star1;
    // public Image star2;
    // public Image star3;
    public Image showStars;
    public Image gearBack;
    public Image gearFront;
    public GameObject rack;
    public Sprite[] stars;
    // public Sprite fullStar;
    // public Sprite emptyStar;

    private bool tutorial;
    private int currentStars;
    private float currentTime;
    private float currentLength;
    private float initialX;
    private float start;
    private float end;
    private float constantBy;
    private DataController dataController;
    private GameController gameController;
    
    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        gameController = FindObjectOfType<GameController>();
        initialX = rack.transform.position.x;
        start = this.transform.Find("Start").position.x;
        end = this.transform.Find("End").position.x;
        constantBy = (start - end) / 120;

        currentTime = 0;
        currentStars = 3;

        tutorial = dataController.GetDifficulty() == 1 || dataController.GetDifficulty() == 2 || dataController.GetDifficulty() == 6 || dataController.GetDifficulty() == 11 || dataController.GetDifficulty() == 16;
    }

    // Update is called once per frame
    void Update()
    {
        if (! tutorial)
        {
            currentTime += Time.deltaTime;
            currentLength = Mathf.Round(currentTime) * constantBy;

            if (currentTime > 30 && currentStars == 3)
            {
                currentStars = 2;
                showStars.sprite = stars[2];
            }
            if (currentTime > 60 && currentStars == 2)
            {
                currentStars = 1;
                showStars.sprite = stars[1];
            }
            if (currentTime > 90 && currentStars == 1)
            {
                currentStars = 0;
                showStars.sprite = stars[0];
            }

            if (currentTime <= 120)
            {
                rack.transform.position = new Vector3(initialX - currentLength, rack.transform.position.y, rack.transform.position.z);
                
                // make this more jittery to match the rack
                gearBack.transform.eulerAngles = new Vector3(0, 0, -2f * Mathf.Round(currentTime));
                gearFront.transform.eulerAngles = new Vector3(0, 0, -2f * Mathf.Round(currentTime));

                // gearBack.transform.Rotate(0, 0, -0.1f, Space.Self);
                // gearFront.transform.Rotate(0, 0, -0.1f, Space.Self);
            }

            /* currentLength = (int) (Mathf.Round(currentTime) * 5 / 3);

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
            } */
        }
    }

    public int FinishedGameGetStars()
    {
        tutorial = false;
        return currentStars;
    }
}
