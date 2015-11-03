using UnityEngine;
using System.Collections;

public class ShootBullet : MonoBehaviour
{
	public float bulletSpeed = 1;
	public Rigidbody bullet;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Fire1"))
			Fire ();
	}
	public void Fire ()
	{
		Rigidbody bulletClone = (Rigidbody)Instantiate (bullet, transform.position, this.transform.rotation);//Quaternion.identity);
//		bulletClone.velocity = transform.forward.normalized * bulletSpeed;
	}
}
