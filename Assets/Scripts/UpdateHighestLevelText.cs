using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpdateHighestLevelText : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Text>().text = "Highest Level Reached: " + GameController.GetHighestLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
