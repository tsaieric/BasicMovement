using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DogPlanner : MonoBehaviour
{

    private DogHealth health;
    private Queue<Action> actionList;
    private HashSet<Action> availableActions;
    private GoalPlanner planner;
    // Use this for initialization
    void Start()
    {
        planner = this.GetComponent<GoalPlanner>();
        health = this.GetComponent<DogHealth>();
        availableActions = new HashSet<Action>();
        GetActions();
    }

    public Dictionary<string, object> GetWorldState()
    {
        Dictionary<string, object> worldState = new Dictionary<string, object>();
        worldState.Add("healthLow", true);// health.GetHealth()<30f);
        return worldState;
    }

    public void PlanAttackMode()
    {
        //attack
        Dictionary<string, object> goalState = new Dictionary<string, object>();
        goalState.Add("healthLow", false);
        goalState.Add("attackingZombie", true);

        foreach (Action a in availableActions)
        {
            Debug.Log(a.Print());
        }
        actionList = planner.Plan(availableActions, GetWorldState(), goalState);
        if (actionList != null)
        {
            foreach (Action a in actionList)
            {
                Debug.Log("Action: " + a.Print());
                foreach (string key in a.effects.Keys)
                {
                    Debug.Log(a.effects[key]);
                }
            }
        }
    }

    private void GetActions()
    {
        Action[] actions = gameObject.GetComponents<Action>();
        foreach (Action a in actions)
        {
            availableActions.Add(a);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlanAttackMode();
        }
    }
}
