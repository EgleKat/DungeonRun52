using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [HideInInspector]
    public int objectID;
	
	private GameObject player;
	private Sprite spr;

	private void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player");
        if (gameObject.tag == "Weapon") {
			objectID = Random.Range(1, 5);
			spr = player.GetComponent<CollideWithObject>().HUDWeaponSprites[objectID];
		} else if (gameObject.tag == "Item") {
			objectID = Random.Range(0, 3);
			spr = player.GetComponent<CollideWithObject>().HUDItemSprites[objectID + 1];
		}
		gameObject.GetComponent<SpriteRenderer>().sprite = spr;
    }
    public void UpdateObject(int id)
    {
		Sprite otherSpr = new Sprite();
		if (gameObject.tag == "Weapon") {
			otherSpr = player.GetComponent<CollideWithObject>().HUDWeaponSprites[id];
		} else if (gameObject.tag == "Item") {
			otherSpr = player.GetComponent<CollideWithObject>().HUDItemSprites[id + 1];
		}
		//if the player does not have any items before picking up this one, remove it from the game
		if (id < 0)
            gameObject.SetActive(false);
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = otherSpr;

        //Set the object on the floor id
        objectID = id;
    }
}
