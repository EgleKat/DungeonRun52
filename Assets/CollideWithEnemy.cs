using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWithEnemy : MonoBehaviour
{
    private int health = 6;
    private HeartDisplay heartDisplay;
    private PlayerShoot shooter;
    bool collidingWithEnemy;
    MusicManager musicManager;
    // Use this for initialization
    void Start()
    {
        heartDisplay = GameObject.Find("Hearts").GetComponent<HeartDisplay>();
        musicManager = GameObject.Find("Music Manager").GetComponent<MusicManager>();
        shooter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>();

        collidingWithEnemy = false;
    }
    private void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided");

        if (collision.gameObject.tag == "Enemy")
        {
            collidingWithEnemy = true;
            DamageToPlayer();
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered");

        if (collision.gameObject.tag == "Weapon")
        {

            //update hud
            //change weapon
            WeaponSpawn weaponSpawnCollided = collision.gameObject.GetComponent<WeaponSpawn>();
            int previousWeaponID = shooter.currGun;
            shooter.currGun = weaponSpawnCollided.gunID;
            //drop other weapon
            weaponSpawnCollided.UpdateWeapon(previousWeaponID);
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
            health--;
            musicManager.PlaySound("hit");
            heartDisplay.UpdateHeartSprite(health);
            if (health != 0)
            {
                //Game Over
            }
            Invoke("DamageToPlayer", 1f);
        }
    }

}
