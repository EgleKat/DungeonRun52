using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour {

	public float maxHP;

	[HideInInspector] public float hp;
	[HideInInspector] public RoomMain rm;
	[HideInInspector] public int locID;

	protected GameObject player;
	protected FloorManager fm;
	protected SpriteRenderer sr;
	protected Rigidbody2D rb;
	protected bool dead = false;

	//Hurt variables
	protected bool hurt = false;
	protected float hurtTime;

	private void Start() {
		hp = maxHP;
		player = GameObject.FindGameObjectWithTag("Player");
		fm = GameObject.FindGameObjectWithTag("FloorManager").GetComponent<FloorManager>();
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		StartExtended();
	}

	private void Update() {
		sr.sortingOrder = (int) (-transform.position.y * 100);
		if (hurt = true && Time.time >= hurtTime) {
			hurt = false;
			sr.material.color = Color.white;
		}
		UpdateExtended();
	}

	public void GetHit(float dmg, Vector2 knockback) {
		if (locID == fm.currLoc && !dead) {

			//Adjust variables
			rb.velocity = knockback;
			hp -= dmg;

			//Set hurt
			if (!hurt) {
				hurt = true;
				hurtTime = Time.time + 0.2f;
				sr.material.color = new Color(1.6f, 0.8f, 0.8f);
			}

			//Check if dead
			if (hp <= 0) {
				dead = true;
				Die();
			}
		}
	}

	public void Die() {
		Deathrattle();
		Destroy(gameObject);
		rm.enemies.Remove(gameObject);
		rm.CheckEnemies();
	}

	public virtual void StartExtended() {}
	public virtual void UpdateExtended() {}
	public virtual void Deathrattle() {}

}
