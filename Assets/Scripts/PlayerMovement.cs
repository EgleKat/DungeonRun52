using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	// Constants
	private Rigidbody2D rb;
	private Animator anim;
	private SpriteRenderer sr;

	private float moveSpeed = 4;
	private Vector2 inputVector;
	private bool diagMove;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
	}

	private void Update() {
		//Get input
		inputVector = new Vector2(Input.GetAxis("HorizontalMove"), Input.GetAxis("VerticalMove"));
		diagMove = Mathf.Abs(inputVector.x * inputVector.y) != 0;

		//Animator
		int moveID = 0;
		if (inputVector.x < 0) {
			moveID = 1;
		} else if (inputVector.x > 0) {
			moveID = 2;
		} else if (inputVector.y < 0) {
			moveID = 3;
		} else if (inputVector.y > 0) {
			moveID = 4;
		}
		anim.SetInteger("moveID", moveID);
		sr.sortingOrder = (int)(-transform.position.y * 100);
	}

	private void FixedUpdate() {
		rb.velocity = moveSpeed * inputVector * (diagMove ? 1 / Mathf.Sqrt(2) : 1);
		rb.rotation = 0;
	}

}