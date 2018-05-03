using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour {

	public List<GameObject> roomList;
	public Sprite[] lockSpriteList;

	[HideInInspector] public int currLoc = -10;

	private CameraMovement cm;
	private List<GameObject> rooms = new List<GameObject>();

	private void Start() {
		cm = Camera.main.gameObject.GetComponent<CameraMovement>();
		rooms.Add(Instantiate(roomList[0], new Vector3(0, 0, 0), Quaternion.identity));
		rooms.Add(Instantiate(roomList[2], new Vector3(0, 8, 0), Quaternion.identity));
		rooms.Add(Instantiate(roomList[1], new Vector3(0, 16, 0), Quaternion.identity));
		for (int i = 0; i<rooms.Count; i++) {
			InitRoom(i);
		}
	}

	public void InitRoom(int i) {
		RoomMain rm = rooms[i].GetComponent<RoomMain>();
		rm.locID = i;
		rm.passages = new HashSet<int>();
		rm.passages.Add(0);
		rm.passages.Add(2);
		rm.passages.Add(3);
		rm.StartExtended();
	}

	//Change room
	public void ChangeRoom(int locID) {
		currLoc = locID;
		cm.LockToPoint(new Vector2(rooms[locID].transform.position.x, rooms[locID].transform.position.y)); 
		rooms[locID].GetComponent<RoomMain>().CheckEnemies();
	}

	public void UnlockCamera() {
		cm.locked = false;
	}
}
