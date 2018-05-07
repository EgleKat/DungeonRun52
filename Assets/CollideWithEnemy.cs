using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWithEnemy : MonoBehaviour
{
    private int health = 6;
    public HeartDisplay heartDisplay;
    bool collidingWithEnemy;
    MusicManager musicManager;
    // Use this for initialization
    void Start()
    {
        heartDisplay = GameObject.Find("Hearts").GetComponent<HeartDisplay>();
        musicManager = GameObject.Find("Music Manager").GetComponent<MusicManager>();

        collidingWithEnemy = false;
    }
    private void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collidingWithEnemy = true;
            DamageToPlayer();
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
