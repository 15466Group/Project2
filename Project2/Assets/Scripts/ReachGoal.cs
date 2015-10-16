using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReachGoal: NPCBehaviour {

	public GameObject goal;
	public GameObject plane;
	private Grid grid;
	private Vector3 endTarget;
	private List<Vector3> tempPositions;


	// Use this for initialization
	public override void Start () {
		base.Start ();
		endTarget = goal.transform.position;
		acceleration = base.calculateAcceleration (target);
		isWanderer = false;
		isReachingGoal = true;
		grid = plane.GetComponent<Grid> ();
		tempPositions = new List<Vector3> ();
		Debug.Log (grid.nodeSize);
	}

	public override void Update () {
		endTarget = goal.transform.position;
		target = nextTarget();
		base.Update ();
	}

	Vector3 nextTarget (){
		Vector3 next = endTarget;
		if (tempPositions.Count > 0) {
			next = tempPositions [0];
			tempPositions.RemoveAt (0);
		}
		return next;
	}
}