using UnityEngine;
using System.Collections;

public class FlockingAnchor : MonoBehaviour {

    public GameObject flock;

    private MoveController[] flockObjects;
    private int numObjects;
    private Vector3 avgCurrentVelocity;
    private Vector3 avgCenterPos;
    private float radius = 5f;
	// Use this for initialization
	void Start () {
        flockObjects = flock.GetComponentsInChildren<MoveController>();
        numObjects = flockObjects.Length;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 totalCurrentVelocity = Vector3.zero;
        Vector3 totalCenterPos = Vector3.zero;
	    foreach(MoveController obj in flockObjects)
        {
            totalCurrentVelocity += obj.GetCurrentVelocity();
            totalCenterPos += obj.transform.position;
        }
        avgCurrentVelocity = totalCurrentVelocity / numObjects;
        avgCenterPos = totalCenterPos / numObjects;
        Vector3 angle = avgCurrentVelocity.normalized;
        Vector3 circlePerimeterPos = avgCenterPos + angle * radius;
        int count = 1;
        foreach (MoveController obj in flockObjects)
        {
            angle = Quaternion.Euler(0f, 360 / numObjects*count,0f) * angle;
            circlePerimeterPos = Quaternion.Euler(0, 45, 0) * circlePerimeterPos;
            obj.Arrive(circlePerimeterPos,1f); //arrive to their position
            obj.Seek(avgCenterPos); //cohesion
            obj.Alignment(avgCurrentVelocity);
            obj.UpdateEverything();
            count++;
        }
    }
}
