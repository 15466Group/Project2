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
	private float timer;
	private float searchTime;
	private bool hitNextNode;
	private Vector3 next;


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
		searchTime = 50.0f;
		timer = 0.0f;
		hitNextNode = false;
		next = transform.position;
	}

	public override void Update () {
		for(int i = 0; i < tempPositions.Count - 1; i++) {
			Debug.DrawLine (tempPositions[i].loc, tempPositions[i+1].loc, Color.red);
		}
		endTarget = goal.transform.position;
		target = nextTarget();
		base.Update ();
	}

	Vector3 nextTarget (){
		Vector3 nextCoords = G.getGridCoords (next);
		Vector3 transCoords = G.getGridCoords (transform.position);
		if (nextCoords.x == transCoords.x && nextCoords.z == transCoords.z) {
			hitNextNode = true;
		}
		timer += Time.deltaTime;
		if (timer >= searchTime || tempPositions.Count == 0) {
			G.updateGrid ();
			graph = new Graph(G);
			tempPositions =  graph.getPath (transform.position, endTarget);
			timer = 0.0f;
		}
		if (hitNextNode){
			next = tempPositions [0].loc;
			Debug.Log (tempPositions.Count + " > 0");
			tempPositions.RemoveAt (0);
			hitNextNode = false;
		}
//		Debug.Log("t + " + transform.position + "n + " + next);
		return next;
	}
}