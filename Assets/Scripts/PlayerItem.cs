using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerItem : MonoBehaviour
{

    private PlayerMovement movement;
    private GameController gameController;



    private MusicManager musicManager;
    private int shieldCharges;
    private CollideWithObject collideWithObject;
    private GameObject infoTextObject;



    private void Start()
    {

        musicManager = GameObject.Find("Music Manager").GetComponent<MusicManager>();
        movement = gameObject.GetComponent<PlayerMovement>();
        collideWithObject = gameObject.GetComponent<CollideWithObject>();
        infoTextObject = GameObject.FindGameObjectWithTag("InfoText");
        ActivateItem(GameController.playerCurrentItem);

    }

    public void ActivateItem(int id)
    {


        //boots
        if (id == 0)
        {
            movement.moveSpeedMultiplier = 2;
            infoTextObject.GetComponent<FadeInOut>().ShowText("Speed Boots");
        }
        //shield
        else if (id == 1)
        {
            shieldCharges = 2;
            collideWithObject.shield = true;
            infoTextObject.GetComponent<FadeInOut>().ShowText("Shield");

        }
        //revive
        else if (id == 2)
        {
            collideWithObject.revive = true;
            infoTextObject.GetComponent<FadeInOut>().ShowText("Reviving Potion");

        }
        //broken shield
        else if (id == 3)
        {
            shieldCharges = 1;
            collideWithObject.shield = true;
            infoTextObject.GetComponent<FadeInOut>().ShowText("Broken Shield");

        }
        GameController.playerCurrentItem = id;




        //if it's not boots, remove speed
        if (id != 0) movement.moveSpeedMultiplier = 1;
        //remove revive
        else if (id != 2) collideWithObject.revive = false;


    }

    public void ShieldHit()
    {
        shieldCharges--;

        //shield depleted
        if (shieldCharges == 0)
        {
            musicManager.PlaySound("glassBreak");
            collideWithObject.shield = false;
            RemoveHUDItemFromGame();
        }
        //shield has one charge
        else if (shieldCharges == 1)
        {
            musicManager.PlaySound("shieldHit");
            //change hud to half a shield
            collideWithObject.ChangeItemHUD(3);
            GameController.playerCurrentItem = 3;

        }
    }

    public void RemoveHUDItemFromGame()
    {
        collideWithObject.ChangeItemHUD(-1);
        GameController.playerCurrentItem = -1;

    }
}
