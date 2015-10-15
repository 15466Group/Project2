using UnityEngine;
using System.Collections;

public class ObstacleMovement : MonoBehaviour {

	public string direction;
	private float movementTime;
	private float howLong;

	// Use this for initialization
	void Start () {
		movementTime = 0.0f;
		howLong = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		movementTime += Time.deltaTime;
		if (movementTime <= howLong) {
			if (string.Compare (direction, "X") == 0) {
				transform.position += transform.right;
			}
			else{
				transform.position += transform.forward;
			}
		} else {
			if (movementTime <= 2*howLong){
				if (string.Compare (direction, "X") == 0) {
					transform.position -= transform.right;
				}
				else{
					transform.position -= transform.forward;
				}
			}
			else {
				movementTime = 0.0f;
			}
		}
	}
}
