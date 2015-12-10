using UnityEngine;
using System.Collections;

public class GrenadeScript : MonoBehaviour
{
	public float damage = 50f;
	public float range = 8f;
	private EnemyHealth[] enemyHealths;
	private float timeBeforeExplosion = 1.5f;
	// Use this for initialization
	void Start ()
	{
		enemyHealths = FindObjectsOfType<EnemyHealth> ();
		Destroy (this.gameObject, timeBeforeExplosion);
		StartCoroutine (Explode (timeBeforeExplosion));
	}

	IEnumerator Explode (float delay)
	{
		yield return new WaitForSeconds (delay - .1f);
		GameObject explosionParticles = (GameObject)Instantiate (Resources.Load ("GrenadeExplosion"), this.transform.position, Quaternion.identity);
		Destroy (explosionParticles, 2f);
		DamageNearestEnemies ();
        SoundManager.Instance.PlayGrenade();
	}

	void DamageNearestEnemies ()
	{
		foreach (EnemyHealth enemy in enemyHealths) {
			if (enemy != null) {
				if (Vector3.Distance (enemy.transform.position, this.transform.position) < range) {
					enemy.ReduceHealth (damage);
				}
			}
		}
	}
}
