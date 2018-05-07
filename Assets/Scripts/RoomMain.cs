using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMain : MonoBehaviour {

	public int roomID;
	public Vector2 roomSize;
	public bool bossRoom;

	[HideInInspector] public int locID = -1;
	[HideInInspector] public HashSet<int> passages;
	[HideInInspector] public List<GameObject> enemies;

	protected GameObject player;
	protected FloorManager fm;
	protected List<GameObject> locks;

	private void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		fm = GameObject.FindGameObjectWithTag("FloorManager").GetComponent<FloorManager>();
	}

	public void StartExtended() {
		enemies = new List<GameObject>();
		foreach (Transform t in transform.Find("Enemies")) {
			enemies.Add(t.gameObject);
			EnemyCombat ec = t.gameObject.GetComponent<EnemyCombat>();
			ec.rm = this;
			ec.locID = locID;
		}

		locks = new List<GameObject>();
		foreach (Transform t in transform.Find("Locks")) {
			locks.Add(t.gameObject);
			t.gameObject.GetComponent<LockMain>().StartExtended();
		}

		LockRoom(true);
	}

	void Update() {

		if (locID > -1) {
			//Check if entered room
			float roomW = (roomSize.x - 1) * 0.32f;
			float roomH = (roomSize.y - 1) * 0.32f;
			Vector3 pos = transform.position;
			Vector3 pp = player.transform.position;
			if (fm.currLoc != locID && pp.x > pos.x - roomW && pp.x < pos.x + roomW && pp.y > pos.y - roomH && pp.y < pos.y + roomH) {
				Debug.Log("Entered Room " + locID);
				fm.ChangeRoom(locID);
			}
		}
	}

	public void CheckEnemies() {
		LockRoom(enemies.Count == 0);
		if (enemies.Count == 0) {
			fm.UnlockCamera();
		}
	}

	public void LockRoom(bool open) {
		int s = (open ? 1 : 0);
		foreach (GameObject l in locks) {
			LockMain lm = l.GetComponent<LockMain>();
			if (passages.Contains(lm.dirID)) {
				lm.SetState(s);
			} else {
				lm.SetState(2);
			}
		}
	}

}
