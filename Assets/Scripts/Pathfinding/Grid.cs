using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid: MonoBehaviour
{
	public static Grid Instance;
	private Transform player;
	public LayerMask unwalkableMask;
	public LayerMask fireMask;
	public Vector2 gridSize;
	public float nodeRadius;
	public bool drawPath;
	Node[,] grid;
	float nodeDiameter;
	int gridX, gridY;

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		nodeDiameter = nodeRadius * 2;
		gridX = Mathf.RoundToInt (gridSize.x / nodeDiameter);
		gridY = Mathf.RoundToInt (gridSize.y / nodeDiameter);
		CreateGrid ();
	}

	void CreateGrid ()
	{
		grid = new Node[gridX, gridY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;
		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere (worldPoint, nodeRadius, unwalkableMask));				
				bool isFire = (Physics.CheckSphere (worldPoint, nodeRadius, fireMask));
				grid [x, y] = new Node (walkable, isFire, worldPoint, x, y);
			}
		}
	}

	public List<Node> GetNeighbors (Node node)
	{
		List<Node> neighbors = new List<Node> ();
		for (int x = -1; x<=1; x++) {
			for (int y = -1; y<= 1; y++) {
				//check if it's origin
				if (x == 0 && y == 0)
					continue;
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;
				if (checkX >= 0 && checkX < gridX && checkY >= 0 && checkY < gridY) {
					neighbors.Add (grid [checkX, checkY]);
				}
			}
		}
		return neighbors;
	}

	public Node NodeFromWorldPoint (Vector3 worldPosition)
	{
		float percentX = worldPosition.x / gridSize.x + .5f;//this is the simplification of (worldPosition.x + gridSize.x / 2) / gridSize.x;
		float percentY = worldPosition.z / gridSize.y + .5f;
		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		int x = Mathf.RoundToInt ((gridX - 1) * percentX);
		int y = Mathf.RoundToInt ((gridY - 1) * percentY);
		return grid [x, y];
	}

	public List<Node>[] paths = new List<Node>[10];

	void OnDrawGizmos ()
	{
		if (drawPath) {
			Gizmos.DrawWireCube (transform.position, new Vector3 (gridSize.x, 1, gridSize.y));
			if (grid != null) {
				foreach (Node n in grid) {
					if (!n.walkable) 
						Gizmos.color = Color.cyan;
					else
						Gizmos.color = Color.white;
					if (n.isFire)
						Gizmos.color = Color.red;

					for (int x=0; x<paths.Length; x++) {
						if (paths [x] != null)
						if (paths [x].Contains (n))
							Gizmos.color = Color.black;
					}
					if (Gizmos.color != Color.white)
						Gizmos.DrawCube (n.worldPosition, Vector3.one * (nodeDiameter - .1f));
				}
			}
		}
	}
}
