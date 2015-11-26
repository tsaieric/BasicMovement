using UnityEngine;
using System.Collections;
using System;

public class AttackZombieAction : Action
{

    public GameObject[] enemies;
    private GameObject targetEnemy;
    private DogMovement movement;
    private Animator anim;
    private EnemyHealth enemyHealth;
    void Start()
    {
        anim = this.GetComponent<Animator>();
        movement = this.GetComponent<DogMovement>();
    }

    public AttackZombieAction()
    {
        AddPrecondition("nearMax", true);
        AddPrecondition("healthLow", false);
        AddEffect("attackingZombie", true);
    }

    //addrange later
    public override bool CheckInRange()
    {
        //GameObject[] enemies = (GameObject[])UnityEngine.GameObject.FindObjectsOfType(typeof(EnemyHealth));
        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("Zombie");
        float closestDistance = 0;
        foreach (GameObject enemy in enemies2)
        {
            if (targetEnemy == null)
            {
                closestDistance = Vector3.Distance(this.transform.position, enemy.transform.position);
                targetEnemy = enemy;
            }
            else
            {
                float dist = Vector3.Distance(this.transform.position, enemy.transform.position);
                if (dist < closestDistance)
                {
                    targetEnemy = enemy;
                    closestDistance = dist;
                }
            }
        }
        if (closestDistance > 40f)
        {
            targetEnemy = null;
        }
        target = targetEnemy;
        return targetEnemy != null;
    }

    public override void Reset()
    {
        targetEnemy = null;
        target = null;
    }

    public override bool Act()
    {
        if (targetEnemy == null)
        {
            return true;
        }
        float distance = Vector3.Distance(this.transform.position, targetEnemy.transform.position);
        if (distance <= 5f)
        {
            if (!anim.GetBool("IsAttacking"))
            {
                anim.SetBool("IsAttacking", true);
            }
            if (anim.GetBool("IsWalking"))
            {
                anim.SetBool("IsWalking", false);
            }
            Debug.Log(anim.playbackTime);
            return true;
        }
        else
        {
            anim.SetBool("IsWalking", true);
            anim.SetBool("IsAttacking", false);
            movement.targetObj = targetEnemy;
            movement.thisBehavior = DogBehavior.Arrive;
            //MoveController seeks target
            return false;
        }
    }

    public void Attack()
    {
        if (targetEnemy != null)
        {
            enemyHealth = targetEnemy.GetComponent<EnemyHealth>();
            enemyHealth.ReduceHealth(10f);
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
