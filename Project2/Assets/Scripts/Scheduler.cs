using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scheduler : MonoBehaviour {
	
	public GameObject goal;
	public GameObject characters; //empty gameobject containing children of in game characters

	public GameObject staticObstacles;
	public GameObject dynamicObstacles;

	private Grid G;
	private Graph graph;
	private ReachGoal reachGoal;

	//each soldier has complete control for one frame 
	private int iChar;
	private int numChars;

	private State[] states;
	private bool[] useOlds;
	private List<Node>[] prevPaths;

	// Use this for initialization
	void Start () {
		iChar = 0;
		numChars = characters.transform.childCount;
		Debug.Log ("numChars: " + numChars);

		G = GetComponent<Grid> ();
		G.initStart ();
		graph = new Graph (G);
		states = new State[numChars];
		for (int i = 0; i < numChars; i++) {
			Transform child = characters.transform.GetChild(i);
			reachGoal = child.GetComponent<ReachGoal> ();
			reachGoal.Start();

			states[i] = new State(new List<Node> (), new List<Node> (), new Dictionary<Node, Node> (),
			                      null, null, reachGoal.swampCost, G, null, false, false);
		}

		//useOlds = new bool[numChars];
		prevPaths = new List<Node>[numChars];
	}
	
	// Update is called once per frame
	void Update () {
		
		//update grid
		//update graph
		//feed graph to character CharList[iChar]
		//every char is moving at each frame, but at frame i % numChars,
		//	char i is moving according to new graph and everyone else is moving according to their 'current' graphs
		Transform currChar = characters.transform.GetChild (iChar);

		reachGoal = currChar.GetComponent<ReachGoal> ();
		Vector3 start = currChar.transform.position;
		Vector3 end = goal.transform.position;
//		G.updateGrid ();
//		graph = new Graph(G);
		//graph.g.updateGrid ();
		//if the guy's in the middle of a search
		State s = states [iChar];
		if (!s.ongoing) {
			Vector3 startCoords = G.getGridCoords (currChar.position);
			Vector3 endCoords = G.getGridCoords (reachGoal.goal.transform.position);
			int startI = (int)startCoords.x;
			int startJ = (int)startCoords.z;
			int endI = (int)endCoords.x;
			int endJ = (int)endCoords.z;
			s.startNode = G.grid [startI, startJ];
			s.startNode.g = 0.0f;
			s.startNode.f = s.startNode.g + graph.weight * s.startNode.h;
			s.open.Add (s.startNode);
			s.endNode = G.grid [endI, endJ];
			s.sGrid = G;
			//s.hasFullPath = true;
		}

		states[iChar] = graph.setState (s);
//		List<Node> path = states [iChar].path;
//		Debug.Log (path);
//		prevPaths [iChar] = path;
		reachGoal.assignedPath (states [iChar].path);


		for (int i = 0; i < numChars; i++) {
			Transform child = characters.transform.GetChild(i);
			reachGoal = child.GetComponent<ReachGoal> ();
			reachGoal.assignGridCoords (graph.g.getGridCoords(reachGoal.next), 
			                            graph.g.getGridCoords(child.transform.position),
			                            graph.g.getGridCoords(goal.transform.position));

			Node r = reachGoal.nextStep ();
			if(r != null) {
//				if (states[i].dictPath.Count > 0)
//				foreach(Node key in states[i].dictPath.Keys) {
//					if(states[i].dictPath[key] == r) {
//						states[i].dictPath.Remove (key);
//						break;
//					}
//				}
				states[i].dictPath.Remove (r);
				bool a = states[i].path.Remove(r);
//				Debug.Log (a);
//				prevPaths[i].Remove (r);
			}

		}
		iChar = (iChar + 1) % numChars;
	}
}
