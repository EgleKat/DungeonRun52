﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWithObject : MonoBehaviour
{
    private int health = 6;
    private HeartDisplay heartDisplay;
    private PlayerShoot shooter;
    private PlayerItem item;
    bool collidingWithEnemy;
    public bool shield = false;
    MusicManager musicManager;
    // Use this for initialization
    void Start()
    {
        heartDisplay = GameObject.Find("Hearts").GetComponent<HeartDisplay>();
        musicManager = GameObject.Find("Music Manager").GetComponent<MusicManager>();
        shooter = gameObject.GetComponent<PlayerShoot>();
        item = gameObject.GetComponent<PlayerItem>();

        collidingWithEnemy = false;
    }
    private void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy" )
        {
            collidingWithEnemy = true; //used to periodically decrease health
            DamageToPlayer();
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {

            //update hud
            //play sound
            musicManager.PlaySound("weaponSwap");
            //change weapon
            ObjectSpawn objectSpawn = collision.gameObject.GetComponent<ObjectSpawn>();
            int previousWeaponID = shooter.currGun;
            shooter.currGun = objectSpawn.objectID;
            //drop other weapon
            objectSpawn.UpdateObject(previousWeaponID);
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

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
            collidingWithEnemy = false;

    }
    private void DamageToPlayer()
    {
        if (collidingWithEnemy)
        {
            if(shield)
            {
                item.ShieldHit();
                musicManager.PlaySound("shieldHit");

            }
            else
            {
                health--;
                musicManager.PlaySound("hit");
                heartDisplay.UpdateHeartSprite(health);
                if (health != 0)
                {
                    //Game Over
                }
            }
           
            Invoke("DamageToPlayer", 1f);
        }
    }

}
