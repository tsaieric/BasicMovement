using UnityEngine;
using System.Collections;
public enum Behavior
{
    Seek,
    Flee,
    Arrive,
    Wander
}

public class Zombie : MonoBehaviour {
    public GameObject targetObj;
    public Behavior thisBehavior;

    private Vector3 targetPosition;
    private MoveController moveController;
    private Animator anim;

    // Use this for initialization
    void Start () {
        moveController = this.GetComponent<MoveController>();
        anim = this.GetComponent<Animator>();
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
            float fleeRange = 50f;
            float distance = (this.transform.position - targetPosition).magnitude;
            if (distance <= fleeRange)
                moveController.Flee(targetPosition);
            else
                moveController.Wander(15f, 150f);
        }
        if (thisBehavior == Behavior.Arrive)
            moveController.Arrive(targetPosition, 20f);
        if (thisBehavior == Behavior.Wander)
            moveController.Wander(15f, 150f);
        moveController.ObstacleAvoidance(2f, 20f);
        moveController.UpdateEverything();

        //change Zombie animation speed based on current speed
        anim.SetFloat("currentSpeed", moveController.GetCurrentVelocity().magnitude);
    }
}
