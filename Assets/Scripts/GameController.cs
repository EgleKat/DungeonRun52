using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance = null;
    public int playerCurrentGun;
    public int playerCurrentItem;
    public static float dmgMultiplier = 1;

    private static int highScore;           //updated at the END of the GAME
    public static int currentRunScore;      //updated at the END of the level

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        currentRunScore = 0;
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
}
