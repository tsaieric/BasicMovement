﻿using UnityEngine;
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
		Vector3 direction = Vector3.forward.normalized;
		Vector3 currentPos;
		float interval = radius/5;                
		avgCenterPos = this.transform.position;
		Vector3 left = Quaternion.Euler(0f, -135f, 0f) * direction;
		Debug.DrawRay(avgCenterPos, left * radius, Color.black);
		Vector3 back = Quaternion.Euler(0f, -180f, 0f) * direction;
		Debug.DrawRay(avgCenterPos, back * radius, Color.black);
		Vector3 right = Quaternion.Euler(0f, 135f, 0f) * direction;
		Debug.DrawRay(avgCenterPos, right * radius, Color.black);
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
                //Vector3 direction = Vector3.forward.normalized;
                //Vector3 currentPos;
				//float interval = radius/5;                
                //avgCenterPos = this.transform.position;
				//Vector3 left = Quaternion.Euler(0f, -135f, 0f) * direction;
				//Debug.DrawRay(avgCenterPos, left * radius, Color.black);
				//Vector3 right = Quaternion.Euler(0f, 135f, 0f) * direction;
				//Debug.DrawRay(avgCenterPos, right * radius, Color.black);
				for (int i=0; i< numObjects; i++)
				{	MoveController obj = flockObjects[i];
					avgCenterPos = this.transform.position;
					if(i%2==0)
						currentPos = avgCenterPos + left * interval*(i+1);						
						
					else						
						currentPos = avgCenterPos + right * interval*i;					
                    obj.Arrive(currentPos, 10f); //arrive to their position
                    obj.Separation(5f, 10f);
                    obj.ObstacleAvoidance(2f, 20f);
                    obj.UpdateEverything();
					//avgCenterPos = currentPos;					
                }								
                break;
            case Formation.Triangle:
				/*Vector3 direction = Vector3.forward.normalized;
                Vector3 currentPos;
				float interval = radius/5;                
                avgCenterPos = this.transform.position;
				Vector3 left = Quaternion.Euler(0f, -135f, 0f) * direction;
				Debug.DrawRay(avgCenterPos, left * radius, Color.black);
				Vector3 back = Quaternion.Euler(0f, -180f, 0f) * direction;
				Debug.DrawRay(avgCenterPos, back * radius, Color.black);
				Vector3 right = Quaternion.Euler(0f, 135f, 0f) * direction;
				Debug.DrawRay(avgCenterPos, right * radius, Color.black);*/
				for (int i=0; i< numObjects; i++)
				{	MoveController obj = flockObjects[i];
					avgCenterPos = this.transform.position;
					if(i%3==0)
						currentPos = avgCenterPos + left * interval*(i+1);						
					else if(i%3==1)
						currentPos = avgCenterPos + back * interval*i;
					else						
						currentPos = avgCenterPos + right * interval*(i-1);					
                    obj.Arrive(currentPos, 10f); //arrive to their position
                    obj.Separation(5f, 10f);
                    obj.ObstacleAvoidance(2f, 20f);
                    obj.UpdateEverything();
					//avgCenterPos = currentPos;					
                }								
                break;
        } 
       
    }
}
