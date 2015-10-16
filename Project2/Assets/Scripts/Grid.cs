using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public float nodeSize;
	public GameObject goal;

	private int obstacleLayer;
	private int goalLayer;

	private Node[,] grid;
	private int gridWidth;
	private int gridHeight;
	private float worldWidth;
	private float worldHeight;
	private Vector3 worldNW; //world north west, top left corner of map/plane


	// Use this for initialization
	void Start () {
		worldWidth = transform.lossyScale.x * 10.0f; //plane
		worldHeight = transform.lossyScale.z * 10.0f; //plane

		gridWidth = Mathf.RoundToInt(worldWidth / nodeSize);
		gridHeight = Mathf.RoundToInt(worldHeight / nodeSize);

		grid = new Node[gridWidth, gridHeight];

		worldNW = transform.position - (transform.right * worldWidth / 2.0f) + (transform.forward * worldHeight / 2.0f);

		obstacleLayer = 1 << LayerMask.NameToLayer ("Obstacles");
		goalLayer = 1 << LayerMask.NameToLayer ("Goal");

		updateGrid ();

	}
	
	// Update is called once per frame
	void Update () {
//		updateGrid ();
	}

	void updateGrid(){
		for (int i = 0; i < gridWidth; i++) {
			for (int j = 0; j < gridHeight; j ++) {
				float xp = i * nodeSize + (nodeSize/2.0f) + worldNW.x;
				float zp = -(j * nodeSize + (nodeSize/2.0f)) + worldNW.z;
				Vector3 nodeCenter = new Vector3(xp, 0.0f, zp);
				Collider[] hits = Physics.OverlapSphere(nodeCenter, nodeSize/2.0f, obstacleLayer | goalLayer);
				bool isGoal = checkIfContainsGoal(hits);
				if(hits.Length == 0) {
					grid[i,j] = new Node(true, nodeCenter, isGoal);
				}
				else {
					grid[i,j] = new Node(false, nodeCenter, isGoal);
				}
			}
		}
	}

	void OnDrawGizmos() {
		for (int i = 0; i < gridWidth; i++) {
			for (int j = 0; j < gridHeight; j ++) {
				if (!grid [i, j].open) {
					Gizmos.color = Color.red;
					if (grid[i,j].isGoal)
						Gizmos.color = Color.green;
					Gizmos.DrawCube (grid [i, j].loc, new Vector3 (nodeSize, 1.0f, nodeSize));
				}
			}
		}
	}

	bool checkIfContainsGoal(Collider[] hits){
		bool isGoal = false;
		float epsilon = 20;
		foreach (Collider hit in hits) {
			float distance = Vector3.Distance(goal.transform.position, hit.transform.position);
			if (distance <= epsilon){
				isGoal = true;
				break;
			}
		}
		return isGoal;
	}


}
