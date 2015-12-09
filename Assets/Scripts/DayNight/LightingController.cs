using UnityEngine;
using System.Collections;

public class LightingController : MonoBehaviour
{
	public float speed;

	// Update is called once per frame
	void Update ()
	{
		this.transform.RotateAround (-Vector3.forward, 1f * Time.deltaTime * speed);
	}
}
