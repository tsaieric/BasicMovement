using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Walker : MonoBehaviour
{
	public GameObject targetObj;
	public Behavior thisBehavior;
	public GameObject[] fleeFrom;
	private Vector3 targetPosition;
	private MoveController moveController;
	private MoveController targetMoveController;

	// Use this for initialization
	void Start ()
	{
		moveController = this.GetComponent<MoveController> ();
		if (targetObj != null)
			targetMoveController = targetObj.GetComponent<MoveController> ();
		if (thisBehavior == Behavior.SeekAStar3D) {
			moveController.Set3D (true);
		}
	}

	void FixedUpdate ()
	{
		if (targetObj != null)
			targetPosition = targetObj.transform.position;

		moveController.ResetSteering ();
		switch (thisBehavior) {
		case Behavior.SeekAStar3D:
			moveController.FollowPath3d (5f);
			break;
		case Behavior.SeekAStar:
			moveController.FollowPath (5f);
			break;
		case Behavior.Seek:
			moveController.Seek (targetPosition);
			break;
		case Behavior.Flee:
			float distance = (this.transform.position - targetPosition).magnitude;
			if (distance <= 30f)
				moveController.Flee (targetPosition);
			else
				moveController.Wander (15f, 150f);
			break;
		case Behavior.Arrive:
			moveController.Arrive (targetPosition, 20f);
			break;
		case Behavior.Wander:
			moveController.Wander (15f, 120f);
			break;
		case Behavior.FlockingWander:
			float neighborRange = 5f;
			moveController.FollowLeader (targetMoveController, 1f);
			moveController.Cohesion (neighborRange);
			moveController.Alignment (neighborRange);
			break;
		case Behavior.FleeFromGroup:
			bool awayFromAll = true;
			foreach (GameObject zombie in fleeFrom) {
				float distFromBigZombie = (this.transform.position - zombie.transform.position).magnitude;
				if (distFromBigZombie <= 30f) {
					awayFromAll = false;
					moveController.Flee (zombie.transform.position);
				}
			}
			if (awayFromAll)
				moveController.Wander (15f, 120f);
			break;
		case Behavior.Formation:
			break;
		}

		if (this.thisBehavior != Behavior.Formation) {
			moveController.Separation (3f, 30f);
			moveController.ObstacleAvoidance (2f, 30f);
			moveController.UpdateEverything ();
		}
	}
}
