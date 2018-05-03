using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public bool locked = false;
	public Vector3 endPos;

	private GameObject player;

	private void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void LockToPoint(Vector2 point) {
		locked = true;
		endPos = new Vector3(point.x, point.y, transform.position.z);
	}

	void Update() {
		if (locked) {
			transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime * 2.5f);
		} else {
			Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
			transform.position = Vector3.Lerp(transform.position, playerPos, Time.deltaTime * 2.5f);
		}

		if (Input.GetKeyDown("l")) { locked = !locked; }
	}
}
