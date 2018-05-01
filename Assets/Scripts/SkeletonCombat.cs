using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCombat : EnemyCombat {

	public float moveSpeed;

	public override void UpdateExtended() {
		sr.flipX = player.transform.position.x > transform.position.x;
	}

	private void FixedUpdate() {
		rb.velocity = rb.velocity * 0.8f + 
			new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized * moveSpeed * 0.2f;
	}

	

}
