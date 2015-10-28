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
		for (int i = 0; i < numChars; i++) {
			Transform child = characters.transform.GetChild(i);
			reachGoal = child.GetComponent<ReachGoal> ();
			reachGoal.Start();
		}

		states = new State[numChars];
		useOlds = new bool[numChars];
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
		iChar = (iChar + 1) % numChars;

		reachGoal = currChar.GetComponent<ReachGoal> ();
		Vector3 start = currChar.transform.position;
		Vector3 end = goal.transform.position;
//		G.updateGrid ();
//		graph = new Graph(G);
		//graph.g.updateGrid ();
		List<Node> path;
		if (useOlds [iChar]) {
			State s = states [iChar];
			path = graph.setState (s, false, start);
			if(graph.seenEnd) {
				prevPaths[iChar] = path;
			}
			else {
				if(prevPaths[iChar] != null)
					path = prevPaths[iChar];
			}
//			reachGoal.assignedPath (path, s.dictPath);
			reachGoal.assignedPath (path);
		}
		else {
			graph.g.updateGrid ();
			Dictionary<Node, Node> dictPath = new Dictionary<Node, Node> ();
			List<Node> open = new List<Node> ();
			List<Node> closed = new List<Node> ();
			State s = new State(open, closed, dictPath, end, reachGoal.swampCost, graph.g);
			path = graph.setState (s, true, start);
			if(graph.seenEnd) {
				prevPaths[iChar] = path;
			}
			else {
				if(prevPaths[iChar] != null)
					path = prevPaths[iChar];
			}
//			reachGoal.assignedPath (path, s.dictPath);
			reachGoal.assignedPath (path);
		}
		useOlds [iChar] = graph.useOld;
//		if (graph.useOld) {
		states[iChar] = graph.oldState;
//		}
//		reachGoal.nextStep ();

		//the character has moved a bit and gotten rid of some of the beginning of the path so update it
//		if (prevPaths [iChar] != null) {
//			prevPaths[iChar] = reachGoal.tempPositions;
//		}

		for (int i = 0; i < numChars; i++) {
			Transform child = characters.transform.GetChild(i);
			reachGoal = child.GetComponent<ReachGoal> ();
			reachGoal.assignGridCoords (graph.g.getGridCoords(reachGoal.next), 
			                            graph.g.getGridCoords(child.transform.position),
			                            graph.g.getGridCoords(goal.transform.position));
			
			reachGoal.nextStep ();
			
			//the character has moved a bit and gotten rid of some of the beginning of the path so update it
			List<Node> iPrevPath = prevPaths[i];
			if (iPrevPath != null){
				prevPaths[i] = reachGoal.tempPositions;
			}
//			if (states[iChar] != null)
//				states[iChar].dictPath = reachGoal.dictPath;
		}
	}
}
