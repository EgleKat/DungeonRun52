using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour {

	protected GameObject player;
	protected SpriteRenderer sr;
	protected Rigidbody2D rb;

	private void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		StartExtended();
	}

	private void Update() {
		sr.sortingOrder = (int) (-transform.position.y * 100);
		UpdateExtended();
	}

	public virtual void StartExtended() {}
	public virtual void UpdateExtended() {}

}
