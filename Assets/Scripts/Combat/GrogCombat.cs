using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrogCombat : EnemyCombat {

	public float nextAttack = 0;
	public int currAttack = 0;

	private Slider slider;

	private Vector3 chargeVector;
	private float chargeTime;
	private float chargeSpeed = 7f;

	public override void StartExtended() {
		slider = GameObject.FindGameObjectWithTag("FloorManager").GetComponent<FloorManager>().slider.GetComponent<Slider>();
	}

	public override void UpdateExtended() {
		sr.flipX = player.transform.position.x > transform.position.x;

		if (locID == fm.currLoc) {
			if (nextAttack == 0) {
				// Init
				nextAttack = Time.time + 3 + Random.Range(0f, 2f);
			} else if (currAttack == 0 && Time.time >= nextAttack) {
				currAttack = 1;
				rb.velocity = Vector2.zero;
				chargeVector = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y, 0);
				nextAttack = Time.time + 0.4f;
				chargeTime = chargeVector.magnitude / chargeSpeed;
			} else if (currAttack == 1 && Time.time >= nextAttack) {
				currAttack = 2;
				nextAttack = Time.time + chargeTime;
			} else if (currAttack == 2 && Time.time >= nextAttack) {
				currAttack = 0;
				nextAttack = Time.time + 3 + Random.Range(0f, 2f);
			}

			slider.value = (hp / maxHP);
		}
	}

	private void FixedUpdate() {
		if (locID == fm.currLoc) {
			if (currAttack == 0) {
				rb.velocity = rb.velocity * 0.9f +
				new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized * 0.15f;
			} else if (currAttack == 2) {
				rb.velocity = chargeVector.normalized * chargeSpeed;
			}
		}
	}

	public override void Deathrattle() {
		slider.value = 0;
		Vector3 ladderPos = new Vector3(fm.rooms[1].transform.position.x, fm.rooms[1].transform.position.y, 0);
		GameObject go = GameObject.Instantiate(transform.GetChild(0).gameObject, ladderPos, Quaternion.identity);
		go.SetActive(true);
	}
}
