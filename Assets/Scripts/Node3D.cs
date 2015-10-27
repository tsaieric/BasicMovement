﻿using UnityEngine;
using System.Collections;

public class Node3d
{
	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;
    public int gridZ;
	public int gCost;
	public int hCost;
	public Node3d parent;

	public Node3d (bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _gridZ)
	{
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
        gridZ = _gridZ;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public bool IsNeighbor (Node3d input)
	{
		if (!IsPositionEqualTo (input)) {
			int distX = Mathf.Abs (this.gridX - input.gridX);
			int distY = Mathf.Abs (this.gridY - input.gridY);
            int distZ = Mathf.Abs(this.gridZ - input.gridZ);
			return (distX + distY+distZ) <= 3;
		}
		return false;
	}
	public bool IsPositionEqualTo (Node3d input)
	{
		return (input.gridX == this.gridX) && (input.gridY == this.gridY) && (input.gridZ == this.gridZ);
	}
}
