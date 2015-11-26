using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Action : MonoBehaviour
{
    public Dictionary<string, object> preconditions;
    public Dictionary<string, object> effects;
    //Attack Mode -> Goal States (kill zombie and stay alive and nearMax)
    //Defend Mode -> Goal States (Max is alive and zombie is dead)
    //StayClose Mode -> Goal States (near Max)
    //FindFood -> Goal State (dog is near food)

    //GetHealth, SmellFood
    //AttackZombie, Preconditions: ZombieNearby, Effect: Zombie loses health, Dog loses health
    //RunAway
    //Threaten
    //RunToMax

    public float cost;

    public Action()
    {
        preconditions = new Dictionary<string, object>();
        effects = new Dictionary<string, object>();
    }

    public void AddPrecondition(string key, object value)
    {
        preconditions.Add(key, value);
    }

    public void RemovePrecondition(string key)
    {
        preconditions.Remove(key);
    }

    public void AddEffect(string key, object value)
    {
        effects.Add(key, value);
    }

    public void RemoveEffect(string key)
    {
        effects.Remove(key);
    }

    public virtual void Reset() { }
}
