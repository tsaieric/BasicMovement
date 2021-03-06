﻿using UnityEngine;
using System.Collections;

public class ZombieActions : MonoBehaviour
{
	private Transform playerT;
	private float attackDist;
	private Animator anim;
	private PlayerHealth playerHealth;
	void Awake ()
	{
		playerT = GameObject.FindGameObjectWithTag ("Player").transform;		
		playerHealth = playerT.GetComponent<PlayerHealth> ();
		anim = this.GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start ()
	{
		attackDist = 2f;
	}
	
	// Update is called once per frame
	void Update ()
	{
//		float distanceFromPlayer = Vector3.Distance (playerT.position, this.transform.position);
//		if (distanceFromPlayer <= attackDist && !anim.GetBool ("isAttacking")) {
//			anim.SetBool ("isAttacking", true);
//		} else {
//			anim.SetBool ("isAttacking", false);
//		}
	}

	public void Attack ()
	{
		playerHealth.ReduceHealth (10f);
	}
}
