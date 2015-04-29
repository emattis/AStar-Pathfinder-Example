using UnityEngine;
using System.Collections;
using Pathfinding;

public class AIScript : MonoBehaviour {
	
	public Transform target;
	public Vector3 targetPosition;
	
	private Seeker seeker;
	private CharacterController controller;
	
	//Path being calculated
	public Path path;
	
	//How fast the object will move
	public float speed = 100;
	
	//Max distance from the unit to a node for it to continue to the next node
	public float nextWaypointDistance = 3;
	
	//The node we are moving too
	private int currentWaypoint = 0;
	
	
	public void Start () {
		targetPosition = target.transform.position;
		//Get a reference to the Seeker component 
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		
		
		//Start a new path to the targetPosition and then returns the result to the OnPathComplete function
		seeker.StartPath (transform.position,targetPosition, OnPathComplete);
	}
	
	public void OnPathComplete (Path p) {

		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}
	
	public void FixedUpdate () {
		if (path == null) {
			return;
		}
		
		if (currentWaypoint >= path.vectorPath.Count) {
			Debug.Log ("End Of Path Reached");
			return;
		}
		
		//Direction to the next node
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;
		controller.Move (dir);
		
		//Check if we are close enough to the next node and ff we are, proceed to follow the next node
		if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}
	
}