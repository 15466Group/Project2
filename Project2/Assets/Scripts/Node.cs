using UnityEngine;
using System.Collections;

public class Node : Object {

	public bool open;
	public Vector3 loc;
	public bool isGoal;

	public Node (bool op, Vector3 pos, bool isG) {
		open = op;
		loc = pos;
		isGoal = isG;
	}

}
