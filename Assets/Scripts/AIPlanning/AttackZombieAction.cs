using UnityEngine;
using System.Collections;
using System;

public class AttackZombieAction : Action
{

	private EnemyHealth[] enemies2;
	private GameObject targetEnemy;
	private DogMovement movement;
	private Animator anim;
	private EnemyHealth enemyHealth;
	private GameObject player;
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		anim = this.GetComponent<Animator> ();
		movement = this.GetComponent<DogMovement> ();
		enemies2 = EnemyHealth.FindObjectsOfType<EnemyHealth> ();
	}

	public AttackZombieAction ()
	{
		AddPrecondition ("nearMax", true);
		AddPrecondition ("healthLow", false);
		AddEffect ("attackingZombie", true);
	}

	public override bool CheckInRange ()
	{
		float closestDistance = 0;
		foreach (EnemyHealth enemy in enemies2) {
			if (enemy != null) {
				if (enemy.isAlive) {
					if (targetEnemy == null) {
						closestDistance = Vector3.Distance (this.transform.position, enemy.transform.position);
						targetEnemy = enemy.gameObject;
					} else {
						float dist = Vector3.Distance (this.transform.position, enemy.transform.position);
						if (dist < closestDistance) {
							targetEnemy = enemy.gameObject;
							closestDistance = dist;
						}
					}
				}
			}
		}
		if (closestDistance > 40f) {
			movement.targetObj = player;
			movement.thisBehavior = DogBehavior.Arrive;
			targetEnemy = null;
			anim.SetBool ("IsAttacking", false);
			if (!anim.GetBool ("Iswalking")) {
				anim.SetBool ("IsWalking", true);
			}
		}
		target = targetEnemy;
		return targetEnemy != null;
	}

	public override void Reset ()
	{
		targetEnemy = null;
		target = null;
	}

	public override bool Act ()
	{
		if (targetEnemy == null) {
			anim.SetBool ("IsWalking", true);
			anim.SetBool ("IsAttacking", false);
			return true;
		}

		float distance = Vector3.Distance (this.transform.position, targetEnemy.transform.position);
		if (distance <= 5f) {
			if (!anim.GetBool ("IsAttacking")) {
				anim.SetBool ("IsAttacking", true);
			}
			if (anim.GetBool ("IsWalking")) {
				anim.SetBool ("IsWalking", false);
			}
			return true;
		} else {
			anim.SetBool ("IsWalking", true);
			anim.SetBool ("IsAttacking", false);
			movement.targetObj = targetEnemy;
			movement.thisBehavior = DogBehavior.Arrive;
			//MoveController seeks target
			return false;
		}
	}

	public void Attack ()
	{
		if (targetEnemy != null) {
			enemyHealth = targetEnemy.GetComponent<EnemyHealth> ();
			enemyHealth.ReduceHealth (10f);
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
		return "attackZombieAction";
	}
}
