using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour
{
	public float maxSpeed = 10f;
	public float maxForce = 20f;

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
        if(transform.parent !=null)
            group = transform.parent.GetComponentsInChildren<MoveController>();
	}

    void FixedUpdate()
    {
        currentPos = this.transform.position;
    }

    public void ResetSteering() {
        finalSteering = Vector3.zero;
    }

    public void UpdateEverything()
    {
        finalSteering.y = 0f; //zero out y velocities
        finalSteering = Vector3.ClampMagnitude(finalSteering, maxForce);

        //v_0 + a*t
        Vector3 finalVelocity = currentVelocity + finalSteering * Time.deltaTime;
        finalVelocity = Vector3.ClampMagnitude(finalVelocity, maxSpeed);

        //p_0 + v*t
        this.transform.position = currentPos + finalVelocity * Time.deltaTime;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(finalVelocity), Time.deltaTime * 2);

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

        finalSteering += obsSteering*weight;
		return obsSteering * weight;
	}

    private Vector3 ObstacleHelperMethod(Vector3 raycastVector, float detectDistance)
    {
        Vector3 combinedSteering = Vector3.zero;
        Debug.DrawRay(currentPos, raycastVector.normalized * detectDistance, Color.yellow);
            if (Physics.Raycast(currentPos, raycastVector, out hitInfo, detectDistance))
            {
                if (hitInfo.transform.tag == sphereTag)
                {
                    Vector3 collisionVelocity = hitInfo.transform.position - this.transform.position;
                    Vector3 steering = raycastVector - collisionVelocity;
                    Debug.DrawRay(currentPos, steering, Color.red);
                    combinedSteering += steering;
                }

                if (hitInfo.transform.tag == wallTag)
                {
                    float lengthPastWall = ((currentPos + raycastVector.normalized * detectDistance) - hitInfo.point).magnitude;
                    Vector3 steering = hitInfo.normal.normalized * lengthPastWall;
                    Debug.DrawRay(currentPos, steering, Color.black);
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
        finalSteering += steering;
        return steering;
	}

	public Vector3 Flee (Vector3 targetPos)
	{
            //same as Seek except desiredVelocity is opposite (currentPos - targetPos)
            Vector3 desiredVelocity = (currentPos - targetPos).normalized * maxSpeed;
            Vector3 steering = desiredVelocity - currentVelocity;
            if (steering.magnitude < 1f)
                steering = steering.normalized;
        //else
        //    steering = Vector3.ClampMagnitude(steering, maxForce);
        finalSteering += steering;
        return steering;
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
        finalSteering += steering;
        return steering;
	}

	public Vector3 Wander (float angleRange, float maxAngle)
	{
		randomAngle = randomAngle + Random.Range (-angleRange, angleRange);
        //reset randomAngle if it goes past maxAngle degrees
        if (Mathf.Abs(randomAngle) > maxAngle)
            randomAngle = 0f;
        Vector3 steering = Quaternion.Euler (0, randomAngle, 0) * currentVelocity;
		if (currentVelocity == Vector3.zero)
			steering = Quaternion.Euler (0, randomAngle, 0) * transform.forward;
        steering = steering.normalized * maxSpeed / 2;
		Debug.DrawRay (currentPos, steering, Color.blue);
        finalSteering += steering*2f;
        return steering;
	}

    public Vector3 FlockingWander()
    {
        Vector3 steering = Vector3.zero;
        float neighborRange = 10f;
        float neighborCount = 0f;
        foreach(MoveController moveControl in group)
        {
            //separation
            float distance = (currentPos - moveControl.transform.position).magnitude;
            if (distance <=neighborRange && distance!=0)
            {
                steering += Flee(moveControl.transform.position);
                neighborCount++;
            }
        }
        finalSteering += steering/neighborCount;
        return steering;
    }

    public Vector3 GetCurrentVelocity()
    {
        return currentVelocity;
    }
}
