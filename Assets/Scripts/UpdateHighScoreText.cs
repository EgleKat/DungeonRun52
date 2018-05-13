using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpdateHighScoreText : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Text>().text = "High Score: " + GameController.GetHighScore();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
