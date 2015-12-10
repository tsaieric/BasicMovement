using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
	public float currentHealth = 100f;
	public float totalHealth = 100f;
	private Transform healthBar;
	public bool isAlive = true;
	private Animator anim;
	// Use this for initialization
	void Awake ()
	{
		anim = this.GetComponent<Animator> ();
		healthBar = this.transform.Find ("HealthBarCanvas/HealthColor");
	}
	
	public float GetHealth ()
	{
		return currentHealth;
	}
	public void ReduceHealth (float difference)
	{
		if (isAlive) {
			float newHealth = Mathf.Max (0, currentHealth - difference);
			float speed = 1f;
			currentHealth = newHealth;
			if (currentHealth <= 0) {
				//anim.SetBool("Alive", false);
				//anim.SetTrigger("Die");
				//isAlive = false;
				//Destroy(this.gameObject, 1.3f);
			}
			StartCoroutine (SetBar (newHealth, speed));
		}
	}

	public void AddHealth (float difference)
	{
		if (isAlive) {
			float newHealth = Mathf.Min (totalHealth, currentHealth + difference);
			float speed = 1f;
			currentHealth = newHealth;
			StartCoroutine (SetBar (newHealth, speed));
		}
	}

	//IEnumerator SetHealth (float input, float speed)
	//{
	//	while (currentHealth!=input) {
	//		currentHealth = Mathf.MoveTowards (currentHealth, input, speed * Time.deltaTime);
	//		yield return null;
	//	}
	//}

	IEnumerator SetBar (float input, float speed)
	{
		float ratio = input / totalHealth;
		Vector3 dest = new Vector3 (ratio, 1f, 1f);
		while (Vector3.Distance(healthBar.transform.localScale, dest) > .05f) {//healthBar.transform.localScale != dest)
			Vector3 curScale = healthBar.transform.localScale;
			healthBar.transform.localScale = Vector3.MoveTowards (curScale, dest, speed * Time.deltaTime);
			yield return null;
		}
		if (currentHealth <= 0 && isAlive) {
			anim.SetBool ("Alive", false);
			anim.SetTrigger ("Die");
			isAlive = false;
			Destroy (this.gameObject, 3f);
		}
	}
}