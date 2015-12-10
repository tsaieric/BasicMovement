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
			bool allDead = true;
			foreach (GameObject guardian in guardians) {
				if (guardian == null) {
					Debug.Log ("guaridna is dead");
					allDead = allDead && true;
				} else {
					allDead = false;
				}
			}
			if (allDead) {
				Debug.Log ("VICTORY");
				Application.LoadLevel ("VictoryScene");
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
