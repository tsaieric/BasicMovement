using UnityEngine;
using System.Collections;

public enum Formation
{
    Circle,
    Triangle
}
public class FlockingAnchor : MonoBehaviour {

    public GameObject flock;
    public Formation form;
    private MoveController[] flockObjects;
    private int numObjects;
    private Vector3 avgCurrentVelocity;
    private Vector3 avgCenterPos;
    private float radius = 15f;
    private MoveController movement;
	// Use this for initialization
	void Start () {
        flockObjects = flock.GetComponentsInChildren<MoveController>();
        numObjects = flockObjects.Length;
        this.movement = this.GetComponent<MoveController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        this.movement.Wander(15f,120f);
        this.movement.ObstacleAvoidance(radius/2, 30f);
        this.movement.UpdateEverything();
        Vector3 totalCurrentVelocity = Vector3.zero;
        Vector3 totalCenterPos = Vector3.zero;
	    foreach(MoveController obj in flockObjects)
        {
            totalCurrentVelocity += obj.GetCurrentVelocity();
            totalCenterPos += obj.transform.position;
        }
        avgCurrentVelocity = totalCurrentVelocity / numObjects;
        avgCenterPos = totalCenterPos / numObjects;

        switch(this.form)
        {
            case Formation.Circle:
                Vector3 angle = Vector3.forward.normalized;
                Vector3 circlePerimeterPos;
                int count = 0;
                float rotateBy = 360 / numObjects;
                avgCenterPos = this.transform.position;
                foreach (MoveController obj in flockObjects)
                {
                    angle = Quaternion.Euler(0f, rotateBy, 0f) * angle;
                    Debug.DrawRay(avgCenterPos, angle * radius, Color.black);
                    circlePerimeterPos = avgCenterPos + angle * radius;
                    obj.Arrive(circlePerimeterPos, 10f); //arrive to their position
                    obj.Separation(5f, 10f);
                    obj.ObstacleAvoidance(2f, 20f);
                    obj.UpdateEverything();
                    count++;
                }
                break;
            case Formation.Triangle:
                break;
        } 
       
    }
}
