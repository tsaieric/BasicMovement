using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum DogBehavior
{
	Seek,
	Flee,
	Arrive,
	Wander,
	FlockingWander,
	FleeFromGroup,
	Formation,
	SeekAStar,
	SeekAStar3D
}

public class DogMovement: MonoBehaviour
{
	public GameObject targetObj;
	public GameObject[] runAwayFrom;
	public DogBehavior thisBehavior;
	private Vector3 targetPosition;
	private MoveController moveController;
	private MoveController targetMoveController;
	private Animator anim;
	private Transform player;
	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		moveController = this.GetComponent<MoveController> ();
		anim = this.GetComponent<Animator> ();
		if (targetObj != null)
			targetMoveController = targetObj.GetComponent<MoveController> ();
		if (thisBehavior == DogBehavior.SeekAStar3D) {
			moveController.Set3D (true);
		}
	}
	
	void FixedUpdate ()
	{
		if (targetObj != null)
			targetPosition = targetObj.transform.position;
		
		moveController.ResetSteering ();
		switch (thisBehavior) {
		case DogBehavior.SeekAStar3D:
			moveController.FollowPath3d (5f);
			break;
		case DogBehavior.SeekAStar:
			moveController.FollowPath (5f);
			break;
		case DogBehavior.Seek:
			moveController.Seek (targetPosition);
			break;
		case DogBehavior.Flee:
			float distance = (this.transform.position - targetPosition).magnitude;
			if (distance <= 30f)
				moveController.Flee (targetPosition);
			else
				moveController.Wander (15f, 150f);
			break;
		case DogBehavior.Arrive:
			moveController.Arrive (targetPosition, 20f);
			break;
		case DogBehavior.Wander:
			moveController.Wander (15f, 120f);
			break;
		case DogBehavior.FlockingWander:
			float neighborRange = 5f;
			moveController.FollowLeader (targetMoveController, 1f);
			moveController.Cohesion (neighborRange);
			moveController.Alignment (neighborRange);
			break;
		case DogBehavior.FleeFromGroup:
			bool awayFromAll = true;
			foreach (GameObject zombie in runAwayFrom) {
				float distFromBigZombie = (this.transform.position - zombie.transform.position).magnitude;
				if (distFromBigZombie <= 30f) {
					awayFromAll = false;
					moveController.Flee (zombie.transform.position);
				}
			}
			if (awayFromAll)
				moveController.Wander (15f, 120f);
			break;
		case DogBehavior.Formation:
			break;
		}

		float distanceFromPlayer = Vector3.Distance (this.transform.position, player.position);

		moveController.maxSpeed = Mathf.Clamp (distanceFromPlayer, 1f, 20f);
		if (this.thisBehavior != DogBehavior.Formation) {
			moveController.Separation (3f, 30f);
			moveController.ObstacleAvoidance (2f, 30f);
			moveController.UpdateEverything ();
		}
		
		anim.SetFloat ("currentSpeed", moveController.GetCurrentVelocity ().magnitude);
	}
}
