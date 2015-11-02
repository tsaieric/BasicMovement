using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	private float currentHealth = 100f;
	private float totalHealth = 100f;
	private Slider healthSlider;
	// Use this for initialization
	void Awake ()
	{
		healthSlider = GameObject.FindGameObjectWithTag ("PlayerHealthSlider").GetComponent<Slider> ();
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
		float speed = 10f;
		StartCoroutine (SetHealth (newHealth, speed));
	}

	void AddHealth (float difference)
	{
		float newHealth = Mathf.Min (totalHealth, currentHealth + difference);
		float speed = 10f;
		StartCoroutine (SetHealth (newHealth, speed));
	}

	IEnumerator SetHealth (float input, float speed)
	{
		while (currentHealth!=input) {
			currentHealth = Mathf.MoveTowards (currentHealth, input, speed * Time.deltaTime);
			healthSlider.value = currentHealth;
			yield return null;
		}
	}

//	IEnumerator SetBar (float input, float speed)
//	{	
//		float ratio = input / totalHealth;
//		Vector3 dest = new Vector3 (ratio, 1f, 1f);
//		healthSlider.
//		while (healthBar.transform.localScale!=dest) {
//			Vector3 curScale = healthBar.transform.localScale;
//			healthBar.transform.localScale = Vector3.MoveTowards (curScale, dest, speed * Time.deltaTime);
//			yield return null;
//		}
//	}
}
