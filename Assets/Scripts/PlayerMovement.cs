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
		anim.SetFloat ("currentSpeed", speed);
	}
	
	// Update is called once per frame
	void Update ()
	{
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		Move (h, v);
		//this.transform.position += new Vector3(h, 0, v) * Time.deltaTime * speed;        
		Vector3 direction = new Vector3 (h, 0, v);
		if (direction != Vector3.zero)
			this.transform.rotation = Quaternion.LookRotation (direction);
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
