﻿using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
	private int damagePerShot = 30;
	public float timeBetweenBullets = 0.15f;
	public float range = 100f;


	float timer;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	ParticleSystem gunParticles;
	LineRenderer gunLine;
	AudioSource gunAudio;
	Light gunLight;
	float effectsDisplayTime = 0.2f;
	GameObject player;

	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		shootableMask = LayerMask.GetMask ("Shootable");
		gunParticles = GetComponent<ParticleSystem> ();
		gunLine = GetComponent<LineRenderer> ();
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
	}


	void Update ()
	{
		timer += Time.deltaTime;

		if (Input.GetButtonDown ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0) {
			Shoot ();
		}

		if (timer >= timeBetweenBullets * effectsDisplayTime) {
			DisableEffects ();
		}
	}


	public void DisableEffects ()
	{
		gunLine.enabled = false;
		gunLight.enabled = false;
	}

	void Shoot ()
	{
		timer = 0f;

        //gunAudio.Play();
        SoundManager.Instance.PlayLaserShot();
		gunLight.enabled = true;

//		gunParticles.Stop ();
//		gunParticles.Play ();

		gunLine.enabled = true;
		gunLine.SetPosition (0, this.transform.position);

		shootRay.origin = this.transform.position;
		shootRay.direction = player.transform.forward;

		if (Physics.Raycast (shootRay, out shootHit, range, shootableMask)) {
			EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth> ();
			if (enemyHealth != null) {
				enemyHealth.ReduceHealth (damagePerShot);
			}
			gunLine.SetPosition (1, shootHit.point);
		} else {
			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
	}
}

