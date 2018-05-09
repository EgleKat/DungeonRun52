using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollideWithObject : MonoBehaviour
{
    private int health = 6;
    private HeartDisplay heartDisplay;
    private PlayerShoot shooter;
    private PlayerItem item;
    bool collidingWithEnemy;
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

        collidingWithEnemy = false;

        ChangeItemHUD(item.currItemID);
        ChangeWeaponHUD(shooter.currGun);
        restartButton = GameObject.Find("Game Over Button");
        restartButton.SetActive(false);

    }
    private void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            collidingWithEnemy = true; //used to periodically decrease health
            DamageToPlayer();
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
            int previousWeaponID = shooter.currGun;
            shooter.SetGun(objectSpawn.objectID);
            //drop other weapon
            objectSpawn.UpdateObject(previousWeaponID);

            //update hud
            ChangeWeaponHUD(shooter.currGun);

        }
        else if (collision.gameObject.tag == "Item")
        {
            ObjectSpawn objectSpawn = collision.gameObject.GetComponent<ObjectSpawn>();
            int previousItemID = item.currItemID;
            item.ActivateItem(objectSpawn.objectID);
            //Play Sound
            musicManager.PlaySound("itemSwap");
            //drop other item
            objectSpawn.UpdateObject(previousItemID);

            //update hud
            ChangeItemHUD(item.currItemID);

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
                health--;
                musicManager.PlaySound("hit");
                heartDisplay.UpdateHeartSprite(health);
                if (health == 0)
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
                        health = 3;
                        //update heart display
                        heartDisplay.UpdateHeartSprite(health);
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
                        overTextObject.GetComponent<FadeInOut>().ShowText("Game Over");
                        // Disable player
                        gameObject.SetActive(false);
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
