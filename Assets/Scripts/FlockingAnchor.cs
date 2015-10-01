using UnityEngine;
using System.Collections;

public enum Formation
{
	Circle,
	Vshape,
	Triangle,
	Square
}

public class FlockingAnchor : MonoBehaviour
{
	public GameObject flock;
	public Formation form;
	private MoveController[] flockObjects;
	private int numObjects;
	//private Vector3 avgCurrentVelocity;
	private Vector3 avgCenterPos;
	private float radius = 15f;
	private MoveController movement;
	// Use this for initialization
	void Start ()
	{
		flockObjects = flock.GetComponentsInChildren<MoveController> ();
		numObjects = flockObjects.Length;
		this.movement = this.GetComponent<MoveController> ();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		this.movement.Wander (15f, 120f);
		this.movement.ObstacleAvoidance (radius / 2, 30f);
		this.movement.UpdateEverything ();
		Vector3 totalCurrentVelocity = Vector3.zero;
		Vector3 totalCenterPos = Vector3.zero;
		foreach (MoveController obj in flockObjects) {
			totalCurrentVelocity += obj.GetCurrentVelocity ();
			totalCenterPos += obj.transform.position;
		}
		//avgCurrentVelocity = totalCurrentVelocity / numObjects;
		avgCenterPos = totalCenterPos / numObjects;
		Vector3 direction = this.movement.GetCurrentVelocity ().normalized;//.forward.normalized;
		Vector3 currentPos;
		float interval = 3;                
		avgCenterPos = this.transform.position;

		switch (this.form) {
		case Formation.Circle:
			Vector3 angle = Vector3.forward.normalized;
			Vector3 circlePerimeterPos;
			int count = 0;
			float rotateBy = 360 / numObjects;
			avgCenterPos = this.transform.position;
			foreach (MoveController obj in flockObjects) {
				angle = Quaternion.Euler (0f, rotateBy, 0f) * angle;
				Debug.DrawRay (avgCenterPos, angle * radius, Color.black);
				circlePerimeterPos = avgCenterPos + angle * radius;
				obj.Arrive (circlePerimeterPos, 10f); //arrive to their position
				obj.Separation (5f, 10f);
				obj.ObstacleAvoidance (2f, 20f);
				obj.UpdateEverything ();
				count++;
			}
			break;
		case Formation.Vshape:
			Vector3 left = Quaternion.Euler (0f, -135f, 0f) * direction;
			Debug.DrawRay (avgCenterPos, left * radius, Color.black);
			Vector3 back = Quaternion.Euler (0f, -180f, 0f) * direction;
			Debug.DrawRay (avgCenterPos, back * radius, Color.black);
			Vector3 right = Quaternion.Euler (0f, 135f, 0f) * direction;
			Debug.DrawRay (avgCenterPos, right * radius, Color.black);

			for (int i=0; i< numObjects; i++) {
				MoveController obj = flockObjects [i];
				avgCenterPos = this.transform.position;
				if (i % 2 == 0)
					currentPos = avgCenterPos + left * interval * (i + 1);
				else						
					currentPos = avgCenterPos + right * interval * i;					
				obj.Arrive (currentPos, 10f); //arrive to their position
				obj.Separation (2f, 10f);
				obj.ObstacleAvoidance (2f, 20f);
				obj.UpdateEverything ();
				//avgCenterPos = currentPos;					
			}								
			break;
		case Formation.Square: 
			MoveController leader = flockObjects [0];
			leader.FollowLeader (this.movement, 1f);
			leader.Separation (2f, 10f);
			leader.ObstacleAvoidance (2f, 20f);
			leader.UpdateEverything ();
			float dis = 5f;
			Vector3 squadPerimeterPos = Vector3.zero;
			int index = 1;
			for (int j = 0; j < 3; j++) {
				for (int k = 0; k < numObjects / 3; k++) {
					MoveController obj = flockObjects [index];
					Vector3 newPos;
					Vector3 newAngle;
					newAngle = Quaternion.Euler (0f, 90f, 0f) * direction;
					newPos = flockObjects [0].transform.position - direction * dis * k + newAngle * j * dis;
					squadPerimeterPos = flockObjects [index].transform.position;
					Debug.DrawRay (squadPerimeterPos, newAngle * dis, Color.black);
					Debug.DrawRay (squadPerimeterPos, -direction * dis, Color.black);
					obj.Arrive (newPos, 10f); //arrive to their position
					obj.Separation (2f, 10f);
					obj.ObstacleAvoidance (2f, 20f);
					obj.UpdateEverything ();
					index++;
				}
			}
			break;
		case Formation.Triangle:
			float triangleAngle = 150f;
			Vector3 leftSideAngle = Quaternion.Euler (0f, -triangleAngle, 0f) * direction;
			Debug.DrawRay (avgCenterPos, leftSideAngle * radius, Color.black);
			Vector3 rightAngle = Quaternion.Euler (0f, 90f, 0f) * direction;
			Debug.DrawRay (avgCenterPos, rightAngle * radius, Color.black);
			int numRows = 4;
			int distance = 4;
			Vector3 leaderPos = this.transform.position;
			Vector3 objPos = Vector3.zero;
			Vector3 prevPos = Vector3.zero;
			int flockIndex = 0;
			for (int row = 0; row < numRows; row++) {
				for (int i =0; i <= row; i++) {
					MoveController flockObj = flockObjects [flockIndex];
					if (i == 0) {
						objPos = leaderPos + leftSideAngle.normalized * distance * row;
						prevPos = objPos;
					} else {
						objPos = prevPos + rightAngle.normalized * distance * i;
					}
					flockObj.Arrive (objPos, 10f);
					flockObj.Separation (2f, 10f);
					flockObj.ObstacleAvoidance (2f, 20f);
					flockObj.UpdateEverything ();
					flockIndex++;
				} 
			}						
			break;
		
		} 
       
	}
}
