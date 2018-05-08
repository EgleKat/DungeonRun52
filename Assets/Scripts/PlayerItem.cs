using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{

    private PlayerMovement movement;

    [HideInInspector]
    public int currItemID = -1;
    private Rigidbody2D rb;
    private MusicManager musicManager;
    private int shieldCharges;
    private CollideWithObject collideWithObject;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        musicManager = GameObject.Find("Music Manager").GetComponent<MusicManager>();
        movement = gameObject.GetComponent<PlayerMovement>();
        collideWithObject = gameObject.GetComponent<CollideWithObject>();

    }

    private void Update()
    {


    }

    public void ActivateItem(int id)
    {

        currItemID = id;
        //boots
        if (id == 0)
        {
            movement.moveSpeedMultiplier = 2;
        }
        //shield
        else if (id == 1)
        {
            shieldCharges = 2;
            collideWithObject.shield = true;

        }
        //revive
        else if (id == 3)
        {

        }
    }

    public void ShieldHit()
    {
        shieldCharges--;
        //shield depleted
        if (shieldCharges == 0)
            collideWithObject.shield = false;
    }
}
