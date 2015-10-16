using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph : MonoBehaviour {

	private Node[,] nodes;
	public Grid g;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Node[] getPath(Vector3 start, Vector3 end) {

		Vector3 startCoords = g.getGridCoords (start);
		Vector3 endCoords = g.getGridCoords (end);



	}

	Node[] getNeighbors(Node n) {
		List <Node> neighbors = new List<Node> ();
		int xCoord = n.xCoord;
		int zCoord = n.zCoord;

		for (int i = -1; i <= 1; i++) {
			for (int j = -1; j <= 1; j++) {
				int newX = xCoord + i;
				int newZ = zCoord + j;
				if(validNeighborCoords (xCoord, zCoord, newX, newZ)) {
					neighbors.Add(nodes[newX,newZ]);
				}
			}
		}
		return neighbors.ToArray ();
	}


	bool validNeighborCoords(int xCoord, int zCoord, int newX, int newZ) {
		return (newX >= 0 && newZ >= 0 && newX < nodes.Length && newX < nodes [1].length && newX != xCoord && newZ != zCoord && nodes [newX, newZ].free);
	}


	float estimateHeuristic (Node n, Vector3 end) {
		return Vector3.Distance (n.loc, end);
	}
}
