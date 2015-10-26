using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;

	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
	// Use this for initialization
	void Start ()
	{

		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		Move (h, v);
		//this.transform.position += new Vector3(h, 0, v) * Time.deltaTime * speed;        
		this.transform.rotation = Quaternion.LookRotation (new Vector3 (h, 0, v));
		Animating (h, v);
	}


	void Move (float h, float v)
	{
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.position + movement);

	}

	void Animating (float h, float v)
	{
		bool walking = h != 0f || v != 0f;
		// Debug.Log(walking);
		anim.SetBool ("IsWalking", walking);
	}
}
