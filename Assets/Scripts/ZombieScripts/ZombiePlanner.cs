using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ZombiePlanner : MonoBehaviour
{
	private GameObject player;
	private EnemyHealth health;
	private Queue<Action> actionList;
	private HashSet<Action> availableActions;
	private GoalPlanner planner;
	private float lowHealthThreshold = 50f;
	private float distFromMaxThreshold = 40f;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		planner = this.GetComponent<GoalPlanner> ();
		health = this.GetComponent<EnemyHealth> ();
		availableActions = new HashSet<Action> ();
		GetActions ();
	}

	public Dictionary<string, object> GetWorldState ()
	{
		Dictionary<string, object> worldState = new Dictionary<string, object> ();
		worldState.Add ("healthLow", health.GetHealth () < lowHealthThreshold);
		worldState.Add ("nearMax", Vector3.Distance (this.transform.position, player.transform.position) < distFromMaxThreshold);
		return worldState;
	}

	public void PlanAttackMode ()
	{
		//attack
		Dictionary<string, object> goalState = new Dictionary<string, object> ();
		goalState.Add ("healthLow", false);
		goalState.Add ("attackingPlayer", true);
		actionList = planner.Plan (availableActions, GetWorldState (), goalState);
		//if (actionList != null)
		//{
		//    Debug.Log("ACTIONLIST IS: ");
		//    foreach (Action a in actionList)
		//    {
		//        Debug.Log("Action: " + a.Print());
		//    }
		//}
	}

	private void GetActions ()
	{
		Action[] actions = gameObject.GetComponents<Action> ();
		foreach (Action a in actions) {
			availableActions.Add (a);
		}
	}
	// Update is called once per frame
	void Update ()
	{
		PlanAttackMode ();
		if (actionList != null) {
			if (actionList.Count > 0) {
				Action a = actionList.Peek ();
				//if acts successfully, pop off
				if (a.Act ()) {
					actionList.Dequeue ();
				}
			}
		}
	}
}
