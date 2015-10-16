using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReachGoal: NPCBehaviour {

	public GameObject goal;
	public GameObject plane;
	private Grid G;
	private Graph graph;
	private Vector3 endTarget;
	private List<Node> tempPositions;


	// Use this for initialization
	public override void Start () {
		base.Start ();
		endTarget = goal.transform.position;
		acceleration = base.calculateAcceleration (target);
		isWanderer = false;
		isReachingGoal = true;
		G = plane.GetComponent<Grid> ();
		G.Start ();
		graph = new Graph (G);
		tempPositions = new List<Node> ();
	}

	public override void Update () {
		endTarget = goal.transform.position;
		target = nextTarget();
		base.Update ();
	}

	Vector3 nextTarget (){
		Vector3 next = endTarget;
		tempPositions =  graph.getPath (transform.position, endTarget);
		if (tempPositions.Count > 0) {
			next = tempPositions [0].loc;
			Debug.Log("asdadsasd");
			tempPositions.RemoveAt (0);
		}
//		Debug.Log("t + " + transform.position + "n + " + next);
		return next;
	}
}