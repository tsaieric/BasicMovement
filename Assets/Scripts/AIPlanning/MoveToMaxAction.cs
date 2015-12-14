using UnityEngine;
using System.Collections;
using System;

public class MoveToMaxAction : Action
{
    private GameObject targetPlayer;
    private DogMovement movement;

    public void Start()
    {
        movement = this.GetComponent<DogMovement>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    public MoveToMaxAction()
    {
        AddEffect("nearMax", true);
    }

    //addrange later
    public override bool CheckInRange()
    {
        return true;
    }

    public override void Reset()
    {
        target = targetPlayer;
    }

    public override bool Act()
    {
        if (targetPlayer == null)
        {
            return true;
        }
        float distance = Vector3.Distance(this.transform.position, targetPlayer.transform.position);
        if (distance <= 10f)
        {
            return true;
        }
        else
        {
            movement.targetObj = targetPlayer;
            movement.thisBehavior = DogBehavior.SeekAStar;
            //MoveController seeks target
            return false;
        }
    }

    public override bool isDone()
    {
        throw new NotImplementedException();
    }

    public override bool RequiresInRange()
    {
        return false;
    }

    public override string Print()
    {
        return "moveToMax";
    }
}
