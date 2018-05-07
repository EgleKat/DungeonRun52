using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomPitRandomise : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int r = Random.Range(0, transform.childCount);
		GetComponent<SpriteRenderer>().sprite = transform.GetChild(r).gameObject.GetComponent<SpriteRenderer>().sprite;
	}
}