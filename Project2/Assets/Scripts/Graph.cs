using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph : Object {

	public Node[,] nodes;
	public Grid g;
	private int numRows;
	private int numCols;
	private float weight;
	private int totalNodesToSearch;
	private int numNodesSeen;
	public bool useOld;
	public State oldState;
	public bool seenEnd;

	public Graph(Grid G){
		g = G;
		nodes = G.grid;
		weight = 2.0f;
		numRows = nodes.GetLength (0);
		numCols = nodes.GetLength (1);
		useOld = false;
		seenEnd = false;
	}

	public List<Node> setState(State S, bool addStart, Vector3 start){

		useOld = false;
		seenEnd = false;
		Vector3 startCoords = g.getGridCoords (start);
		Vector3 endCoords = g.getGridCoords (S.end);
		int startI = (int)startCoords.x;
		int startJ = (int)startCoords.z;
		int endI = (int)endCoords.x;
		int endJ = (int)endCoords.z;
		Node startNode = nodes [startI, startJ];
		Node endNode = nodes [endI, endJ];

		List<Node> path;

		//not sure about this
		if (addStart) {
			startNode.g = 0;
			startNode.f = startNode.g + weight * startNode.h;
			S.open.Add (startNode);
		} else {
			g = S.sGrid;
//			startNode.g = 0;
//			startNode.f = startNode.g + weight * startNode.h;
//			S.open.Add (startNode);
		}

//		if (useOld){
		return getPath (S.open, S.closed, S.dictPath, startNode, endNode, S.swampCost);
//		} else {
//			List<Node> open = new List<Node> ();
//			List<Node> closed = new List<Node> ();
//			Dictionary<Node, Node> dictPath = new Dictionary<Node, Node> ();
//			startNode.g = 0;
//			startNode.f = startNode.g + weight * startNode.h;
//			open.Add (startNode);
//			path = getPath (open, closed, dictPath, startNode, endNode, swampCost);
//		}

		//set old stuff
		//check if next frame needs to use old
//		return path;
	}

	public List<Node> getPath(List<Node> open, List<Node> closed, Dictionary<Node, Node> dictPath, 
	                          Node startNode, Node endNode, float swampCost) {

		totalNodesToSearch = 100;
		numNodesSeen = 0;

//		Vector3 startCoords = g.getGridCoords (start);
//		Vector3 endCoords = g.getGridCoords (end);
//		int startI = (int)startCoords.x;
//		int startJ = (int)startCoords.z;
//		int endI = (int)endCoords.x;
//		int endJ = (int)endCoords.z;
//
//		Node startNode = nodes [startI, startJ];
//		Node endNode = nodes [endI, endJ];
//		startNode.g = 0;
//		startNode.f = startNode.g + weight * startNode.h;
//
//		List<Node> open = new List<Node> ();
//		List<Node> closed = new List<Node> ();
//		open.Add (startNode);
//

		//infinite heuristic
		Node estimEndNode = new Node(false, Vector3.zero, false, 0, 0, Mathf.Infinity, false);

		List<Node> failPath = new List<Node> ();
		failPath.Add (endNode);

		while (open.Count > 0) {
			Node current = findSmallestVal(open);
			numNodesSeen += 1;
			if (Vector3.Distance(endNode.loc, current.loc) <= 0.5f){
//				Debug.Log ("numSeen: " + numNodesSeen);
				seenEnd = true;
				return makePath(dictPath, endNode);
			}
			if (current.h < estimEndNode.h){
//				Debug.Log("new estim");
				estimEndNode = current;
			}
			if (numNodesSeen >= totalNodesToSearch){
//				Debug.Log ("return makePath(dictPath, estimEndNode);");
				useOld = true;
				oldState = new State(open, closed, dictPath, endNode.loc, swampCost, g);
				return makePath(dictPath, estimEndNode);
			}
			open.Remove (current);
			closed.Add (current);
			foreach (Node successor in getNeighbors(current)){
				Debug.DrawLine (successor.loc, current.loc, Color.blue);
				if (closed.Contains (successor)){
					continue; //in the closed set
				}
				float newCost = current.g + costOfStep(current, successor, swampCost);
				if (!open.Contains(successor)){
					open.Add (successor);
				}
				else if (successor.g <= newCost){
					continue;
				}

				successor.g = newCost;
				successor.f = successor.g + weight * successor.h;
				if(dictPath.ContainsKey (successor)) {
					dictPath[successor] = current;
				}
				else {
					dictPath.Add(successor, current); //successor came from smallestVal, to reconstruct path backwards
				}
			}
		}
		return failPath;
	}

	List<Node> makePath(Dictionary<Node, Node> dictPath, Node endNode){
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;
		path.Add (currentNode);
		Node prevNode = endNode;
		while (dictPath.ContainsKey(currentNode)) {
			currentNode = dictPath[currentNode];
			prevNode = currentNode;
			path.Add(currentNode);
		}
		path.Reverse ();
		if (path.Count > 0)
			path.RemoveAt (0);
		return path;
	}

	Node findSmallestVal(List<Node> open){
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
	float costOfStep(Node currNode, Node nextNode, float swampCost){
		float cost = Vector3.Distance (currNode.loc, nextNode.loc);
//		if ((currNode.isSwamp && !nextNode.isSwamp) ||
//		    (!currNode.isSwamp && nextNode.isSwamp)){
		if (currNode.isSwamp || nextNode.isSwamp){
			cost *= swampCost;
		}
		return cost;
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
