using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class DogHealth : Health
{
	// Use this for initialization
	protected override void Awake ()
	{
		base.Awake ();
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.H)) {
			ReduceHealth (10f);
		}
	}

	public void ReduceHealth (float difference)
	{
		if (isAlive) {
			float newHealth = Mathf.Max (0, currentHealth - difference);
			float speed = 1f;
			currentHealth = newHealth;
			StartCoroutine (SetBar (newHealth, speed));
		}
	}

	public override IEnumerator SetBar (float input, float speed)
	{
		if (isAlive) {
			yield return StartCoroutine (base.SetBar (input, speed));
			if (currentHealth <= 0) {
				Destroy (this.gameObject);
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "HealthPack") {
			Debug.Log ("reached health pack");
			AddHealth (15f);
			Destroy (other.gameObject);
		}
	}

	public float GetHealth ()
	{
		return currentHealth;
	}
}