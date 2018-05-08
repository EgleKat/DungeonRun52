using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [HideInInspector]
    public int objectID;

    public Sprite[] sprites;
    static System.Random random = new System.Random();

    private void Start()
    {
        if (gameObject.tag == "Weapon")
            objectID = random.Next(1, 4);
        else if (gameObject.tag == "Item")
            objectID = random.Next(0, 3);
        //objectID = 1;

        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[objectID];

    }
    public void UpdateObject(int id)
    {
        //if the player does not have any items before picking up this one, remove it from the game
        if (id < 0)
            gameObject.SetActive(false);
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[id];

        //Set the object on the floor id
        objectID = id;
    }
}
