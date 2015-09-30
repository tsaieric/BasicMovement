using UnityEngine;
using System.Collections;

public enum Formation
{
    Circle,
	Vshape,
    Triangle
}
public class FlockingAnchor : MonoBehaviour {

    public GameObject flock;
    public Formation form;
    private MoveController[] flockObjects;
    private int numObjects;
    //private Vector3 avgCurrentVelocity;
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
        //avgCurrentVelocity = totalCurrentVelocity / numObjects;
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
			case Formation.Vshape:
                Vector3 front = Vector3.forward.normalized;
                Vector3 currentPos;
				float interval = radius/4;
                //int count = 0;
                //float rotateBy = 360 / numObjects;
                avgCenterPos = this.transform.position;
				for (int i=0; i<= numObjects/2-1; i++)
				{	MoveController obj = flockObjects[i];
                    Vector3 left = Quaternion.Euler(0f, -135f, 0f) * front;
                    Debug.DrawRay(avgCenterPos, left * radius, Color.black);
                    currentPos = avgCenterPos + left * interval;
                    obj.Arrive(currentPos, 10f); //arrive to their position
                    obj.Separation(5f, 10f);
                    obj.ObstacleAvoidance(2f, 20f);
                    obj.UpdateEverything();
					avgCenterPos = currentPos;
                }
				avgCenterPos = this.transform.position;
				for (int i=numObjects/2; i<numObjects; i++)
				{	MoveController obj = flockObjects[i];
                    Vector3 right = Quaternion.Euler(0f, 135f, 0f) * front;
                    Debug.DrawRay(avgCenterPos, right * radius, Color.black);
                    currentPos = avgCenterPos + right * interval;
                    obj.Arrive(currentPos, 10f); //arrive to their position
                    obj.Separation(5f, 10f);
                    obj.ObstacleAvoidance(2f, 20f);
                    obj.UpdateEverything();
					avgCenterPos = currentPos;
                }
                break;
            case Formation.Triangle:
                break;
        } 
       
    }
}
