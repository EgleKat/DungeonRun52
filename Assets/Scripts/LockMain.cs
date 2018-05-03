using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMain : MonoBehaviour {

	private Sprite[] spriteList = new Sprite[3];

	//0-Up 1-Right 2-Down 3-Left
	public int dirID;

	//0-Normal 1-Open 2-Sealed
	public int stateID;

	private SpriteRenderer sr;
	private Collider2D coll;
	private FloorManager fm;

	public void StartExtended() {
		sr = GetComponent<SpriteRenderer>();
		coll = GetComponent<Collider2D>();
		fm = GameObject.FindGameObjectWithTag("FloorManager").GetComponent<FloorManager>();
		spriteList[0] = fm.lockSpriteList[0];
		spriteList[1] = fm.lockSpriteList[1];
		spriteList[2] = fm.lockSpriteList[2];
	}

	public void SetState(int state) {

		stateID = state;
		sr.sprite = spriteList[stateID];
		coll.enabled = (stateID != 1);

	}
}
