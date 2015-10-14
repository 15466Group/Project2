using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	private Node[,] grid;
	private int gridWidth;
	private int gridHeight;
	public float nodeSize;
	public float worldWidth;
	public float worldHeight;


	// Use this for initialization
	void Start () {

		gridWidth = Mathf.RoundToInt(worldWidth / nodeSize);
		gridHeight = Mathf.RoundToInt(worldHeight / nodeSize);

		grid = new Node[gridWidth, gridHeight];

		Vector3 worldNW = transform.position - (transform.right * worldWidth / 2.0f) + (transform.forward * worldHeight / 2.0f);

		for (int i = 0; i < gridWidth; i++) {
			for (int j = 0; j < gridHeight; j ++) {
				float xp = i * nodeSize + (nodeSize/2.0f) + worldNW.x;
				float zp = -(j * nodeSize + (nodeSize/2.0f)) + worldNW.z;
				Vector3 nodeCenter = new Vector3(xp, 0.0f, zp);
				Collider[] hits = Physics.OverlapSphere(nodeCenter, nodeSize/2.0f);
				if(hits.Length == 0) {
					grid[i,j] = new Node(true, nodeCenter);
				}
				else {
					grid[i,j] = new Node(false, nodeCenter);
				}
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {
		for (int i = 0; i < gridWidth; i++) {
			for (int j = 0; j < gridHeight; j ++) {
				if(grid[i,j].open) {
					Gizmos.color = Color.green;
					Gizmos.DrawCube (grid[i,j].loc, new Vector3(nodeSize, 1.0f, nodeSize));
				}
				else {
					Gizmos.color = Color.red;
					Gizmos.DrawCube (grid[i,j].loc, new Vector3(nodeSize, 1.0f, nodeSize));
				}
			}
		}



	}



}
