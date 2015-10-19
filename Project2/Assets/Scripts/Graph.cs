using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph : Object {

	public Node[,] nodes;
	public Grid g;
	private int numRows;
	private int numCols;
	private float weight = 1.0f;

	public Graph(Grid G){
		g = G;
		nodes = G.grid;
		numRows = nodes.GetLength (0);
		numCols = nodes.GetLength (1);
	}

//	// Use this for initialization
//	void Start () {
//		numRows = nodes.GetLength (0);
//		numCols = nodes.GetLength (1);
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	public List<Node> getPath(Vector3 start, Vector3 end) {

		Vector3 startCoords = g.getGridCoords (start);
		Vector3 endCoords = g.getGridCoords (end);
		int startI = (int)startCoords.x;
		int startJ = (int)startCoords.z;
		int endI = (int)endCoords.x;
		int endJ = (int)endCoords.z;

		Node startNode = nodes [startI, startJ];
		Node endNode = nodes [endI, endJ];
		startNode.g = 0;
		startNode.f = startNode.g + startNode.h;

//		List<Node> path = getNeighbors (startNode);

//		dictPath.Add (path [1], path [0]);
//		dictPath.Add (path [3], path [2]);
//
//		return getNeighbors (startNode);

		List<Node> open = new List<Node> ();
		List<Node> closed = new List<Node> ();
		open.Add (startNode);

		Dictionary<Node, Node> dictPath = new Dictionary<Node, Node> ();
		int c = 0;
		while (open.Count > 0) {
			c++;
			Debug.Log ("c = " + c);
			Node current = findSmallestVal(open, end);
			if (Vector3.Distance(endNode.loc, current.loc) <= 0.5f){
				return makePath(dictPath, endNode);
			}
			open.Remove (current);
			closed.Add (current);
			int d = 0;
			foreach (Node successor in getNeighbors(current)){
				Debug.DrawLine (successor.loc, current.loc, Color.blue);
				d++;
				Debug.Log("d = " + d);
				if (closed.Contains (successor)){
					continue; //in the closed set
				}
				float newCost = current.g + costOfStep(current, successor);
				if (!open.Contains(successor)){
					open.Add (successor);
					Debug.Log("added to open list");
				}
				else if (successor.g <= newCost){
					continue;
				}

				successor.g = newCost;
				successor.f = successor.g + successor.h;
				if(dictPath.ContainsKey (successor)) {
					dictPath[successor] = current;
				}
				else {
					dictPath.Add(successor, current); //successor came from smallestVal, to reconstruct path backwards
				}
			}
		}
		//Debug.Log ("endNode.closed = " + endNode.closed + " open.Count = " + open.Count);
		return makePath(dictPath, endNode);
	}

	List<Node> makePath(Dictionary<Node, Node> dictPath, Node endNode){
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;
		path.Add (currentNode);
		Node prevNode = endNode;
		while (dictPath.ContainsKey(currentNode)) {
			Debug.Log("dictpath construction");
			currentNode = dictPath[currentNode];
			//Debug.DrawLine(prevNode.loc, currentNode.loc, Color.red);
			prevNode = currentNode;
			path.Add(currentNode);
		}
		path.Reverse ();
//		Debug.Log ("path length: " + path.Count + "dictPathLength: " + dictPath.Count);
		return path;
	}

	Node findSmallestVal(List<Node> open, Vector3 end){
		Node smallestVal = open[0];
		//float min = smallestVal.g + weight * smallestVal.h;
		float min = smallestVal.f;
		foreach (Node n in open) {
			float potentialMin = n.f;
			if (potentialMin < min){
				min = potentialMin;
				smallestVal = n;
			}
		}
		return smallestVal;
	}

	//simple movements only
	float costOfStep(Node currNode, Node nextNode){
		return Vector3.Distance (currNode.loc, nextNode.loc);
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
