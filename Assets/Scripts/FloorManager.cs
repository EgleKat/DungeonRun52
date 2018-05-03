using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour {

	public List<GameObject> roomList;
	public Sprite[] lockSpriteList;
	public Vector2 gridSize;
	public int maxRooms;

	[HideInInspector] public int currLoc = -10;
	[HideInInspector] public int[,] floorGrid;

	private List<SimRoom> asr;
	private List<int> rrl;

	private CameraMovement cm;
	private GameObject player;
	private List<GameObject> rooms = new List<GameObject>();
	private Dictionary<int, Vector2> roomSizes;

	public class SimRoom {

		public int border = 4;
		public int index = -1;
		public int x, y, w, h;
		public float x1, x2, y1, y2;

		public SimRoom(float x, float y, float w, float h) : this((int)x, (int)y, (int)w, (int)h) { }

		public SimRoom(int x, int y, int w, int h) {
			this.x = x;
			this.y = y;
			this.w = w;
			this.h = h;

			x1 = x - w / 2f;
			x2 = x + w / 2f;
			y1 = y - h / 2f;
			y2 = y + h / 2f;
		}

		public bool Intersect(SimRoom other) {
			return (x1 <= other.x2 + border && x2 >= other.x1 - border && y1 <= other.y2 + border && other.y2 >= other.y1 - border);
		}

		public float Distance(SimRoom other) {
			return Mathf.Sqrt(Mathf.Pow(x - other.x, 2) + Mathf.Pow(y - other.y, 2));
		}

	}

	//List shuffle function - found online
	//https://stackoverflow.com/questions/273313/randomize-a-listt
	public List<T> ShuffleList<T>(List<T> list) {
		int n = list.Count;
		while (n > 1) {
			n--;
			int k = UnityEngine.Random.Range(0, n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
		return list;
	}

	private void Start() {
		cm = Camera.main.gameObject.GetComponent<CameraMovement>();
		player = GameObject.FindGameObjectWithTag("Player");
		LayoutFloor();
		for (int i = 0; i<rooms.Count; i++) {
			InitRoom(i);
		}
	}

	//Get size of every room in roomList
	public void GetRoomSizes() {
		roomSizes = new Dictionary<int, Vector2>();
		for (int i = 0; i<roomList.Count; i++) {
			GameObject newRoom = Instantiate(roomList[i], new Vector3(100, 100, 100), Quaternion.identity);
			roomSizes.Add(i, newRoom.GetComponent<RoomMain>().roomSize);
			Destroy(newRoom);
		}
	}

	public void PaintGrid(int value, int x, int y, int w, int h) {
		for (int i = x; i < x + w; i++) {
			for (int j = y; j < y + h; j++) {
				floorGrid[i, j] = value;
			}
		}
	}

	public void LayoutFloor() {

		//Initialise grid
		GetRoomSizes();
		int[] gs = new int[2] { (int) Mathf.Round(gridSize.x), (int)Mathf.Round(gridSize.y) };
		floorGrid = new int[gs[0], gs[1]];
		PaintGrid(0, 0, 0, gs[0], gs[1]);
		rrl = new List<int>();
		for (int i = 1; i<3; i++) {
			rrl.Add(i);
		}
		rrl = ShuffleList(rrl);

		//Place vital rooms first
		asr = new List<SimRoom>();
		Vector2 randPos = Random.insideUnitCircle * Mathf.Min(gs[0], gs[1])/2;
		randPos = new Vector2(Mathf.Round(randPos.x + gs[0]/2), Mathf.Round(randPos.y + gs[1]/2));
		asr.Add(new SimRoom(randPos.x, randPos.y, roomSizes[0].x, roomSizes[0].y));
		asr[0].index = 0;

		PlaceRoomDistanced(10, Mathf.Min(gs[0], gs[1])/2, asr[0]);
		for (int r = 0; r<maxRooms; r++) {
			PlaceRoomRegular(rrl[r]);
		}

		//Build rooms if can
		if (asr.Count >= maxRooms/1.5f + 2) {
			foreach (SimRoom simRoom in asr) {
				rooms.Add(Instantiate(roomList[simRoom.index], new Vector3(simRoom.x * 0.64f, simRoom.y * 0.64f, 0), Quaternion.identity));
			}
			player.transform.position = new Vector3(asr[0].x * 0.64f, asr[0].y * 0.64f, 0);
			cm.JumpToPoint(new Vector2(asr[0].x * 0.64f, asr[0].y * 0.64f));
		} else {
			LayoutFloor();
		}
	}

	public void PlaceRoomRegular(int i) {
		PlaceRoomDistanced(i, 0, new SimRoom(-1, -1, 0, 0));
	}

	public void PlaceRoomDistanced(int i, float minDist, SimRoom other) {
		GameObject roomObj = roomList[i];
		int[] gs = new int[2] { (int)Mathf.Round(gridSize.x), (int)Mathf.Round(gridSize.y) };
		int repeats = 0;
		while (repeats < 1000000) {
			Vector2 randPos = new Vector2(Random.Range(0, gs[0]), Random.Range(0, gs[1]));
			SimRoom simRoom = new SimRoom(randPos.x, randPos.y, roomSizes[i].x, roomSizes[i].y);

			bool srBool = false;
			foreach (SimRoom sr in asr) {
				if (simRoom.Intersect(sr)) {
					srBool = true;
				}
			}
			if (simRoom.Distance(other) < minDist) { srBool = true; }
			
			//Add room if acceptable
			if (!srBool) {
				simRoom.index = i;
				asr.Add(simRoom);
				break;
			}
			repeats++;
		}
	}

	public void InitRoom(int i) {
		RoomMain rm = rooms[i].GetComponent<RoomMain>();
		rm.locID = i;
		rm.passages = new HashSet<int>();
		rm.passages.Add(0);
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
