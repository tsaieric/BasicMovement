using UnityEngine;
using System.Collections;

public enum Behavior
{
	Seek,
	Flee,
	Arrive,
	Wander
}

public class MovementBehavior : MonoBehaviour
{
	public GameObject targetObj;
	public Behavior thisBehavior;

	private float maxSpeed = 10f;
	private float maxForce = 10f;
	private Vector3 currentPos, currentVelocity, finalSteering, targetPosition;
	private RaycastHit hitInfo;

	void Start ()
	{
		currentVelocity = Vector3.zero;
	}
	
	void FixedUpdate ()
	{
		targetPosition = targetObj.transform.position;
		currentPos = this.transform.position;

		finalSteering = Vector3.zero;
		if (thisBehavior == Behavior.Seek)
			finalSteering += Seek (targetPosition);
		if (thisBehavior == Behavior.Flee)
			finalSteering += Flee (targetPosition);
		if (thisBehavior == Behavior.Arrive)
			finalSteering += Arrive (targetPosition);
        if (thisBehavior == Behavior.Wander)
            finalSteering += Wander();
		finalSteering += ObstacleAvoidance ();
		finalSteering.y = 0f; //zero out y velocities

		//v_0 + a*t
		Vector3 finalVelocity = currentVelocity + finalSteering * Time.deltaTime;
		finalVelocity = Vector3.ClampMagnitude (finalVelocity, maxSpeed);

		//p_0 + v*t
		this.transform.position = currentPos + finalVelocity * Time.deltaTime;
		this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (finalVelocity), Time.deltaTime);

		//update current velocity for the next second
		currentVelocity = finalVelocity;
		//change Zombie animation speed based on current speed
		this.GetComponent<Animator> ().SetFloat ("currentSpeed", currentVelocity.magnitude);
	}

	public Vector3 ObstacleAvoidance ()
	{
		string sphereTag = "SphereObs";
		string wallTag = "WallObs";

		Vector3 obsSteering = Vector3.zero;
		float minAheadDist = 3f;
		float detectDistance = minAheadDist + currentVelocity.magnitude / maxSpeed * minAheadDist;

		//Raycast ahead
		Vector3 velocityAhead = currentVelocity.normalized * detectDistance;
		Debug.DrawRay (currentPos, velocityAhead.normalized * detectDistance, Color.yellow);
		if (Physics.Raycast (currentPos, velocityAhead, out hitInfo, detectDistance)) {
			if (hitInfo.transform.tag == sphereTag) {
				Vector3 collisionVelocity = hitInfo.transform.position - this.transform.position;
				Vector3 steering = velocityAhead - collisionVelocity;
				Debug.DrawRay (currentPos, steering * maxForce, Color.red);
				obsSteering += steering;
			}
		}
		
		//Raycast left
		velocityAhead = Quaternion.Euler (0, 45, 0) * velocityAhead;
		Debug.DrawRay (currentPos, velocityAhead.normalized * detectDistance * 2, Color.yellow);
		if (Physics.Raycast (currentPos, velocityAhead, out hitInfo, detectDistance)) {
			if (hitInfo.transform.tag == sphereTag) {
				Vector3 collisionVelocity = hitInfo.transform.position - this.transform.position;
				Vector3 steering = velocityAhead - collisionVelocity;
				Debug.DrawRay (currentPos, steering * maxForce, Color.red);
				obsSteering += steering;
			}
		}
		if (Physics.Raycast (currentPos, velocityAhead, out hitInfo, detectDistance / 2)) {
			if (hitInfo.transform.tag == sphereTag) {
				Vector3 collisionVelocity = hitInfo.transform.position - this.transform.position;
				Vector3 steering = velocityAhead - collisionVelocity;
				Debug.DrawRay (currentPos, steering * maxForce, Color.red);
				obsSteering += steering;
			}
		}
		
		//Raycast right
		velocityAhead = Quaternion.Euler (0, -90, 0) * velocityAhead;//rotating +45-90 = -45 degrees
		Debug.DrawRay (currentPos, velocityAhead.normalized * detectDistance * 2, Color.yellow);
		if (Physics.Raycast (currentPos, velocityAhead, out hitInfo, detectDistance)) {
			if (hitInfo.transform.tag == sphereTag) {
				Vector3 collisionVelocity = hitInfo.transform.position - this.transform.position;
				Vector3 steering = velocityAhead - collisionVelocity;
				Debug.DrawRay (currentPos, steering * maxForce, Color.red);
				obsSteering += steering;
			} 
		}
		if (Physics.Raycast (currentPos, velocityAhead, out hitInfo, detectDistance / 2)) {
			if (hitInfo.transform.tag == sphereTag) {
				Vector3 collisionVelocity = hitInfo.transform.position - this.transform.position;
				Vector3 steering = velocityAhead - collisionVelocity;
				Debug.DrawRay (currentPos, steering * maxForce, Color.red);
				obsSteering += steering;
			} 
		}
		return obsSteering * maxForce;
	}

	public Vector3 Seek (Vector3 targetPos)
	{
		Vector3 desiredVelocity = (targetPos - currentPos).normalized * maxSpeed;
		Vector3 steering = desiredVelocity - currentVelocity;
		if (steering.magnitude < 1f)
			steering = steering.normalized;
		else
			steering = Vector3.ClampMagnitude (steering, maxForce);
		return steering;
	}

	public Vector3 Flee (Vector3 targetPos)
	{
		Vector3 desiredVelocity = (currentPos - targetPos).normalized * maxSpeed;
		Vector3 steering = desiredVelocity - currentVelocity;
		if (steering.magnitude < 1f)
			steering = steering.normalized;
		else
			steering = Vector3.ClampMagnitude (steering, maxForce);
		return steering;
	}

	public Vector3 Arrive (Vector3 targetPos)
	{
		float slowDistance = 20f;
		float distance = (targetPos - currentPos).magnitude;
		//distance/slowDistance will be greater than 1 if he's farther away
		//slowSpeed will never exceed maxSpeed because of clamp.
		float slowSpeed = Mathf.Clamp (maxSpeed * distance / slowDistance, 0f, maxSpeed);
		Vector3 desiredVelocity = (targetPos - currentPos).normalized * slowSpeed;
		Vector3 steering = desiredVelocity - currentVelocity;
		if (steering.magnitude < 1f)
			steering = steering.normalized;
		else
			steering = Vector3.ClampMagnitude (steering, maxForce);
		return steering;
	}

	public Vector3 Wander ()
	{
        float randomAngle = Random.Range(-90, 90);
        Vector3 desiredVelocity = Quaternion.Euler(0, randomAngle, 0) * this.transform.forward;
        desiredVelocity = desiredVelocity.normalized * maxSpeed;
        Vector3 steering = desiredVelocity - currentVelocity;
        //steering = Vector3.ClampMagnitude(steering, maxSpeed);
        return steering;
	}
}
