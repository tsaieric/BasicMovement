using UnityEngine;
using System.Collections;

public class PatrolArea : MonoBehaviour
{
	public Transform destA;
	public float timeToDest;
	private Vector3 origPos;
	// Use this for initialization
	void Start ()
	{
		origPos = this.transform.position;
		StartCoroutine (MoveTo (origPos, destA.position, timeToDest));
	}

	IEnumerator MoveTo (Vector3 startPos, Vector3 desiredPos, float time)
	{
		float distance = Mathf.Abs (transform.position.magnitude - desiredPos.magnitude);
		float moveSpeed = distance / time;
		while (transform.position!=desiredPos) {
			transform.position = Vector3.MoveTowards (transform.position, desiredPos, moveSpeed * Time.deltaTime);
			yield return null;
		}
		StartCoroutine (MoveTo (desiredPos, startPos, time));
		yield return null;
	}
}
