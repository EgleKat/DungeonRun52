using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    PlayerShoot shooter;
    public int gunID;

    public Sprite[] sprites;

    // Use this for initialization
    void Start()
    {
        shooter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>();

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void UpdateWeapon(int id)
    {
        gunID = id;
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[id];
    }
}
