using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
	protected float currentHealth = 100f;
	protected float totalHealth = 100f;
	protected Transform healthBar;
	protected Animator anim;
	private Health[] healths;
	public bool isAlive = true;

	virtual protected void  Awake ()
	{
		anim = this.GetComponent<Animator> ();
		healthBar = this.transform.Find ("HealthBarCanvas/HealthColor");
		healths = GameObject.FindObjectsOfType<Health> ();
	}

	public float GetHealth() {
		return currentHealth;
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

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "HealthPack") {
			Destroy (other.gameObject);
            UIController.Instance.DecreaseFoodCount();
			foreach (Health h in healths) {
				h.AddHealth (15f);
			}
		}
	}

	virtual public IEnumerator SetBar (float input, float speed)
	{
		if (isAlive) {
			float ratio = input / totalHealth;
			Vector3 dest = new Vector3 (ratio, 1f, 1f);
			while (healthBar.transform.localScale != dest) {
				Vector3 curScale = healthBar.transform.localScale;
				healthBar.transform.localScale = Vector3.MoveTowards (curScale, dest, speed * Time.deltaTime);
				yield return null;
			}
		}
	}
}
