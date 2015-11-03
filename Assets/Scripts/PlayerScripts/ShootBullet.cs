using UnityEngine;
using System.Collections;

public class ShootBullet : MonoBehaviour {
    public float bulletSpeed = 10;
    public Rigidbody bullet;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
            Fire();

    }
    void Fire()
    {
        Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, transform.position, transform.rotation);
        bulletClone.velocity = transform.forward * bulletSpeed;
    }
}
