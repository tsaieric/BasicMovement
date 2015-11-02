using UnityEngine;
using System.Collections;

public class TargetHop : MonoBehaviour
{
	public float changePosDelay = 3.5f;
	public Vector3[]
		positions = new Vector3[4];
	// Use this for initialization
	void Start ()
	{
		StartCoroutine (ChangePosition ());
	}

    public IEnumerator ChangePosition ()
	{
		yield return new WaitForSeconds (changePosDelay);
		int index = Random.Range (0, positions.Length - 1);
		this.transform.position = positions [index];
		StartCoroutine (ChangePosition ());
	}
}
