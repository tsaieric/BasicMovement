using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
	private float currentHealth = 100f;
	private float totalHealth = 100f;
	private Transform healthBar;
	public bool isAlive = true;
	private Animator anim;
	// Use this for initialization
	void Awake ()
	{
		anim = this.GetComponent<Animator> ();
		healthBar = this.transform.Find ("HealthBarCanvas/HealthColor");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isAlive) {
			this.GetComponent<Rigidbody> ().isKinematic = true;
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			ReduceHealth (30f);
		}
		if (Input.GetKeyDown (KeyCode.V)) {
			AddHealth (10f);
		}
	}

	public void ReduceHealth (float difference)
	{
		float newHealth = Mathf.Max (0, currentHealth - difference);
		float speed = 1f;
		currentHealth = newHealth;
		if (currentHealth <= 0 && isAlive) {
			anim.SetBool ("Alive", false);
			anim.SetTrigger ("Die");
			isAlive = false;
			Destroy (this.gameObject, 1.3f);
		}
//		StartCoroutine (SetHealth (newHealth, speed));
		StartCoroutine (SetBar (newHealth, speed));
	}

	public void AddHealth (float difference)
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
