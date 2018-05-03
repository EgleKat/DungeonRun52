using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

	public List<GameObject> bullets;

	[HideInInspector] public int currGun = 0;

	private Vector2 fireVector;
	private float nextFire = 0;

	private Rigidbody2D rb;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update () {

		//Set firing direction
		if (Input.GetButtonDown("FireRight")) {
			fireVector = new Vector2(1, 0);
		} else if (Input.GetButtonDown("FireUp")) {
			fireVector = new Vector2(0, 1);
		} else if (Input.GetButtonDown("FireLeft")) {
			fireVector = new Vector2(-1, 0);
		} else if (Input.GetButtonDown("FireDown")) {
			fireVector = new Vector2(0, -1);
		}

		//Fire if can
		if (Time.time >= nextFire && (Input.GetButton("FireRight") || Input.GetButton("FireUp") || Input.GetButton("FireLeft") || Input.GetButton("FireDown"))) {
			FireBullet(currGun);
		}

	}

	private void FireBullet(int gunID) {
		GameObject firedBullet;
		switch (gunID) {
			case 0:
				firedBullet = Instantiate(bullets[0], transform.position, Quaternion.identity);
				firedBullet.GetComponent<PlayerBullet>().moveVector = rb.velocity * 0.2f + fireVector * 4;
				nextFire = Time.time + 0.5f;
				break;
		}
	}

}
