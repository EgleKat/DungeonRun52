using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    [HideInInspector]
    public int gunID;

    public Sprite[] sprites;
    static System.Random random = new System.Random();

    private void Start()
    {
        gunID = random.Next(1, 4);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[gunID];

    }
    public void UpdateWeapon(int id)
    {
        gunID = id;
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[id];
    }
}
