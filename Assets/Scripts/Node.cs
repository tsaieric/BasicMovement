using UnityEngine;
using System.Collections;

public class Node
{
	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;
	public int gCost;
	public int hCost;
	public Node parent;

	public Node (bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
	{
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public bool IsNeighbor (Node input)
	{
		if (!IsPositionEqualTo (input)) {
			int distX = Mathf.Abs (this.gridX - input.gridX);
			int distY = Mathf.Abs (this.gridY - input.gridY);
			return (distX + distY) <= 2;
		}
		return false;
	}
	public bool IsPositionEqualTo (Node input)
	{
		return (input.gridX == this.gridX) && (input.gridY == this.gridY);
	}
}
