using UnityEngine;
using System.Collections;
using System;

public class AttackZombieAction : Action {

    public GameObject[] enemies;
    private GameObject targetEnemy;
    
    public AttackZombieAction()
    {
        AddPrecondition("healthLow", false);
        AddEffect("attackingZombie", true);
    }

    //addrange later
    public override bool CheckInRange()
    {
        //GameObject[] enemies = (GameObject[])UnityEngine.GameObject.FindObjectsOfType(typeof(EnemyHealth));
        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("Zombie");
        float closestDistance = 0;
        foreach(GameObject enemy in enemies2)
        {
            if(targetEnemy == null)
            {
                closestDistance = Vector3.Distance(this.transform.position, enemy.transform.position);
                targetEnemy = enemy;
            } else
            {
                float dist = Vector3.Distance(this.transform.position, enemy.transform.position);
                if(dist<closestDistance)
                {
                    targetEnemy = enemy;
                    closestDistance = dist;
                }
            }
        }
        target = targetEnemy;
        return targetEnemy != null;
    }

    public override void Reset()
    {
        targetEnemy = null;
    }

    public override bool Act()
    {
        if(inRange)
        {
            //play attackAnim;
            return true;
        } else
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
        return "attackZombieAction";
    }
}
