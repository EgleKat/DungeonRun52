using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryText : MonoBehaviour
{

    private RectTransform textTransform;
    bool move = true;
    private float moveSpeed;
    private FadeInOut fadeInStartText;

    // Use this for initialization
    void Start()
    {
        textTransform = gameObject.GetComponent<RectTransform>();
        fadeInStartText = GameObject.Find("StartText").GetComponent<FadeInOut>();
        //string storyText = "Long ago";
        moveSpeed = 130;

        fadeInStartText.timeToFade = 2f;
        fadeInStartText.fadeOutAfterSeconds = 6f;
        fadeInStartText.ShowText("Press 'Space' to start");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
            LoadNextScene();

        if (move)
        {
            float yPos = textTransform.localPosition.y;
            if (yPos < 1800)
                textTransform.localPosition = new Vector3(textTransform.localPosition.x, (yPos + (moveSpeed * Time.deltaTime)));
            else
            {
                move = false;
                Invoke("LoadNextScene", 1.5f);
            }

        }
    }



    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
