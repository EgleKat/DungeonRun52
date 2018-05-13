using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTimer : MonoBehaviour
{

    private float currentScore = 200;       //keep track of current score in float
    public int currentIntScore;            //used to display score on screen and to write globally
    public bool countdown;
    // Use this for initialization
    void Start()
    {
        currentScore += GameController.currentRunScore;
        currentIntScore = (int)currentScore;
        ChangeScoreText();
        countdown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScore > 0 && countdown)
        {
            currentScore -= Time.deltaTime;
            currentIntScore = (int)currentScore;
            ChangeScoreText();
        }

    }

    private void ChangeScoreText()
    {
        gameObject.GetComponent<Text>().text = "Score: " + currentIntScore;
    }
}
