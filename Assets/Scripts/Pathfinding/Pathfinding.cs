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
    void Awake()
    {
        seekerIndex = 0;
        seekerMoveControllers = new MoveController[seekers.Length];
        for (int x = 0; x < seekers.Length; x++)
            seekerMoveControllers[x] = seekers[x].GetComponent<MoveController>();
        grid = GetComponent<Grid>();
    }

    void Start()
    {
        calculatePaths = true;
    }

    void Update()
    {
        if (calculatePaths)
        {
            CalcOnePathPerFrame();
        }
        currentTargetNode = grid.NodeFromWorldPoint(target.position);
        if (previousTargetNode != null)
        {
            //if current target node != previous target node, then calculate all paths
            if (!currentTargetNode.IsPositionEqualTo(previousTargetNode) && currentTargetNode.walkable)
            {
                calculatePaths = true;
            }
        }
        previousTargetNode = currentTargetNode;
    }

    //used to calculate a path per frame instead of all paths in one frame
    void CalcOnePathPerFrame()
    {
        if (seekerIndex < numPaths)
        {
            if (seekers[seekerIndex] != null)
            {
                if (seekerIndex != seekers.Length - 1)
                {
                    if (seekers[seekerIndex].GetComponent<ZombieMovement>().thisBehavior == Behavior.SeekAStar)
                    {
                        FindPathPQ(seekers[seekerIndex].position, target.position, seekerIndex);
                    }
                } else
                {
                    if (seekers[seekerIndex].GetComponent<DogMovement>().thisBehavior == DogBehavior.SeekAStar)
                    {
                        FindPathPQ(seekers[seekerIndex].position, target.position, seekerIndex);
                    }
                }
            }
            seekerIndex++;
        }
        if (seekerIndex == numPaths)
        {
            seekerIndex = 0;
            calculatePaths = false;
        }
    }

    //not being used
    void CalcPathPerXFrame(int x)
    {
        if (seekerIndex < numPaths * x)
        {
            if (seekerIndex % x == 0)
                FindPathPQ(seekers[seekerIndex / x].position, target.position, seekerIndex / x);
            seekerIndex++;
        }
        if (seekerIndex == numPaths * x && seekerIndex % x == 0)
        {
            seekerIndex = 0;
            calculatePaths = false;
        }
    }

    void CalculateAllPaths()
    {
        for (int x = 0; x < numPaths; x++)
        {
            if (seekers[x] != null)
            {
                FindPathPQ(seekers[x].position, target.position, x);
            }
        }
    }

    void FindPathPQ(Vector3 startPos, Vector3 targetPos, int x)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
        PriorityQueue<Node> openSet = new PriorityQueue<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Enqueue(startNode.fCost, startNode);
        while (openSet.Count > 0)
        {
            //get the lowest fCost in openSet, o(logn)
            Node currentNode = openSet.Dequeue();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode, x);
                return;
            }
            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }
                //calculate new cost it takes to get to neighbor
                int newMovementCostToNeighbor = currentNode.gCost + GetNeighborCost(currentNode, neighbor, x);

                //update if new path to neighbor is less costly, or neighbor hasn't been considered
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor.fCost, neighbor);
                    }
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
        seekerMoveControllers[x].SetPath(path);
    }

    //checks for distance and accounts for terrain/type costs
    int GetNeighborCost(Node current, Node neighbor, int x)
    {
        int cost = GetDistance(current, neighbor);
        if (neighbor.isFire)
        {
            //movement cost of 8, scaled by 10 to match
            if (!seekerMoveControllers[x].canWalkOnFire)
                cost += 80 * weight;
        }
        return cost;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        //calculate the octile distance
        if (distX > distY)
            return weight * (14 * distY + 10 * (distX - distY)); //14 for diagonal movement cost (14 from sqrt(200)), 10 for vertical or horizontal movement cost 
        return weight * (14 * distX + 10 * (distY - distX));
    }
}
