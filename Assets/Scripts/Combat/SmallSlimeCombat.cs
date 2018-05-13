using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSlimeCombat: EnemyCombat {

	public float moveSpeed;

	public override void StartExtended() {
		rb.velocity = Vector2.zero;
	}

	public override void UpdateExtended() {
		sr.flipX = player.transform.position.x > transform.position.x;
	}

	private void FixedUpdate() {
		if (locID == fm.currLoc) {
			rb.velocity = rb.velocity * 0.85f +
				new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized * moveSpeed * 0.15f;
		}
	}
}
