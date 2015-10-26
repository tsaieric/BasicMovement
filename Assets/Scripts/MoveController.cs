using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoveController : MonoBehaviour
{
	public float maxSpeed = 10f;
	public float maxForce = 20f;
	private List<Node> path;
	private int destNodeIndex;
	private float randomAngle;
	private Vector3 currentPos, currentVelocity, finalSteering;
	private string sphereTag = "SphereObs";
	private string wallTag = "WallObs";
	private RaycastHit hitInfo;
	private MoveController[] group;
	void Start ()
	{
		currentPos = this.transform.position;
		randomAngle = 0f;
		currentVelocity = Vector3.zero;
		if (transform.parent != null)
			group = transform.parent.GetComponentsInChildren<MoveController> ();
	}

	void FixedUpdate ()
	{
		currentPos = this.transform.position;
	}

	public void ResetSteering ()
	{
		finalSteering = Vector3.zero;
	}

	public void UpdateEverything ()
	{
		finalSteering.y = 0f; //zero out y velocities
		//finalSteering = Vector3.ClampMagnitude(finalSteering, maxForce);

		//v_0 + a*t
		Vector3 finalVelocity = currentVelocity + finalSteering * Time.deltaTime;
		finalVelocity = Vector3.ClampMagnitude (finalVelocity, maxSpeed);

		//p_0 + v*t
		this.transform.position = currentPos + finalVelocity * Time.deltaTime;
		if (finalVelocity != Vector3.zero)
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (finalVelocity), Time.deltaTime * 2);

		//update current velocity for the next second
		currentVelocity = finalVelocity;
	}

	public Vector3 ObstacleAvoidance (float minDetectDist, float weight)
	{
		Vector3 obsSteering = Vector3.zero;
		//change the detecting distance based on currentVelocity
		float detectDistance = minDetectDist + currentVelocity.magnitude / maxSpeed * minDetectDist;

		//Raycast ahead
		Vector3 raycastVector = currentVelocity.normalized * detectDistance;
		obsSteering += ObstacleHelperMethod (raycastVector, detectDistance);

		//Raycast right
		raycastVector = Quaternion.Euler (0, 45, 0) * raycastVector; //rotating +45 degrees
		obsSteering += ObstacleHelperMethod (raycastVector, detectDistance);

		//Raycast left
		raycastVector = Quaternion.Euler (0, -90, 0) * raycastVector; //rotating +45-90 = -45 degrees
		obsSteering += ObstacleHelperMethod (raycastVector, detectDistance);

		finalSteering += obsSteering * weight;
		return obsSteering * weight;
	}

	private Vector3 ObstacleHelperMethod (Vector3 raycastVector, float detectDistance)
	{
		Vector3 combinedSteering = Vector3.zero;
		//Debug.DrawRay (currentPos, raycastVector.normalized * detectDistance, Color.yellow);
		if (Physics.Raycast (currentPos, raycastVector, out hitInfo, detectDistance)) {
			if (hitInfo.transform.tag == sphereTag) {
				Vector3 collisionVelocity = hitInfo.transform.position - this.transform.position;
				Vector3 steering = raycastVector - collisionVelocity;
				//Debug.DrawRay (currentPos, steering, Color.red);
				combinedSteering += steering;
			}

			if (hitInfo.transform.tag == wallTag) {
				float lengthPastWall = ((currentPos + raycastVector.normalized * detectDistance) - hitInfo.point).magnitude;
				Vector3 steering = hitInfo.normal.normalized * lengthPastWall;
				//Debug.DrawRay (currentPos, steering, Color.black);
				combinedSteering += steering;
			}
		}
		return combinedSteering;
	}

	public void Seek (Vector3 targetPos)
	{
		finalSteering += _Seek (targetPos);
	}

	private Vector3 _Seek (Vector3 targetPos)
	{
		Vector3 desiredVelocity = (targetPos - currentPos).normalized * maxSpeed;
		Vector3 steering = desiredVelocity - currentVelocity;
		if (steering.magnitude < 1f)
			steering = steering.normalized;
		//else
		//	steering = Vector3.ClampMagnitude (steering, maxForce);
		return steering;
	}

	public void Flee (Vector3 targetPos)
	{
		finalSteering += _Flee (targetPos);
	}

	private Vector3 _Flee (Vector3 targetPos)
	{
		//same as Seek except desiredVelocity is opposite (currentPos - targetPos)
		Vector3 desiredVelocity = (currentPos - targetPos).normalized * maxSpeed;
		Vector3 steering = desiredVelocity - currentVelocity;
		if (steering.magnitude < 1f)
			steering = steering.normalized;
		//Debug.DrawRay (currentPos, steering, Color.white);
		return steering;
	}

	public void Arrive (Vector3 targetPos, float slowDistance)
	{
		finalSteering += _Arrive (targetPos, slowDistance);
	}

	private Vector3 _Arrive (Vector3 targetPos, float slowDistance)
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

	public void Wander (float angleRange, float maxAngle)
	{
		finalSteering += _Wander (angleRange, maxAngle);
	}
	private Vector3 _Wander (float angleRange, float maxAngle)
	{
		randomAngle = randomAngle + Random.Range (-angleRange, angleRange);
		//reset randomAngle if it goes past maxAngle degrees
		if (Mathf.Abs (randomAngle) > maxAngle)
			randomAngle = 0f;
		Vector3 steering = Quaternion.Euler (0, randomAngle, 0) * currentVelocity;
		if (currentVelocity == Vector3.zero)
			steering = Quaternion.Euler (0, randomAngle, 0) * transform.forward;
		steering = steering.normalized * maxSpeed;
		//Debug.DrawRay (currentPos, steering, Color.blue);
		return steering;
	}

	public void FollowLeader (MoveController leaderController, float minDistBehind)
	{
		Vector3 leaderDirection = leaderController.GetCurrentVelocity ().normalized;
		Vector3 leaderPos = leaderController.transform.position;
		Vector3 desiredPos = leaderPos - leaderDirection * minDistBehind;
		finalSteering += _Seek (desiredPos);
	}

	public void Alignment (float neighborRange)
	{
		finalSteering += _Alignment (neighborRange);
	}
	public void Alignment (Vector3 avgVelocity)
	{
		finalSteering += avgVelocity;
	}
	private Vector3 _Alignment (float neighborRange)
	{
		Vector3 steering = Vector3.zero;
		float neighborCount = 0f;
		foreach (MoveController moveControl in group) {
			Vector3 neighborDirection = moveControl.transform.position - currentPos;
			float distance = neighborDirection.magnitude;
			if (distance <= neighborRange && distance != 0) {
				steering += moveControl.GetCurrentVelocity ();
				neighborCount++;
			}
		}
		if (neighborCount != 0f)
			steering = steering / neighborCount;
		return steering.normalized;
	}

	public void Cohesion (float neighborRange)
	{
		finalSteering += _Cohesion (neighborRange);
	}

	private Vector3 _Cohesion (float neighborRange)
	{
		Vector3 steering = Vector3.zero;
		Vector3 centerPos = Vector3.zero;
		float neighborCount = 0f;
		foreach (MoveController moveControl in group) {
			Vector3 neighborDirection = moveControl.transform.position - currentPos;

			float distance = neighborDirection.magnitude;
			if (distance <= neighborRange && distance != 0) {
				centerPos += moveControl.transform.position;
				neighborCount++;
			}
		}
		if (neighborCount != 0f)
			centerPos = centerPos / neighborCount;
		steering = _Seek (centerPos);
		return steering.normalized;
	}

	public void Separation (float neighborRange, float weight)
	{
		if (group != null)
			finalSteering += _Separation (neighborRange) * weight;
	}
	private Vector3 _Separation (float neighborRange)
	{
		Vector3 steering = Vector3.zero;
		float neighborCount = 0f;
		foreach (MoveController moveControl in group) {
			Vector3 neighborDirection = moveControl.transform.position - currentPos;
			float distance = neighborDirection.magnitude;
			if (distance <= neighborRange && distance != 0) {
				steering += _Flee (moveControl.transform.position);
				neighborCount++;
			}
		}
		if (neighborCount != 0f)
			steering = steering / neighborCount;
		return steering.normalized;
	}


	public void SetPath (List<Node> _path)
	{
		path = _path;
		destNodeIndex = 0;
	}

	public void FollowPath (float smoothRadius)
	{
		Vector3 steering = Vector3.zero;
		if (path != null) {
			Vector3 destination = path [destNodeIndex].worldPosition;
			float distance = (currentPos - destination).magnitude;
			steering = _Arrive (destination, Grid.Instance.nodeRadius);
			if (distance <= smoothRadius) {
				if (destNodeIndex < path.Count - 1)
					destNodeIndex++;
			}
		}
		finalSteering += steering;
	}

	public Vector3 GetCurrentVelocity ()
	{
		return currentVelocity;
	}
}
