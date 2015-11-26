﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanNode
{
    public PlanNode parent;
    public float totalCost;
    public Dictionary<string, object> currentState;
    public Action action;
    public PlanNode(PlanNode parent, float cost, Dictionary<string, object> state, Action a)
    {
        this.parent = parent;
        totalCost = cost;
        currentState = state;
        action = a;
    }
}

public class GoalPlanner : MonoBehaviour
{

    public Queue<Action> Plan(HashSet<Action> availableActions, Dictionary<string,object> currentState, Dictionary<string,object> goalState) 
    {
        foreach(Action a in availableActions)
        {
            a.Reset();
        }
        List<PlanNode> leaves = new List<PlanNode>();
        PlanNode leastCostLeaf = null;
        PlanNode first = new PlanNode(null, 0f, currentState, null);
        bool planAvailable = BuildTree(first, availableActions, goalState, leastCostLeaf);
        if(planAvailable)
        {
            LinkedList<Action> list = new LinkedList<Action>();
            while(leastCostLeaf!=null) {
                list.AddFirst(leastCostLeaf.action);
                leastCostLeaf = leastCostLeaf.parent;
            }
            return new Queue<Action>(list);
        } else
        {
            Debug.Log("no plan available currently");
            return null;
        }
    }

    public bool BuildTree(PlanNode parent, HashSet<Action> actionList, Dictionary<string, object> goalState, List<PlanNode> leaves)
    {
        bool hasPath = false;
        foreach (Action a in actionList)
        {
            if (AreConditionsInState(a.preconditions, parent.currentState))
            {
                Dictionary<string, object> newState = CreateStateWithEffects(a.effects, parent.currentState);
                PlanNode newNode = new PlanNode(parent, parent.totalCost + a.cost, newState, a);
                if (AreConditionsInState(goalState, newState))
                {
                    leaves.Add(newNode);
                    hasPath = true;
                }
                else
                {
                    HashSet<Action> actionsMinusOne = new HashSet<Action>(actionList);
                    actionsMinusOne.Remove(a);
                    BuildTree(newNode, actionsMinusOne, goalState, leaves);
                }
            }
        }
        return hasPath;
    }

    //build tree and track the leastcostleaf node;
    public bool BuildTree(PlanNode parent, HashSet<Action> actionList, Dictionary<string, object> goalState, PlanNode leastCostLeaf)
    {
        bool hasPath = false;
        foreach (Action a in actionList)
        {
            if (AreConditionsInState(a.preconditions, parent.currentState))
            {
                Dictionary<string, object> newState = CreateStateWithEffects(a.effects, parent.currentState);
                PlanNode newNode = new PlanNode(parent, parent.totalCost + a.cost, newState, a);
                if (AreConditionsInState(goalState, newState))
                {
                    if (leastCostLeaf == null)
                    {
                        leastCostLeaf = newNode;
                    }
                    else if (newNode.totalCost < leastCostLeaf.totalCost)
                    {
                        leastCostLeaf = newNode;
                    }
                    hasPath = true;
                }
                else
                {
                    //remove the current action from list of available actions and build tree based on remaining actions
                    HashSet<Action> actionsMinusOne = new HashSet<Action>(actionList);
                    actionsMinusOne.Remove(a);
                    //if any path was found, set to true
                    hasPath = hasPath || BuildTree(newNode, actionsMinusOne, goalState, leastCostLeaf);
                }
            }
        }
        return hasPath;
    }

    //create a new state with the effects added in to the state
    private Dictionary<string, object> CreateStateWithEffects(Dictionary<string, object> effects, Dictionary<string, object> state)
    {
        Dictionary<string, object> result = new Dictionary<string, object>(state);
        foreach (string key in effects.Keys)
        {
            result.Add(key, effects[key]);
        }
        return result;
    }

    //check if conditions are a subset of the current state
    private bool AreConditionsInState(Dictionary<string, object> conditions, Dictionary<string, object> state)
    {
        bool inState = true;
        foreach (string key in conditions.Keys)
        {
            if (!state.ContainsKey(key))
            {
                return false;
            }
            else
            {
                if (!state[key].Equals(conditions[key]))
                {
                    return false;
                }
            }
        }
        return inState;
    }
}
