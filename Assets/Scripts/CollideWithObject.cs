using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollideWithObject : MonoBehaviour
{
    private HeartDisplay heartDisplay;
    private PlayerShoot shooter;
    private PlayerItem item;
    bool collidingWithEnemy = false;
    public bool shield = false;
    MusicManager musicManager;
    private Image itemHud;
    private Image weaponHud;

    public Sprite[] HUDWeaponSprites;
    public Sprite[] HUDItemSprites;
    public bool revive = false;
    private bool invincible = false;

    private GameObject restartButton;

    // Use this for initialization
    void Start()
    {
        heartDisplay = GameObject.Find("Hearts").GetComponent<HeartDisplay>();
        musicManager = GameObject.Find("Music Manager").GetComponent<MusicManager>();
        shooter = gameObject.GetComponent<PlayerShoot>();
        item = gameObject.GetComponent<PlayerItem>();
        itemHud = GameObject.Find("Item Image").GetComponent<Image>();
        weaponHud = GameObject.Find("Weapon Image").GetComponent<Image>();

        restartButton = GameObject.Find("Game Over Button");
        restartButton.SetActive(false);


        //INITIALISE ITEM
        ChangeItemHUD(GameController.playerCurrentItem);
        //INITIALISE WEAPON
        ChangeWeaponHUD(GameController.playerCurrentGun);
        //INITIALISE HEARTS
        heartDisplay.UpdateHeartSprite(GameController.playerCurrentHealth);


        //show level text (didn't know where to put this)
        GameObject.Find("Level Name").GetComponent<FadeInOut>().ShowText("Dungeon " + GameController.currentLevel);



    }
    private void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            collidingWithEnemy = true; //used to periodically decrease GameController.playerCurrentHealth
            DamageToPlayer();
        }
        else if (collision.gameObject.tag == "Ladder")
        {
            //Restart level
            Debug.Log("Restarting room");
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            //set current high score
            GameController.currentRunScore += GameObject.FindGameObjectWithTag("ScoreText").GetComponent<HighScoreTimer>().currentIntScore;
            Debug.Log("Current Run Score: " + GameController.currentRunScore);
            //Set current level
            GameController.currentLevel++;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {

            //play sound
            musicManager.PlaySound("weaponSwap");
            //change weapon
            ObjectSpawn objectSpawn = collision.gameObject.GetComponent<ObjectSpawn>();
            int previousWeaponID = GameController.playerCurrentGun;
            shooter.SetGun(objectSpawn.objectID);
            GameController.playerCurrentGun = objectSpawn.objectID;
            //drop other weapon
            objectSpawn.UpdateObject(previousWeaponID);

            //update hud
            ChangeWeaponHUD(GameController.playerCurrentGun);

        }
        else if (collision.gameObject.tag == "Item")
        {
            ObjectSpawn objectSpawn = collision.gameObject.GetComponent<ObjectSpawn>();
            int previousItemID = GameController.playerCurrentItem;
            item.ActivateItem(objectSpawn.objectID);
            GameController.playerCurrentItem = objectSpawn.objectID;
            //Play Sound
            musicManager.PlaySound("itemSwap");
            //drop other item
            objectSpawn.UpdateObject(previousItemID);

            //update hud
            ChangeItemHUD(GameController.playerCurrentItem);

        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
            collidingWithEnemy = false;

    }
    private void DamageToPlayer()
    {
        if (collidingWithEnemy && !invincible)
        {
            if (shield)
            {
                item.ShieldHit();
            }
            else
            {
                GameController.playerCurrentHealth--;
                musicManager.PlaySound("hit");
                heartDisplay.UpdateHeartSprite(GameController.playerCurrentHealth);
                if (GameController.playerCurrentHealth == 0)
                {
                    //revive player
                    if (revive)
                    {
                        //make a sound
                        musicManager.PlaySound("windChime");
                        //animation
                        invincible = true;
                        gameObject.GetComponent<Animator>().SetTrigger("reviving");
                        //set hearts
                        GameController.playerCurrentHealth = 3;
                        //update heart display
                        heartDisplay.UpdateHeartSprite(GameController.playerCurrentHealth);
                        //remove item from game
                        item.RemoveHUDItemFromGame();
                        revive = false;
                        Invoke("SetInvincibleToFalse", 0.7f);

                    }
                    //Game Over
                    else
                    {
                        restartButton.SetActive(true);
                        GameObject.Find("Music Manager").GetComponent<MusicManager>();
                        GameObject overTextObject = GameObject.FindGameObjectWithTag("Game Over");
                        overTextObject.GetComponent<FadeInOut>().StartFadeIn();
                        // Disable player
                        gameObject.SetActive(false);

                        HighScoreTimer highScoreTimer = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<HighScoreTimer>();
                        //disable counting
                        highScoreTimer.countdown = false;
                        //update high score, level
                        GameController.SetHighScore(highScoreTimer.currentIntScore);
                        GameController.SetHighestLevel();
                        GameController.ResetGameValues();
                        //play a sound

                    }
                }
            }

            Invoke("DamageToPlayer", 0.7f);
        }
    }
    public void ChangeItemHUD(int id)
    {
        itemHud.sprite = HUDItemSprites[id + 1];

    }
    public void ChangeWeaponHUD(int id)
    {
        weaponHud.sprite = HUDWeaponSprites[id];

    }
    private void SetInvincibleToFalse()
    {
        invincible = false;
    }


}
