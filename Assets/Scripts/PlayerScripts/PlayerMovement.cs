using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public bool disabled = false;
	public float speed = 6f;
	public LayerMask floorMask;
	float camRayLength = 1000f;
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

    void Update()
    {
		if(Input.GetMouseButtonDown(0)||Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("Punch");
        }
    }
	// Update is called once per frame
	void FixedUpdate ()
	{
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		if (disabled) {
			v = 0f;
			h = 0f;
		}
		Move (h, v);
		Animating (h, v);
		TurnToMouse ();
	}

	void TurnToMouse ()
	{
		Ray mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit rayHitInfo;
		if (Physics.Raycast (mouseRay, out rayHitInfo, camRayLength, floorMask)) {
			Vector3 playerToMouse = rayHitInfo.point - this.transform.position;
			playerToMouse.y = 0f;
			playerRigidbody.MoveRotation (Quaternion.LookRotation (playerToMouse));
		}
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
		Vector3 moveDir = new Vector3 (h, 0, v);
		//animate in reverse if you're moving backwards or forwards
		float result = Vector3.Dot (moveDir, this.transform.forward);
		if (result >= 0) {
			anim.SetFloat ("currentSpeed", speed);
		} else {
			anim.SetFloat ("currentSpeed", -speed);
		}
	}
}
