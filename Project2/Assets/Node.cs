using UnityEngine;
using System.Collections;

public class Node : Object {

	public bool open;
	public Vector3 loc;

	public Node (bool op, Vector3 pos) {
		open = op;
		loc = pos;
	}

}
