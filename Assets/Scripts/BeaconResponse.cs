using UnityEngine;
using System.Collections;

public class BeaconResponse : MonoBehaviour
{
	public GameObject[] guardians;
	private GameObject arrow;
	// Use this for initialization
	void Start ()
	{
		arrow = GameObject.FindGameObjectWithTag ("Arrow");
		arrow.SetActive (false);
	}

	void OnTriggerStay (Collider other)
	{
		//Vector3 direction;
		if (other.tag == "Radar") {
			arrow.SetActive (true);
			//direction = this.transform.position - other.transform.position;
			Vector3 pos = this.transform.position;
			//Debug.DrawLine(other.transform.position, other.transform.position + direction * 1000f, Color.black, 100f);
			Vector3 sameHeightBeacon = new Vector3 (pos.x, arrow.transform.position.y, pos.z);
			arrow.transform.LookAt (sameHeightBeacon);
		}
		
		if (other.tag == "Player") {
            Debug.Log("player in beacon");
			bool allDead = true;
			foreach (GameObject guardian in guardians) {
				if (guardian == null) {
					allDead = allDead && true;
				} else {
					if (!guardian.GetComponent<EnemyHealth> ().isAlive) {
						allDead = allDead && true;
					} else {
						allDead = false;
					}
				}
			}
			if (allDead) {
				Debug.Log ("VICTORY");
				StartCoroutine (other.gameObject.GetComponent<PlayerHealth> ().WinGame ());
				//load victory scene
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		//Vector3 direction;
		if (other.tag == "Radar") {
			arrow.SetActive (false);
			//direction = this.transform.position - other.transform.position;
			Vector3 pos = this.transform.position;
			//Debug.DrawLine(other.transform.position, other.transform.position + direction * 1000f, Color.black, 100f);
			Vector3 sameHeightBeacon = new Vector3 (pos.x, arrow.transform.position.y, pos.z);
			arrow.transform.LookAt (sameHeightBeacon);
		}
	}
}
