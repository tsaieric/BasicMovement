using UnityEngine;
using System.Collections;

public class BeaconResponse : MonoBehaviour {
    private GameObject arrow;
	// Use this for initialization
	void Start () {
        arrow = GameObject.FindGameObjectWithTag("Arrow");
        arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        //Vector3 direction;
        if (other.tag=="Radar")
        {
            arrow.SetActive(true);
            //direction = this.transform.position - other.transform.position;
            Vector3 pos = this.transform.position;
            //Debug.DrawLine(other.transform.position, other.transform.position + direction * 1000f, Color.black, 100f);
            Vector3 sameHeightBeacon = new Vector3(pos.x, arrow.transform.position.y, pos.z);
            arrow.transform.LookAt(sameHeightBeacon);
        }
    }
    void OnTriggerExit(Collider other)
    {
        //Vector3 direction;
        if (other.tag == "Radar")
        {
            arrow.SetActive(false);
            //direction = this.transform.position - other.transform.position;
            Vector3 pos = this.transform.position;
            //Debug.DrawLine(other.transform.position, other.transform.position + direction * 1000f, Color.black, 100f);
            Vector3 sameHeightBeacon = new Vector3(pos.x, arrow.transform.position.y, pos.z);
            arrow.transform.LookAt(sameHeightBeacon);
        }
    }
}
