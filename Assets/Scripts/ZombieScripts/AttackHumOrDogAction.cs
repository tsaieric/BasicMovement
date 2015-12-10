using UnityEngine;
using System.Collections;
using System;

public class AttackHumOrDogAction : Action
{
	public float healthLoss = 10f;
	public float attackRadius = 2f;
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
	
	public AttackHumOrDogAction ()
	{
		AddPrecondition ("healthLow", false);
		AddPrecondition ("nearMax", true);
		AddEffect ("attackingPlayer", true);
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
			anim.SetBool ("isAttacking", false);
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
		if (player == null) {
			anim.SetBool ("isWalking", true);
			anim.SetBool ("isAttacking", false);
			return true;
		}
		float distance = Vector3.Distance (this.transform.position, player.transform.position);
		if (distance <= attackRadius) {
			if (!anim.GetBool ("isAttacking")) {
				anim.SetBool ("isAttacking", true);
			}
			if (anim.GetBool ("isWalking")) {
				anim.SetBool ("isWalking", false);
			}
			return true;
		} else {
			anim.SetBool ("isWalking", true);
			anim.SetBool ("isAttacking", false);
			movement.targetObj = player;
			movement.thisBehavior = Behavior.SeekAStar;
			//MoveController seeks target
			return false;
		}
	}
	
	public void AttackHumOrDog ()
	{
		if (player != null) {
			PlayerHealth health = player.GetComponent<PlayerHealth> ();
			if (health != null) {
				health.ReduceHealth (healthLoss);
			} else {
				player.GetComponent<DogHealth> ().ReduceHealth (healthLoss);
			}
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
		return "attackPlayerAction";
	}
}
