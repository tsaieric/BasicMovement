using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour
{
	private Quaternion camRotation;
	// Use this for initialization
	void Start ()
	{
		camRotation = Camera.main.transform.rotation;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.rotation = camRotation;
	}
}
