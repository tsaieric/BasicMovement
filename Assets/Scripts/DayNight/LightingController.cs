using UnityEngine;
using System.Collections;

public class LightingController : MonoBehaviour
{
	private bool isDay = true;
	public float speed;
	private float totalChange = 0f;
	private GameObject[] zombies;
	void Awake ()
	{
		zombies = GameObject.FindGameObjectsWithTag ("Zombie");
	}

	// Update is called once per frame
	void Update ()
	{
		totalChange += Time.deltaTime * speed;
		if (Mathf.Abs (totalChange - 3f) <= .01f) {
			totalChange = 0;
			isDay = !isDay;
			Debug.Log (isDay);
		}
		this.transform.RotateAround (-Vector3.forward, 1f * Time.deltaTime * speed);
	}
}
