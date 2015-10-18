using UnityEngine;
using System.Collections;

public class Node : Object {

	public bool free;
	public Vector3 loc;
	public bool isGoal;
	public int i;
	public int j;
	public float g;
	public float h;
//	public bool open;
	public bool closed;

	public Node (bool isFree, Vector3 pos, bool isG, int newi, int newj, float heuristic) {
		free = isFree;
		loc = pos;
		isGoal = isG;
		i = newi;
		j = newj;
		g = Mathf.Infinity;
		h = heuristic;
//		open = true;
		closed = false;
	}

}
