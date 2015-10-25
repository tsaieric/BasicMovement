using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

public class PriorityQueue<T> {
    public int Count;
    SortedDictionary<int, Stack<T>> dict;     //int is the priority, and maps to a Stack of objects
    Dictionary<T, bool> valuesAdded;

    public PriorityQueue() {
        valuesAdded = new Dictionary<T, bool>();
        Count = 0;
        dict = new SortedDictionary<int,Stack<T>>();
    }

    public bool Contains(T obj)
    {
        if (valuesAdded.ContainsKey(obj))
            return valuesAdded[obj];
        else
            return false;
    }
    public bool IsEmpty()
    {
        return Count == 0;
    }

    //o(logn)
    public void Enqueue(int priority, T obj) 
    {
        if(!dict.ContainsKey(priority))
        {
            dict.Add(priority, new Stack<T>());
        }
        dict[priority].Push(obj);
        if (!valuesAdded.ContainsKey(obj))
            valuesAdded.Add(obj, true);
        else
            valuesAdded[obj] = true;
        Count++;
    }

    //should be o(1)
    public T Dequeue() {
        if (this.IsEmpty())
            throw new UnityException("Stack is empty");
        else
        {
            foreach(Stack<T> s in dict.Values)
            {
                if(s.Count>0)
                {
                    Count--;
                    valuesAdded[s.Peek()] = false;
                    return s.Pop();
                }
            }
        }
        return default(T);
    }
}
