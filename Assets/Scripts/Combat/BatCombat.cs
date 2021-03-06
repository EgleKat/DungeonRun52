﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCombat : EnemyCombat {

	public float moveSpeed;

	public override void StartExtended() {

	}

	public override void UpdateExtended() {
		sr.flipX = player.transform.position.x > transform.position.x;
	}

	private void FixedUpdate() {
		if (locID == fm.currLoc) {
			rb.velocity = rb.velocity * 0.9f +
				new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized * moveSpeed * 0.1f;
		}
	}
}
