using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    public int gunID;

    public Sprite[] sprites;

    public void UpdateWeapon(int id)
    {
        gunID = id;
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[id];
    }
}
