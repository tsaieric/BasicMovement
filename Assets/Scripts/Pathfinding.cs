using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
	public Transform[] seekers;
	public Transform target;
	public int numPaths;
	public int weight;
	private Node previousTargetNode;
	private MoveController[] seekerMoveControllers;
	private Node currentTargetNode;
	private int seekerIndex;
	private bool calculatePaths = false;
	Grid grid;

	// Use this for initialization
	void Awake ()
	{
		seekerIndex = 0;
		seekerMoveControllers = new MoveController[seekers.Length];
		for (int x=0; x<seekers.Length; x++)
			seekerMoveControllers [x] = seekers [x].GetComponent<MoveController> ();
		grid = GetComponent<Grid> ();
	}

	void Start ()
	{
		CalculateAllPaths ();
	}

	void Update ()
	{
		if (calculatePaths) {
			CalcOnePathPerFrame ();
		}
		currentTargetNode = grid.NodeFromWorldPoint (target.position);
		if (previousTargetNode != null) {
			//if current target node != previous target node, then calculate all paths
			if (!currentTargetNode.IsPositionEqualTo (previousTargetNode) && currentTargetNode.walkable) {
				calculatePaths = true;
				//CalculateAllPaths ();
			}
//			if (!currentTargetNode.IsNeighbor (previousTargetNode))
//				previousTargetNode = currentTargetNode;
		} 
//		else {
		previousTargetNode = currentTargetNode;
//		}
	}

	void CalcOnePathPerFrame ()
	{
		if (seekerIndex < numPaths) {
			FindPathPQ (seekers [seekerIndex].position, target.position, seekerIndex);
			seekerIndex++;
		}
		if (seekerIndex == numPaths) {
			seekerIndex = 0;
			calculatePaths = false;
		}
	}

	void CalcXPathsPerFrame (int x)
	{
		if (seekerIndex < numPaths * x) {
			if (seekerIndex % x == 0)
				FindPathPQ (seekers [seekerIndex / x].position, target.position, seekerIndex / x);
			seekerIndex++;
		}
		if (seekerIndex == numPaths * x && seekerIndex % x == 0) {
			seekerIndex = 0;
			calculatePaths = false;
		}
	}

	void CalculateAllPaths ()
	{
		for (int x = 0; x < numPaths; x++) {
			if (seekers [x] != null) {
				FindPathPQ (seekers [x].position, target.position, x);
			}
		}
	}

//	void FindPathList (Vector3 startPos, Vector3 targetPos, int num)
//	{
//		Node startNode = grid.NodeFromWorldPoint (startPos);
//		Node targetNode = grid.NodeFromWorldPoint (targetPos);
//		List<Node> openSet = new List<Node> ();
//		HashSet<Node> closedSet = new HashSet<Node> ();
//		openSet.Add (startNode);
//
//		while (openSet.Count > 0) {
//			Node currentNode = openSet [0];
//			//get the lowest fCost in openSet
//			//this is o(n) time
//			for (int i = 1; i < openSet.Count; i++) {
//				if (openSet [i].fCost < currentNode.fCost || openSet [i].fCost == currentNode.fCost && openSet [i].hCost < currentNode.hCost) {
//					currentNode = openSet [i];
//				}
//			}
//			//list removal is o(n), o(2n) vs. o(1) with PQ
//			openSet.Remove (currentNode);
//
//			closedSet.Add (currentNode);
//
//			if (currentNode == targetNode) {
//				RetracePath (startNode, targetNode, num);
//				return;
//			}
//
//			foreach (Node neighbor in grid.GetNeighbors(currentNode)) {
//				//if not walkable or visited already, then skip
//				if (!neighbor.walkable || closedSet.Contains (neighbor)) {
//					continue;
//				}
//
//				int newMovementCostToNeighbor = currentNode.gCost + GetDistance (currentNode, neighbor);
//				//list contains is o(n)
//				if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains (neighbor)) {
//					neighbor.gCost = newMovementCostToNeighbor;
//					neighbor.hCost = GetDistance (neighbor, targetNode);
//					neighbor.parent = currentNode;
//
//					if (!openSet.Contains (neighbor))
//						openSet.Add (neighbor);
//				}
//			}
//		}
//	}

	void FindPathPQ (Vector3 startPos, Vector3 targetPos, int x)
	{
		Node startNode = grid.NodeFromWorldPoint (startPos);
		Node targetNode = grid.NodeFromWorldPoint (targetPos);
		PriorityQueue<Node> openSet = new PriorityQueue<Node> ();
		HashSet<Node> closedSet = new HashSet<Node> ();
		openSet.Enqueue (startNode.fCost, startNode);
		while (openSet.Count > 0) {
			//get the lowest fCost in openSet, o(1)
			Node currentNode = openSet.Dequeue ();
			closedSet.Add (currentNode);

			if (currentNode == targetNode) {
				RetracePath (startNode, targetNode, x);
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
		seekerMoveControllers [x].SetPath (path);
	}

	int GetDistance (Node nodeA, Node nodeB)
	{
		int distX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs (nodeA.gridY - nodeB.gridY);
		//calculate the octile distance
		if (distX > distY)
			return weight * (14 * distY + 10 * (distX - distY)); //14 for diagonal movement cost (14 from sqrt(200)), 10 for vertical or horizontal movement cost 
		return weight * (14 * distX + 10 * (distY - distX));
	}
}
