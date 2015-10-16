using UnityEngine;
using System.Collections;

public class Node : Object {

	public bool free;
	public Vector3 loc;
	public bool isGoal;
	public int xCoord;
	public int zCoord;
	public float g;
	public float h;
	public bool open;
	//public bool closed;

	public Node (bool op, Vector3 pos, bool isG, int x, int z) {
		free = op;
		loc = pos;
		isGoal = isG;
		xCoord = x;
		zCoord = z;
		g = Mathf.Infinity;
		h = Mathf.Infinity;
		open = true;
		//closed = false;
	}

}
