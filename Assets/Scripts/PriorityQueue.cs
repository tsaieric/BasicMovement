using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

public class PriorityQueue<T>
{
	public int Count;
	SortedDictionary<int, Stack<T>> dict;     //int is the priority, and maps to a Stack of objects
	Dictionary<T, bool> valuesAdded;

	public PriorityQueue ()
	{
		valuesAdded = new Dictionary<T, bool> ();
		Count = 0;
		dict = new SortedDictionary<int,Stack<T>> ();
	}
	//o(1) because we keep track of valuesAdded
	public bool Contains (T obj)
	{
		if (valuesAdded.ContainsKey (obj))
			return valuesAdded [obj];
		else
			return false;
	}
	public bool IsEmpty ()
	{
		return Count == 0;
	}

	//o(logn)
	public void Enqueue (int priority, T obj)
	{
		Stack<T> tempStack;
		if (dict.TryGetValue (priority, out tempStack)) {
			tempStack.Push (obj);
		} else {
			tempStack = new Stack<T> ();
			tempStack.Push (obj);
			dict.Add (priority, tempStack);
		}
		valuesAdded [obj] = true;
		Count++;
	}

	//o(logn)
	public T Dequeue ()
	{
		if (this.IsEmpty ())
			throw new UnityException ("Stack is empty");
		else {
			foreach (Stack<T> s in dict.Values) {
				if (s.Count > 0) {
					Count--;
					T obj = s.Pop ();
					valuesAdded [obj] = false;
					return obj;
				}
			}
		}
		return default(T);
	}
}
