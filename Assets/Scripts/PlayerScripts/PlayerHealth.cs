using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : Health
{
	public Image blackOverlay;

	// Use this for initialization
	protected override void Awake ()
	{
		blackOverlay.gameObject.SetActive (true);
		blackOverlay.canvasRenderer.SetAlpha (0);
		base.Awake ();
	}

	public void ReduceHealth (float difference)
	{
		if (isAlive) {
			float newHealth = Mathf.Max (0, currentHealth - difference);
			float speed = 1f;
			currentHealth = newHealth;
			StartCoroutine (SetBar (newHealth, speed));
			StartCoroutine (FlashRed (.2f));
		}
	}

	//IEnumerator SetHealth(float input, float speed)
	//{
	//    while (currentHealth != input)
	//    {
	//        currentHealth = Mathf.MoveTowards(currentHealth, input, speed * Time.deltaTime);
	//        yield return null;
	//    }
	//}
	public override IEnumerator SetBar (float input, float speed)
	{
		if (isAlive) {
			yield return StartCoroutine (base.SetBar (input, speed));
			if (currentHealth <= 0) {
				StartCoroutine (EndGame ());
			}
		}
	}

	IEnumerator FlashRed (float time)
	{
		blackOverlay.color = Color.red;
		blackOverlay.CrossFadeAlpha (.4f, time, false);
		yield return new WaitForSeconds (time);
		blackOverlay.canvasRenderer.SetAlpha (0f);
	}

	IEnumerator EndGame ()
	{
		Time.timeScale = .2f;
		anim.SetTrigger ("Die");
		isAlive = false;
		yield return new WaitForSeconds (.2f);
		blackOverlay.color = Color.black;
		blackOverlay.CrossFadeAlpha (1f, 1f, false);
		yield return new WaitForSeconds (1f);
		Application.LoadLevel ("EndScene");
	}

}