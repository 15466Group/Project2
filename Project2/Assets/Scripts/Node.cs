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
	public bool open;
	//public bool closed;

	public Node (bool op, Vector3 pos, bool isG, int newi, int newj) {
		free = op;
		loc = pos;
		isGoal = isG;
		i = newi;
		j = newj;
		g = Mathf.Infinity;
		h = Mathf.Infinity;
		open = true;
		//closed = false;
	}

}
