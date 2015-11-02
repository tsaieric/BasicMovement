using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
	private float currentHealth = 100f;
	private float totalHealth = 100f;
	private Transform healthBar;
	// Use this for initialization
	void Awake ()
	{
		healthBar = this.transform.Find ("HealthBarCanvas/HealthColor");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			ReduceHealth (30f);
		}
		if (Input.GetKeyDown (KeyCode.V)) {
			AddHealth (10f);
		}
	}

	void ReduceHealth (float difference)
	{
		float newHealth = Mathf.Max (0, currentHealth - difference);
		float speed = 1f;
		currentHealth = newHealth;
//		StartCoroutine (SetHealth (newHealth, speed));
		StartCoroutine (SetBar (newHealth, speed));
	}

	void AddHealth (float difference)
	{
		float newHealth = Mathf.Min (totalHealth, currentHealth + difference);
		float speed = 1f;
		currentHealth = newHealth;
//		StartCoroutine (SetHealth (newHealth, speed));
		StartCoroutine (SetBar (newHealth, speed));
	}

	IEnumerator SetHealth (float input, float speed)
	{
		while (currentHealth!=input) {
			currentHealth = Mathf.MoveTowards (currentHealth, input, speed * Time.deltaTime);
			yield return null;
		}
	}

	IEnumerator SetBar (float input, float speed)
	{	
		float ratio = input / totalHealth;
		Vector3 dest = new Vector3 (ratio, 1f, 1f);
		while (healthBar.transform.localScale!=dest) {
			Vector3 curScale = healthBar.transform.localScale;
			healthBar.transform.localScale = Vector3.MoveTowards (curScale, dest, speed * Time.deltaTime);
			yield return null;
		}
	}
}
