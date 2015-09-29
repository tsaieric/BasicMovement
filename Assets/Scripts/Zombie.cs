using UnityEngine;
using System.Collections;
public enum Behavior
{
    Seek,
    Flee,
    Arrive,
    Wander,
    FlockingWander,
    FleeFromGroup
}

public class Zombie : MonoBehaviour {
    public GameObject targetObj;
    public GameObject[] bigZombies;
    public Behavior thisBehavior;

    private Vector3 targetPosition;
    private MoveController moveController;
    private MoveController targetMoveController;
    private Animator anim;

    // Use this for initialization
    void Start () {
        moveController = this.GetComponent<MoveController>();
        anim = this.GetComponent<Animator>();
        if(targetObj !=null)
            targetMoveController = targetObj.GetComponent<MoveController>();
	}

    void FixedUpdate()
    {
        if(targetObj!=null)
            targetPosition = targetObj.transform.position;

        moveController.ResetSteering();
        if (thisBehavior == Behavior.Seek)
            moveController.Seek(targetPosition);
        if (thisBehavior == Behavior.Flee)
        {
            float distance = (this.transform.position - targetPosition).magnitude;
            if (distance <= 50f)
                moveController.Flee(targetPosition);
            else
                moveController.Wander(15f, 150f);
        }
        if (thisBehavior == Behavior.Arrive)
            moveController.Arrive(targetPosition, 20f);
        if (thisBehavior == Behavior.Wander)
            moveController.Wander(15f, 120f);
        if (thisBehavior == Behavior.FlockingWander)
        {
            float neighborRange = 5f;
            moveController.FollowLeader(targetMoveController, 1f);
            moveController.Cohesion(neighborRange);
            moveController.Alignment(neighborRange);
        }
        if(thisBehavior == Behavior.FleeFromGroup)
        {
            bool awayFromAll = true;
            foreach(GameObject zombie in bigZombies)
            {
                float distance = (this.transform.position - zombie.transform.position).magnitude;
                if (distance <= 50f)
                {
                    awayFromAll = false;
                    moveController.Flee(zombie.transform.position);
                }
            }
            if(awayFromAll)
                moveController.Wander(15f, 120f);
        }
        moveController.Separation(10f,10f);
        moveController.ObstacleAvoidance(2f, 20f);
        moveController.UpdateEverything();
        //change Zombie animation speed based on current speed
        anim.SetFloat("currentSpeed", moveController.GetCurrentVelocity().magnitude);
    }
}
