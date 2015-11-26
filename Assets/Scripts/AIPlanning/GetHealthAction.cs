using UnityEngine;
using System.Collections;
using System;

public class GetHealthAction : Action
{
    private GameObject targetFood;

    public GetHealthAction()
    {
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
        target = targetFood;
        return targetFood != null;
    }

    public override void Reset()
    {
        targetFood = null;
    }

    public override bool Act()
    {
        if (inRange)
        {
            return true;
        }
        else
        {
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
