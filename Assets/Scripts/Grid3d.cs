using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid3d : MonoBehaviour
{
    public static Grid3d Instance;
    private Transform player;
    public LayerMask unwalkableMask;
    public Vector3 gridSize;
    public float nodeRadius;
    public bool drawPath;
    Node3d[,,] grid;
    float nodeDiameter;
    int gridX, gridY, gridZ;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
        gridZ = Mathf.RoundToInt(gridSize.z / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node3d[gridX, gridY, gridZ];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2 - Vector3.up * gridSize.z / 2;
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                for (int z = 0; z < gridZ; z++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius) + Vector3.up * (z * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                    grid[x, y, z] = new Node3d(walkable, worldPoint, x, y, z);
                }
            }
        }
    }

    public List<Node3d> GetNeighbors(Node3d node)
    {
        List<Node3d> neighbors = new List<Node3d>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    //check if it's origin
                    if (x == 0 && y == 0)
                        continue;
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    int checkZ = node.gridZ + z;
                    if (checkX >= 0 && checkX < gridX && checkY >= 0 && checkY < gridY && checkZ >= 0 && checkZ < gridZ)
                    {
                        neighbors.Add(grid[checkX, checkY, checkZ]);
                    }
                }
            }
        }
        return neighbors;
    }

    public Node3d NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = worldPosition.x / gridSize.x + .5f;//this is the simplification of (worldPosition.x + gridSize.x / 2) / gridSize.x;
        float percentY = worldPosition.z / gridSize.y + .5f;
        float percentZ = worldPosition.y / gridSize.z + .5f;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        percentZ = Mathf.Clamp01(percentZ);
        int x = Mathf.RoundToInt((gridX - 1) * percentX);
        int y = Mathf.RoundToInt((gridY - 1) * percentY);
        int z = Mathf.RoundToInt((gridZ - 1) * percentZ);
        return grid[x, y, z];
    }

    public List<Node3d>[] paths = new List<Node3d>[10];

    void OnDrawGizmos()
    {
        if (drawPath)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, gridSize.z, gridSize.y));
            if (grid != null)
            {
                //Node playerNode = NodeFromWorldPoint(player.position);
                foreach (Node3d n in grid)
                {
                    if (n.walkable)
                        Gizmos.color = Color.white;
                    else
                        Gizmos.color = Color.red;
                    for (int x = 0; x < paths.Length; x++)
                    {
                        if (paths[x] != null)
                            if (paths[x].Contains(n))
                                Gizmos.color = Color.black;
                    }
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));

                    /* if (playerNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                */
                }
            }
        }
    }
}
