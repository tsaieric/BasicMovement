using UnityEngine;
using System.Collections;


public class PlayerActions : MonoBehaviour
{
	public float radarSpeed = 10f;
	public Rigidbody bullet;
	public GameObject grenade;
	private float bulletSpeed = 30f;
	private PlayerMovement movement;
	private GameObject radar;
	private GameObject gunEnd;
	// Use this for initialization
	void Start ()
	{
		movement = this.GetComponent<PlayerMovement> ();
		radar = GameObject.FindGameObjectWithTag ("Radar");
		radar.SetActive (false);
		gunEnd = GameObject.FindGameObjectWithTag ("GunEnd");
	}

	// Update is called once per frame
	void Update ()
	{
		RadarControl ();

        if (Input.GetButtonDown ("Fire2")) {
			Fire ();
		}

        if(Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
        }
    }

	void RadarControl ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			radar.SetActive (true);
			movement.disabled = true;
		}

		if (Input.GetKey (KeyCode.Space)) {
			Vector3 curScale = radar.transform.localScale;
			radar.transform.localScale = Vector3.MoveTowards (curScale, curScale * 100, Time.deltaTime * radarSpeed);
			radar.transform.Rotate (Vector3.up, 1f);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			StartCoroutine (ShrinkRadar ());
		}
	}

	IEnumerator ShrinkRadar ()
	{
		while (radar.transform.localScale!=Vector3.one) {
			radar.transform.Rotate (Vector3.up, -1f);
			Vector3 curScale = radar.transform.localScale;
			radar.transform.localScale = Vector3.MoveTowards (curScale, Vector3.one, Time.deltaTime * 100f);
			yield return null;
		}
		radar.SetActive (false);
		movement.disabled = false;
	}

	public void ThrowGrenade ()
	{
		GameObject newGrenade = (GameObject)GameObject.Instantiate (grenade, this.transform.position + this.transform.forward * 3f + this.transform.up * 2f, Quaternion.identity);
		newGrenade.GetComponent<Rigidbody> ().AddForce ((this.transform.forward + Vector3.up) * 4000f);
	}

	public void Fire ()
	{
        SoundManager.Instance.PlayGunShot();
		Rigidbody bulletClone = (Rigidbody)Instantiate (bullet, gunEnd.transform.position, bullet.rotation);//Quaternion.identity);
		bulletClone.velocity = this.transform.forward * bulletSpeed;
	}
}
