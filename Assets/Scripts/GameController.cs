using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController instance = null;
	public int playerCurrentGun;
	public int playerCurrentItem;
	public static float dmgMultiplier = 1;

	private void Start() {
		DontDestroyOnLoad(gameObject);
	}

	private void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}
}
