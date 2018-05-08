using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FloorManager : MonoBehaviour {

	public List<GameObject> roomList;
	public Sprite[] lockSpriteList;
	public Vector2 gridSize;
	public int maxRooms;
	public List<GameObject> genAssets;

	[HideInInspector] public int currLoc = -10;
	[HideInInspector] public int[,] floorGrid;

	private List<SimRoom> asr;
	private List<int> rrl;

	private CameraMovement cm;
	private GameObject player;
	private List<GameObject> rooms = new List<GameObject>();
	private Dictionary<int, Vector2> roomSizes;
	private List<PathEdge> pel;

	public class SimRoom {

		public int border = 4;
		public int index = -1;
		public int x, y, w, h;
		public float x1, x2, y1, y2;
		public HashSet<int> passages = new HashSet<int>();

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

		public PathEdge PlanPath(SimRoom other) {
			PathEdge minPath = new PathEdge(index, other.index);

			//Lock locations
			List<Vector2> locks = new List<Vector2>();
			locks.Add(new Vector2(x, y + h/2 + 3));
			locks.Add(new Vector2(x + w/2 + 3, y));
			locks.Add(new Vector2(x, y - h/2 - 3));
			locks.Add(new Vector2(x - w/2 - 3, y));

			//Other lock locations
			List<Vector2> otherLocks = new List<Vector2>();
			otherLocks.Add(new Vector2(other.x, other.y + other.h / 2 + 3));
			otherLocks.Add(new Vector2(other.x + other.w / 2 + 3, other.y));
			otherLocks.Add(new Vector2(other.x, other.y - other.h / 2 - 3));
			otherLocks.Add(new Vector2(other.x - other.w / 2 - 3, other.y));

			//Check shortest lock
			for (int i = 0; i<4; i++) {
				for (int j = 0; j<4; j++) {
					if (Vector2.Distance(locks[i], otherLocks[j]) < minPath.d) {
						minPath.d = Vector2.Distance(locks[i], otherLocks[j]);
						minPath.startLock = i;
						minPath.endLock = j;
					}
				}
			}
			return minPath;
		}
	}

	public class PathEdge {

		public float d;
		public int startRoom, endRoom;
		public int startLock, endLock;

		public PathEdge(int startRoom, int endRoom) : this(100000, startRoom, endRoom, 0, 0) { }

		public PathEdge(float distance, int startRoom, int endRoom, int startLock, int endLock) {
			d = distance;
			this.startRoom = startRoom;
			this.endRoom = endRoom;
			this.startLock = startLock;
			this.endLock = endLock;
		}

		public void Console() {
			Debug.Log("Path from Room " + startRoom + " (Lock " + startLock + ") to Room " + endRoom + " (Lock " + endLock + "): " + d);
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
        //Random.InitState(1);
		cm = Camera.main.gameObject.GetComponent<CameraMovement>();
		player = GameObject.FindGameObjectWithTag("Player");
		GetRoomSizes();
		LayoutFloor();
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
		for (int i = Mathf.Max(x, 0); i < Mathf.Min(x + w, floorGrid.GetLength(0)); i++) {
			for (int j = Mathf.Max(y, 0); j < Mathf.Min(y + h, floorGrid.GetLength(1)); j++) {
				floorGrid[i, j] = value;
			}
		}
	}

	public void PaintGridCareful(int value, int x, int y, int w, int h) {
		for (int i = Mathf.Max(x, 0); i < Mathf.Min(x + w, floorGrid.GetLength(0)); i++) {
			for (int j = Mathf.Max(y, 0); j < Mathf.Min(y + h, floorGrid.GetLength(1)); j++) {
				if (floorGrid[i, j] == 0) {
					floorGrid[i, j] = value;
				}
			}
		}
	}

	public void PaintPath(int value, int border, int x, int y, int w, int h) {
		for (int i = Mathf.Max(x - border, 0); i < Mathf.Min(x + w + border, floorGrid.GetLength(0)); i++) {
			for (int j = Mathf.Max(y - border, 0); j < Mathf.Min(y + h + border, floorGrid.GetLength(1)); j++) {
				if (floorGrid[i, j] == 0) {
					floorGrid[i, j] = value;
				}
			}
		}
	}

	public int FloorGridSafe(int x, int y) {
		int xMax = floorGrid.GetLength(0);
		int yMax = floorGrid.GetLength(1);
		if (x > -1 && x < xMax && y > -1 && y < yMax) {
			return floorGrid[x, y];
		} else {
			return 0;
		}
	}

	public void LayoutFloor() {

		//Initialise grid
		int[] gs = new int[2] { (int) Mathf.Round(gridSize.x), (int)Mathf.Round(gridSize.y) };
		floorGrid = new int[gs[0], gs[1]];
		PaintGrid(0, 0, 0, gs[0], gs[1]);
		rrl = new List<int>();
		for (int i = 1; i<10; i++) {
			if (roomList[i].name != "EmptyRoom") {
				rrl.Add(i);
			}
		}
		rrl = ShuffleList(rrl);

		//Place vital rooms first
		asr = new List<SimRoom>();
		Vector2 randPos = Random.insideUnitCircle * Mathf.Min(gs[0], gs[1])/2;
		randPos = new Vector2(Mathf.Round(randPos.x + gs[0]/2), Mathf.Round(randPos.y + gs[1]/2));
		asr.Add(new SimRoom(randPos.x, randPos.y, roomSizes[0].x, roomSizes[0].y));
		asr[0].index = 0;

		PlaceRoomDistanced(10, Mathf.Max(gs[0], gs[1])/2, asr[0]);
		PlaceRoomRegular(15);
		for (int r = 0; r<maxRooms; r++) {
			PlaceRoomRegular(rrl[r]);
		}

		//Build rooms if can
		if (asr.Count >= maxRooms/1.2f + 3 && asr[1].index == 10 && asr[2].index == 15) {

			// --Connect dungeon--

			// 0. Init algorithm
			pel = new List<PathEdge>();
			for (int i = 0; i<asr.Count - 1; i++) {
				for (int j = i + 1; j< asr.Count; j++) {
					PathEdge pe = asr[i].PlanPath(asr[j]);
					if (pe.startRoom != 0 || pe.endRoom != 10) {
						pel.Add(pe);
					}
				}
			}
			List<PathEdge> connEdges = new List<PathEdge>();
			List<int> connVertex = new List<int>();

			// 1. Find the shortest path from Room 0
			PathEdge startPath = new PathEdge(0, -1000);
			foreach (PathEdge pe in pel) {
				if (pe.d < startPath.d && pe.startRoom == 0) {
					startPath = pe;
				}
			}
			connEdges.Add(startPath);
			connVertex.Add(startPath.startRoom);
			connVertex.Add(startPath.endRoom);
			pel.Remove(startPath);

			// 2. Connect all other rooms to this path
			while(pel.Count > 0) {
				List<PathEdge> tempPEL = new List<PathEdge>();
				foreach (PathEdge pe in pel) {
					tempPEL.Add(pe);
				}
				PathEdge minPath = new PathEdge(-1000, 1000);
				foreach (PathEdge pe in tempPEL) {
					if (connVertex.Contains(pe.startRoom) && connVertex.Contains(pe.endRoom)) {
						pel.Remove(pe);
					} else if ((connVertex.Contains(pe.startRoom) || connVertex.Contains(pe.endRoom)) && pe.d < minPath.d) {
						minPath = pe;
					}
				}
				if (minPath.startRoom != -1000 || minPath.endRoom != 1000) {
					connEdges.Add(minPath);
					if (connVertex.Contains(minPath.endRoom)) {
						connVertex.Add(minPath.startRoom);
					} else {
						connVertex.Add(minPath.endRoom);
					}
					pel.Remove(minPath);
				} else {
					break;
				}
			}

			// 3. Plan out passages
			foreach (SimRoom simRoom in asr) {
				PaintGrid(simRoom.index + 100, simRoom.x - simRoom.w / 2 - 1, simRoom.y - simRoom.h / 2 - 1, simRoom.w + 2, simRoom.h + 2);
			}

			foreach (PathEdge pe in connEdges) {
				int lockOffset = Mathf.Abs(pe.startLock - pe.endLock);
				SimRoom srStart = asr.Find(item => item.index == pe.startRoom);
				SimRoom srEnd = asr.Find(item => item.index == pe.endRoom);
				if (!srStart.passages.Contains(pe.startLock)) { srStart.passages.Add(pe.startLock); }
				if (!srEnd.passages.Contains(pe.endLock)) { srEnd.passages.Add(pe.endLock); }
				if (lockOffset % 2 == 0) {
					if (pe.startLock % 2 == 0) {
						int x0 = srStart.x ;
						int x1 = srEnd.x;
						int y0 = srStart.y;
						int y1 = srEnd.y;
						int mid = (y0 + y1) / 2;
						PaintPath(2, 1, x0, Mathf.Min(y0, mid), 1, Mathf.Abs(y0 - mid));
						PaintPath(2, 1, Mathf.Min(x0, x1), mid, Mathf.Abs(x0 - x1) + 1, 1);
						PaintPath(2, 1, x1, Mathf.Min(y1, mid), 1, Mathf.Abs(y1 - mid));
					} else if (pe.startLock % 2 == 1) {
						int x0 = srStart.x;
						int x1 = srEnd.x;
						int y0 = srStart.y;
						int y1 = srEnd.y;
						int mid = (x0 + x1) / 2;
						PaintPath(2, 1, Mathf.Min(x0, mid), y0, Mathf.Abs(x0 - mid), 1);
						PaintPath(2, 1, mid, Mathf.Min(y0, y1), 1, Mathf.Abs(y0 - y1) + 1);
						PaintPath(2, 1, Mathf.Min(x1, mid), y1, Mathf.Abs(x1 - mid), 1);
					}
				} else if (lockOffset % 2 == 1) {
					if (pe.startLock % 2 == 0) {
						int x0 = srStart.x;
						int x1 = srEnd.x;
						int y0 = srStart.y;
						int y1 = srEnd.y;
						PaintPath(2, 1, x0, Mathf.Min(y0, y1), 1, Mathf.Abs(y0 - y1) + 1);
						PaintPath(2, 1, Mathf.Min(x0, x1), y1, Mathf.Abs(x0 - x1) + 1, 1);
					} else if (pe.startLock % 2 == 1) {
						int x0 = srStart.x;
						int x1 = srEnd.x;
						int y0 = srStart.y;
						int y1 = srEnd.y;
						PaintPath(2, 1, Mathf.Min(x0, x1), y0, Mathf.Abs(x0 - x1) + 1, 1);
						PaintPath(2, 1, x1, Mathf.Min(y0, y1), 1, Mathf.Abs(y0 - y1) + 1);
					}
				}
			}

			// 4. OPTIONAL - output the layout to output.csv
			/*
			string txtPath = "Assets/Resources/output.csv";
			StreamWriter writer = new StreamWriter(txtPath, false);
			for (int i = 0; i < gs[0]; i++) {
				string str = "";
				for (int j = 0; j < gs[1]; j++) {
					str += floorGrid[i, j] + ",";
				}
				writer.WriteLine(str);
			}
			writer.Close();
			*/

			// 5. Create dungeon
			foreach (SimRoom simRoom in asr) {
				GameObject rmObj = Instantiate(roomList[simRoom.index], new Vector3(simRoom.x * 0.64f, simRoom.y * 0.64f, 0), Quaternion.identity);
				InitRoom(rmObj, simRoom);
			}
			for (int i = 0; i < gs[0]; i++) {
				for (int j = 0; j < gs[1]; j++) {
					if (floorGrid[i, j] == 2) {
						Instantiate(genAssets[0],  new Vector3(i * 0.64f, j * 0.64f, 0), Quaternion.identity);
						if (FloorGridSafe(i - 1, j) == 0) { Instantiate(genAssets[1], new Vector3((i - 1) * 0.64f, j * 0.64f, 0), Quaternion.identity); }
						if (FloorGridSafe(i + 1, j) == 0) { Instantiate(genAssets[1], new Vector3((i + 1) * 0.64f, j * 0.64f, 0), Quaternion.identity); }
						if (FloorGridSafe(i, j - 1) == 0) { Instantiate(genAssets[2], new Vector3(i * 0.64f, (j - 1) * 0.64f, 0), Quaternion.identity); }
						if (FloorGridSafe(i, j + 1) == 0) { Instantiate(genAssets[1], new Vector3(i * 0.64f, (j + 1) * 0.64f, 0), Quaternion.identity); }
					}
				}
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
		Vector2 rSize = roomSizes[i];
		int[] gs = new int[2] { (int)Mathf.Round(gridSize.x), (int)Mathf.Round(gridSize.y) };
		int repeats = 0;
		while (repeats < 10000) {
			Vector2 randPos = new Vector2(Random.Range(2, gs[0]-2), Random.Range(2, gs[1]-2));
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

	public void InitRoom(GameObject roomObj, SimRoom simRoom) {
		int i = rooms.Count;
		rooms.Add(roomObj);
		RoomMain rm = roomObj.GetComponent<RoomMain>();
		rm.locID = i;
		rm.passages = simRoom.passages;
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
