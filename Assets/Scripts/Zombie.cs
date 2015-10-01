using UnityEngine;
using System.Collections;
public enum Behavior
{
	Seek,
	Flee,
	Arrive,
	Wander,
	FlockingWander,
	FleeFromGroup,
	Formation
}

public class Zombie : MonoBehaviour
{
	public GameObject targetObj;
	public GameObject[] bigZombies;
	public Behavior thisBehavior;

	private Vector3 targetPosition;
	private MoveController moveController;
	private MoveController targetMoveController;
	private Animator anim;

	// Use this for initialization
	void Start ()
	{
		moveController = this.GetComponent<MoveController> ();
		anim = this.GetComponent<Animator> ();
		if (targetObj != null)
			targetMoveController = targetObj.GetComponent<MoveController> ();
	}

	void FixedUpdate ()
	{
		if (targetObj != null)
			targetPosition = targetObj.transform.position;

		moveController.ResetSteering ();
		switch (thisBehavior) {
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
			foreach (GameObject zombie in bigZombies) {
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
			moveController.Separation (5f, 10f);
			moveController.ObstacleAvoidance (2f, 30f);
			moveController.UpdateEverything ();
		}

		//change Zombie animation speed based on current speed
		anim.SetFloat ("currentSpeed", moveController.GetCurrentVelocity ().magnitude);
	}
}
