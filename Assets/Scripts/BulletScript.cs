using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
//	private float bulletSpeed = 30f;
	// Use this for initialization
	void Start ()
	{
		Destroy (this.gameObject, 5f);
	}

	// Update is called once per frame
	void Update ()
	{
//		this.transform.Translate (-Vector3.up * bulletSpeed * Time.deltaTime);
	}

	void OnTriggerEnter (Collider col)
	{
		//Destroy (col.gameObject);

		if (col.tag == "Zombie") {
			Debug.Log ("collision");
			col.GetComponent<EnemyHealth> ().ReduceHealth (40f);
		}
	}
}

