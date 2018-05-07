using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

	//Bullet stats
	public float dmg;
	public float lifetime;
	public bool piercing;
	public float weight;
	public float rps;

	[HideInInspector] public Vector2 moveVector;

	//Lifetime variables
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	private float deathTime;
	private List<GameObject> hitList;

	private void Start () {
		deathTime = Time.time + lifetime;
		hitList = new List<GameObject>();
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
	}

	private void Update () {
		if (Time.time > deathTime) {
			GameObject.Destroy(gameObject);
		} else {
			sr.sortingOrder = (int)(-transform.position.y * 100);
		}
		Vector3 trl = transform.rotation.eulerAngles;
		transform.Rotate(0, 0, rps * 360 * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other) {
		switch (other.tag) {
			case "Wall":
				GameObject.Destroy(gameObject);
				break;
			case "Enemy":
				if (piercing) {
					if (!hitList.Contains(other.gameObject)) {
						hitList.Add(other.gameObject);
						EnemyCombat ec = other.gameObject.GetComponent<EnemyCombat>();
						ec.GetHit(dmg, moveVector * weight);
					}
				} else {
					EnemyCombat ec = other.gameObject.GetComponent<EnemyCombat>();
					ec.GetHit(dmg, moveVector * weight);
					GameObject.Destroy(gameObject);
				}
				break;
		}
	}

	private void FixedUpdate() {
		rb.MovePosition(rb.position + moveVector * Time.deltaTime);
	}
}
