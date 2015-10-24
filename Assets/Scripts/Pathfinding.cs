﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public Transform[] seekers;
    public Transform target;
    public int weight;
    public bool calculatePerFrame;
    private Node previousTargetNode;
    private Node currentTargetNode;
    Grid grid;

    // Use this for initialization
    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        if (!calculatePerFrame)
        {
            currentTargetNode = grid.NodeFromWorldPoint(target.position);
            if (previousTargetNode != null)
            {
                if (!currentTargetNode.IsPositionEqualTo(previousTargetNode))
                {
                    CalculateAllPaths();
                }
            }
            previousTargetNode = currentTargetNode;
        }
        else
            CalculateAllPaths();
    }

    void CalculateAllPaths()
    {
        //Debug.Log("calculating paths");
        for (int x = 0; x < seekers.Length; x++)
        {
            if (seekers[x] != null)
                FindPath(seekers[x].position, target.position, x);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos, int num)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode, num);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode, int x)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.paths[x] = path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        return weight * (distY + distX);
        //if (distX > distY)
        //    return weight*(14 * distY + 10 * (distX - distY));
        //return weight * (14 * distX + 10 * (distY - distX));
    }
}
