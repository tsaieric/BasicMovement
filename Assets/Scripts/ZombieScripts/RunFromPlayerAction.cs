using UnityEngine;
using System.Collections;
using System;

public class RunFromPlayerAction : Action
{
	private Health[] players;
	private GameObject player;
	private ZombieMovement movement;
	private Animator anim;
	
	void Start ()
	{
		anim = this.GetComponent<Animator> ();
		movement = this.GetComponent<ZombieMovement> ();
		players = Health.FindObjectsOfType<Health> ();
	}
	
	public RunFromPlayerAction ()
	{
		AddPrecondition ("nearMax", true);
		AddEffect ("healthLow", false); //run away?
	}
	
	//addrange later
	public override bool CheckInRange ()
	{
		float closestDistance = 0;
		foreach (Health playerX in players) {
			if (playerX != null) {
				if (playerX.isAlive) {
					if (player == null) {
						closestDistance = Vector3.Distance (this.transform.position, playerX.transform.position);
						player = playerX.gameObject;
					} else {
						float dist = Vector3.Distance (this.transform.position, playerX.transform.position);
						if (dist < closestDistance) {
							player = playerX.gameObject;
							closestDistance = dist;
						}
					}
				}
			}
		}
		if (closestDistance > 40f) {
			player = null;
		}
		target = player;
		return player != null;
	}
	
	public override void Reset ()
	{
		player = null;
		target = null;
	}
	
	public override bool Act ()
	{
		Debug.Log ("acting?");
		if (player == null) {
			anim.SetBool ("isWalking", true);
			anim.SetBool ("isAttacking", false);
			return true;
		}
		movement.targetObj = player;
		float distance = Vector3.Distance (this.transform.position, player.transform.position);
		if (distance >= 50f) {
//			if (anim.GetBool ("isWalking")) {
//				anim.SetBool ("isWalking", false);
//			}
			movement.thisBehavior = Behavior.Idle;
			return true;
		} else {
//			if (!anim.GetBool ("isWalking")) {
//				anim.SetBool ("isWalking", true);
//			}
			movement.thisBehavior = Behavior.Flee;
			return false;
		}
	}
	
	public override bool isDone ()
	{
		throw new NotImplementedException ();
	}
	
	public override bool RequiresInRange ()
	{
		return true;
	}
	
	public override string Print ()
	{
		return "runFromPlayerAction";
	}
}
