using UnityEngine;
using System.Collections;
using System;

public class GetHealthAction : Action
{
    private GameObject targetFood;
    private DogMovement movement;

    public void Start()
    {
        movement = this.GetComponent<DogMovement>();
    }

    public GetHealthAction()
    {
        AddPrecondition("nearMax", true);
        AddEffect("healthLow", false);
    }

    //addrange later
    public override bool CheckInRange()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("HealthPack");
        float closestDistance = 0;
        foreach (GameObject food in foods)
        {
            if (targetFood == null)
            {
                closestDistance = Vector3.Distance(this.transform.position, food.transform.position);
                targetFood = food;
            }
            else
            {
                float dist = Vector3.Distance(this.transform.position, food.transform.position);
                if (dist < closestDistance)
                {
                    targetFood = food;
                    closestDistance = dist;
                }
            }
        }
        if(closestDistance>50f)
        {
            targetFood = null;
        }
        target = targetFood;
        return targetFood != null;
    }

    public override void Reset()
    {
        targetFood = null;
        target = null;
    }

    public override bool Act()
    {
        if(targetFood == null)
        {
            return true;
        }
        float distance = Vector3.Distance(this.transform.position, targetFood.transform.position);
        if (distance<=.1f)
        {
            return true;
        }
        else
        {
            movement.targetObj = targetFood;
            movement.thisBehavior = DogBehavior.Arrive;
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
        return true;
    }

    public override string Print()
    {
        return "getHealthAction";
    }
}
