using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance = null;
    public static int playerCurrentGun;     //updated every time a gun is swapped
    public static int playerCurrentItem;    //updated every time iten is changed

    public static int playerCurrentHealth;
    public static float dmgMultiplier = 1;

    private static int highScore;           //updated at the END of the GAME
    public static int currentRunScore;      //updated at the END of the level

    public static int currentLevel;
    private static int highestLevel;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        ResetGameValues();
        highScore = 0;

    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void SetHighScore(int newHighScore)
    {
        if (highScore < newHighScore)
            highScore = newHighScore;
    }
    public static int GetHighScore()
    {
        return highScore;
    }

    public static void ResetGameValues()
    {
        currentRunScore = 0;
        playerCurrentHealth = 6;
        playerCurrentGun = 0;
        currentLevel = 1;
        playerCurrentItem = -1;
    }

    public static int GetHighestLevel()
    {
        return highestLevel;
    }

    public static void SetHighestLevel()
    {
        if (highestLevel < currentLevel)
            highestLevel = currentLevel;
    }


}
