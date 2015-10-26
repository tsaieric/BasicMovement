﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
	public Transform[] seekers;
	public Transform target;
	public int numPaths;
	public int weight;
	public bool calculatePerFrame;
	public bool usePQ;
	private Node previousTargetNode;
	private Node currentTargetNode;
	Grid grid;

	// Use this for initialization
	void Awake ()
	{
		grid = GetComponent<Grid> ();
	}

	void Start ()
	{
		CalculateAllPaths ();
	}
	void Update ()
	{
		if (!calculatePerFrame) {
			currentTargetNode = grid.NodeFromWorldPoint (target.position);
			if (previousTargetNode != null) {
				//if current node != previous node, then calculate all paths
				if (!currentTargetNode.IsPositionEqualTo (previousTargetNode)) {
					CalculateAllPaths ();
				}
			}
			previousTargetNode = currentTargetNode;
		} else
			CalculateAllPaths ();
	}

	void CalculateAllPaths ()
	{
		for (int x = 0; x < numPaths; x++) {
			if (seekers [x] != null) {
				if (usePQ)
					FindPathPQ (seekers [x].position, target.position, x);
				else
					FindPathList (seekers [x].position, target.position, x);
			}
		}
	}

	void FindPathList (Vector3 startPos, Vector3 targetPos, int num)
	{
		Node startNode = grid.NodeFromWorldPoint (startPos);
		Node targetNode = grid.NodeFromWorldPoint (targetPos);

		List<Node> openSet = new List<Node> ();
		HashSet<Node> closedSet = new HashSet<Node> ();
		openSet.Add (startNode);

		while (openSet.Count > 0) {
			Node currentNode = openSet [0];
			//get the lowest fCost in openSet
			//this is o(n) time
			for (int i = 1; i < openSet.Count; i++) {
				if (openSet [i].fCost < currentNode.fCost || openSet [i].fCost == currentNode.fCost && openSet [i].hCost < currentNode.hCost) {
					currentNode = openSet [i];
				}
			}
			//list removal is o(n), o(2n) vs. o(1) with PQ
			openSet.Remove (currentNode);

			closedSet.Add (currentNode);

			if (currentNode == targetNode) {
				RetracePath (startNode, targetNode, num);
				return;
			}

			foreach (Node neighbor in grid.GetNeighbors(currentNode)) {
				//if not walkable or visited already, then skip
				if (!neighbor.walkable || closedSet.Contains (neighbor)) {
					continue;
				}

				int newMovementCostToNeighbor = currentNode.gCost + GetDistance (currentNode, neighbor);
				//list contains is o(n)
				if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains (neighbor)) {
					neighbor.gCost = newMovementCostToNeighbor;
					neighbor.hCost = GetDistance (neighbor, targetNode);
					neighbor.parent = currentNode;
            
					if (!openSet.Contains (neighbor))
						openSet.Add (neighbor);
				}
			}
		}
	}

	void FindPathPQ (Vector3 startPos, Vector3 targetPos, int num)
	{
		Node startNode = grid.NodeFromWorldPoint (startPos);
		Node targetNode = grid.NodeFromWorldPoint (targetPos);
		PriorityQueue<Node> openSet = new PriorityQueue<Node> ();
		HashSet<Node> closedSet = new HashSet<Node> ();
		openSet.Enqueue (startNode.fCost, startNode);
		while (openSet.Count > 0) {
			//get the lowest fCost in openSet, 
			//o(1)
			Node currentNode = openSet.Dequeue ();

			closedSet.Add (currentNode);

			if (currentNode == targetNode) {
				RetracePath (startNode, targetNode, num);
				return;
			}

			foreach (Node neighbor in grid.GetNeighbors(currentNode)) {
				if (!neighbor.walkable || closedSet.Contains (neighbor)) {
					continue;
				}

				int newMovementCostToNeighbor = currentNode.gCost + GetDistance (currentNode, neighbor);
				//contains is constant because custom PQ uses another Dict<node,bool> to track values added
				if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains (neighbor)) {
					neighbor.gCost = newMovementCostToNeighbor;
					neighbor.hCost = GetDistance (neighbor, targetNode);
					neighbor.parent = currentNode;

					if (!openSet.Contains (neighbor)) {
						openSet.Enqueue (neighbor.fCost, neighbor);
					}
				}
			}
		}
	}

	void RetracePath (Node startNode, Node endNode, int x)
	{
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;
		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse ();
		grid.paths [x] = path;
        Debug.Log("path has been retraced??");
		seekers [x].GetComponent<Zombie> ().SetPath (path);
	}

	int GetDistance (Node nodeA, Node nodeB)
	{
		int distX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs (nodeA.gridY - nodeB.gridY);
		return weight * (distY + distX);
		//if (distX > distY)
		//    return weight*(14 * distY + 10 * (distX - distY));
		//return weight * (14 * distX + 10 * (distY - distX));
	}
}
