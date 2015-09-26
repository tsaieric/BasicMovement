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
	public float maxSpeed = 10f;

	private float maxForce = 20f;
	private float randomAngle = 0f;
	private Vector3 currentPos, currentVelocity, finalSteering, targetPosition;
	private string sphereTag = "SphereObs";
	private string wallTag = "WallObs";
	private RaycastHit hitInfo;

	void Start ()
	{	Debug.Log ("test");
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
			finalSteering += Flee (targetPosition, 50f);
		if (thisBehavior == Behavior.Arrive)
			finalSteering += Arrive (targetPosition, 20f);
		if (thisBehavior == Behavior.Wander)
			finalSteering += Wander (15f, 120f) * 2f;
		finalSteering += ObstacleAvoidance () * 20f;
		finalSteering.y = 0f; //zero out y velocities
		finalSteering = Vector3.ClampMagnitude (finalSteering, maxForce);

		//v_0 + a*t
		Vector3 finalVelocity = currentVelocity + finalSteering * Time.deltaTime;
		finalVelocity = Vector3.ClampMagnitude (finalVelocity, maxSpeed);

		//p_0 + v*t
		this.transform.position = currentPos + finalVelocity * Time.deltaTime;
		this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (finalVelocity), Time.deltaTime * 2);

		//update current velocity for the next second
		currentVelocity = finalVelocity;

		//change Zombie animation speed based on current speed
		this.GetComponent<Animator> ().SetFloat ("currentSpeed", currentVelocity.magnitude);
	}

	public Vector3 ObstacleAvoidance ()
	{
		Vector3 obsSteering = Vector3.zero;
		float minAheadDist = 3f;
		//change the detecting distance based on currentVelocity
		float detectDistance = minAheadDist + currentVelocity.magnitude / maxSpeed * minAheadDist;

		//Raycast ahead
		Vector3 raycastVector = currentVelocity.normalized * detectDistance;
		obsSteering += ObstacleHelperMethod (raycastVector, detectDistance);

		//Raycast right
		raycastVector = Quaternion.Euler (0, 45, 0) * raycastVector; //rotating +45 degrees
		obsSteering += ObstacleHelperMethod (raycastVector, detectDistance);

		//Raycast left
		raycastVector = Quaternion.Euler (0, -90, 0) * raycastVector; //rotating +45-90 = -45 degrees
		obsSteering += ObstacleHelperMethod (raycastVector, detectDistance);

		return obsSteering;
	}

	private Vector3 ObstacleHelperMethod (Vector3 raycastVector, float detectDistance)
	{
		Vector3 combinedSteering = Vector3.zero;
		Debug.DrawRay (currentPos, raycastVector.normalized * detectDistance, Color.yellow);
		if (Physics.Raycast (currentPos, raycastVector, out hitInfo, detectDistance)) {
			if (hitInfo.transform.tag == sphereTag) {
				Vector3 collisionVelocity = hitInfo.transform.position - this.transform.position;
				Vector3 steering = raycastVector - collisionVelocity;
				Debug.DrawRay (currentPos, steering, Color.red);
				combinedSteering += steering;
			} 
			
			if (hitInfo.transform.tag == wallTag) {
				float lengthPastWall = ((currentPos + raycastVector.normalized * detectDistance) - hitInfo.point).magnitude;
				Vector3 steering = hitInfo.normal.normalized * lengthPastWall;
				Debug.DrawRay (currentPos, steering, Color.black);
				combinedSteering += steering;
			} 
		}
		return combinedSteering;
	}

	public Vector3 Seek (Vector3 targetPos)
	{
		Vector3 desiredVelocity = (targetPos - currentPos).normalized * maxSpeed;
		Vector3 steering = desiredVelocity - currentVelocity;
		if (steering.magnitude < 1f)
			steering = steering.normalized;
		//else
		//	steering = Vector3.ClampMagnitude (steering, maxForce);
		return steering;
	}

	public Vector3 Flee (Vector3 targetPos, float fleeRange)
	{
        //if distance from target > fleeRange, Wander
        float distance = (currentPos - targetPos).magnitude;
        if (distance <= fleeRange)
        {
            //same as Seek except desiredVelocity is opposite (currentPos - targetPos)
            Vector3 desiredVelocity = (currentPos - targetPos).normalized * maxSpeed;
            Vector3 steering = desiredVelocity - currentVelocity;
            if (steering.magnitude < 1f)
                steering = steering.normalized;
            //else
            //    steering = Vector3.ClampMagnitude(steering, maxForce);
            return steering;
        }
        else
        {
            return Wander(15f, 120f);
        }
	}

	public Vector3 Arrive (Vector3 targetPos, float slowDistance)
	{
		float distance = (targetPos - currentPos).magnitude;
		//distance/slowDistance will be greater than 1 if he's farther away
		//slowSpeed will never exceed maxSpeed because of clamp.
		float slowSpeed = Mathf.Clamp (maxSpeed * distance / slowDistance, 0f, maxSpeed);
		Vector3 desiredVelocity = (targetPos - currentPos).normalized * slowSpeed;
		Vector3 steering = desiredVelocity - currentVelocity;
		if (steering.magnitude < 1f)
			steering = steering.normalized;
		//else
		//	steering = Vector3.ClampMagnitude (steering, maxForce);
		return steering;
	}

	public Vector3 Wander (float angleRange, float maxAngle)
	{
		randomAngle = randomAngle + Random.Range (-angleRange, angleRange);
		//reset randomAngle if it goes past maxAngle degrees
		if (Mathf.Abs (randomAngle) > maxAngle)
			randomAngle = 0f;
		Vector3 steering = Quaternion.Euler (0, randomAngle, 0) * currentVelocity;
		if (currentVelocity == Vector3.zero)
			steering = Quaternion.Euler (0, randomAngle, 0) * transform.forward;
		steering = steering.normalized * maxSpeed / 2;
		Debug.DrawRay (currentPos, steering, Color.blue);
		return steering;
	}
}
