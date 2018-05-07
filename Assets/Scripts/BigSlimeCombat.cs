using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlimeCombat : EnemyCombat {

	public float moveSpeed;

	private GameObject ls;

	public override void StartExtended() {
		ls = transform.GetChild(0).gameObject;
	}

	public override void UpdateExtended() {
		sr.flipX = player.transform.position.x > transform.position.x;
	}

	public override void Deathrattle() {
		for (int i = 0; i<2; i++) {
			GameObject lsNew = Instantiate(ls, new Vector3(transform.position.x - 0.24f + i * 0.48f, transform.position.y, 0), Quaternion.identity);
			EnemyCombat lsCombat = lsNew.GetComponent<EnemyCombat>();
			lsNew.SetActive(true);
			lsNew.transform.localScale = new Vector3(3, 3, 1);
			lsCombat.locID = locID;
			lsCombat.rm = rm;
			rm.enemies.Add(lsNew);
		}
	}

	private void FixedUpdate() {
		if (locID == fm.currLoc) {
			rb.velocity = rb.velocity * 0.8f +
				new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized * moveSpeed * 0.2f;
		}
	}
}
