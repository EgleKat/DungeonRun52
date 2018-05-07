﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

	public List<GameObject> bullets;

	[HideInInspector] public int currGun = 0;
	[HideInInspector] public int currItem = -1;

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

		for (int i = 0; i<5; i++) {
			if (Input.GetKeyDown("" + i)) {
				currGun = i;
			}
		}

	}

	private void FireBullet(int gunID) {
		if (gunID == 0) {
			GameObject fb;
			fb = Instantiate(bullets[0], transform.position, Quaternion.identity);
			fb.GetComponent<PlayerBullet>().moveVector = rb.velocity * 0.4f + fireVector * 4;
			nextFire = Time.time + 0.5f;
		} else if (gunID == 1) {
			for (int i = 0; i<3; i++) {
				GameObject fb;
				fb = Instantiate(bullets[1], transform.position, Quaternion.identity);
				fb.GetComponent<PlayerBullet>().moveVector = rb.velocity * 0.2f + fireVector.Rotate(-30 + 30 * i) * 6;
			}
			nextFire = Time.time + 0.8f;
		} else if (gunID == 2) {
			GameObject fb;
			Quaternion rot = (fireVector.y == 0 ? Quaternion.Euler(0, 0, 90) : Quaternion.identity);
			fb = Instantiate(bullets[2], transform.position + new Vector3(fireVector.x, fireVector.y, 0) * 0.5f, rot);
			fb.GetComponent<PlayerBullet>().moveVector = fireVector * 15f;
			nextFire = Time.time + 0.03f;
		} else if (gunID == 3) {
			GameObject fb;
			float spread = Random.Range(-5f, 5f);
			fb = Instantiate(bullets[3], transform.position, Quaternion.identity);
			fb.GetComponent<PlayerBullet>().moveVector = rb.velocity * 0.2f + fireVector.Rotate(spread) * 8;
			nextFire = Time.time + 0.15f;
		} else if (gunID == 4) {
			GameObject fb;
			fb = Instantiate(bullets[4], transform.position, Quaternion.identity);
			fb.GetComponent<PlayerBullet>().moveVector = rb.velocity * 0.2f + fireVector * 2.5f;
			nextFire = Time.time + 2f;
		}
	}

}