using UnityEngine;
using System.Collections;

public class GrenadeScript : MonoBehaviour
{
	private float timeBeforeExplosion = 1.5f;
	// Use this for initialization
	void Start ()
	{
		Destroy (this.gameObject, timeBeforeExplosion);
		StartCoroutine (Explode (timeBeforeExplosion));
	}

	IEnumerator Explode (float delay)
	{
		yield return new WaitForSeconds (delay - .2f);
		GameObject explosionParticles = (GameObject)Instantiate (Resources.Load ("GrenadeExplosion"), this.transform.position, Quaternion.identity);
		Destroy (explosionParticles, 2f);

	}
}
