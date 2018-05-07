using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour {

    private Image sr;
    [SerializeField]
    private Sprite[] heartSprites;

	// Use this for initialization
	void Start () {
        sr = GetComponent<Image>();
	}
	
	public void UpdateHeartSprite(int numberOfHearts)
    {
        Debug.Log(numberOfHearts);
        switch (numberOfHearts)
        {
            case 0:
                sr.sprite = heartSprites[0];
                break;
            case 1:
                sr.sprite = heartSprites[1];
                break;
            case 2:
                sr.sprite = heartSprites[3];
                break;
            case 3:
                sr.sprite = heartSprites[4];

                break;
            case 4:
                sr.sprite = heartSprites[5];

                break;
            case 5:
                sr.sprite = heartSprites[6];
                Debug.Log("changed sprite");
                break;
            case 6:
                sr.sprite = heartSprites[7];
                break;
        }
    }
}
