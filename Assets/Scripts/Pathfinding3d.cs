using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding3d : MonoBehaviour
{
	public Transform[] seekers;
	public Transform target;
	public int numPaths;
	public int weight;
	private Node3d previousTargetNode;
	private MoveController[] seekerMoveControllers;
	private Node3d currentTargetNode;
	private int seekerIndex;
	private bool calculatePaths = false;
	Grid3d grid;

	// Use this for initialization
	void Awake ()
	{
		seekerIndex = 0;
		seekerMoveControllers = new MoveController[seekers.Length];
		for (int x=0; x<seekers.Length; x++)
			seekerMoveControllers [x] = seekers [x].GetComponent<MoveController> ();
		grid = GetComponent<Grid3d> ();
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

	void FindPathPQ (Vector3 startPos, Vector3 targetPos, int x)
	{
		Node3d startNode = grid.NodeFromWorldPoint (startPos);
		Node3d targetNode = grid.NodeFromWorldPoint (targetPos);
		PriorityQueue<Node3d> openSet = new PriorityQueue<Node3d> ();
		HashSet<Node3d> closedSet = new HashSet<Node3d> ();
		openSet.Enqueue (startNode.fCost, startNode);
		while (openSet.Count > 0) {
			//get the lowest fCost in openSet, o(1)
			Node3d currentNode = openSet.Dequeue ();
			closedSet.Add (currentNode);

			if (currentNode == targetNode) {
				RetracePath (startNode, targetNode, x);
				return;
			}
			foreach (Node3d neighbor in grid.GetNeighbors(currentNode)) {
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

	void RetracePath (Node3d startNode, Node3d endNode, int x)
	{
		List<Node3d> path = new List<Node3d> ();
		Node3d currentNode = endNode;
		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse ();
		grid.paths [x] = path;
		seekerMoveControllers [x].SetPath3d (path);
	}

	int GetDistance (Node3d nodeA, Node3d nodeB)
	{
		int diffX = nodeA.gridX - nodeB.gridX;
		int diffY = nodeA.gridY - nodeB.gridY;
        int diffZ = nodeA.gridZ - nodeB.gridZ;
        return Mathf.FloorToInt(Mathf.Sqrt(diffX * diffX + diffY * diffY + diffZ * diffZ));
	}
}
