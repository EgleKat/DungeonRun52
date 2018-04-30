using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	// Constants
	private Rigidbody2D rb;

	private float moveSpeed = 4;
	private Vector2 inputVector;
	private bool diagMove;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		//Get input
		inputVector = new Vector2(Input.GetAxis("HorizontalMove"), Input.GetAxis("VerticalMove"));
		diagMove = Mathf.Abs(inputVector.x * inputVector.y) != 0;
	}

	private void FixedUpdate() {
		rb.velocity = moveSpeed * inputVector * (diagMove ? 1 / Mathf.Sqrt(2) : 1);
	}
}
