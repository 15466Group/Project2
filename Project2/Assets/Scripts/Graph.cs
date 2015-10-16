using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph : Object {

	public Node[,] nodes;
	public Grid g;
	private int numRows;
	private int numCols;

	public Graph(Grid G){
		g = G;
		nodes = G.grid;
		numRows = nodes.GetLength (0);
		numCols = nodes.GetLength (1);
	}

	// Use this for initialization
	void Start () {
		numRows = nodes.GetLength (0);
		numCols = nodes.GetLength (1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public List<Node> getPath(Vector3 start, Vector3 end) {

		Vector3 startCoords = g.getGridCoords (start);
		Vector3 endCoords = g.getGridCoords (end);
		int startI = (int)startCoords.x;
		int startJ = (int)startCoords.z;
		int endI = (int)endCoords.x;
		int endJ = (int)endCoords.z;

		return getNeighbors (nodes [startI, startJ]);


	}

	List<Node> getNeighbors(Node n) {
		List <Node> neighbors = new List<Node> ();
		for (int newi = n.i - 1; newi <= n.i + 1; newi++) {
			for (int newj = n.j - 1; newj <= n.j + 1; newj++) {
				if(validNeighborIndexes (n.i, n.j, newi, newj)) {
					neighbors.Add(nodes[newi,newj]);
				}
			}
		}
		return neighbors;
	}


	bool validNeighborIndexes(int i, int j, int newi, int newj) {
		return (newi >= 0 && newj >= 0 && 
				newi < numRows && newj < numCols &&
				(i != newi || j != newj) &&
				nodes [newi, newj].free);
	}


	float estimateHeuristic (Node n, Vector3 end) {
		return Vector3.Distance (n.loc, end);
	}
}
