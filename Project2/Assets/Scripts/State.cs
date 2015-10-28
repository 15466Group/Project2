using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class State {

	public List<Node> open { get; set; }
	public List<Node> closed { get; set; }
	public Dictionary<Node, Node> dictPath { get; set; }
	public Vector3 end { get; set; }
	public float swampCost { get; set; }
	public Grid sGrid { get; set; }

	public State (List<Node> o, List<Node> c, Dictionary<Node, Node> d, Vector3 e, float sw, Grid sg){
		open = o;
		closed = c;
		dictPath = d;
		end = e;
		swampCost = sw;
		sGrid = sg;
	}
}
