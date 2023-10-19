using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // Public fields
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    // 2D array that will store all nodes in the grid
    Node[,] grid;

    // Private grid and node values declared to be set at start
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start()
    {
        // Init grid and node values
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        // Generate Node Grid
        CreateGrid();
    }

    // Property for getting the max size of the grid
    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    // Method for generating the node grid
    void CreateGrid()
    {
        // Init the grid
        grid = new Node[gridSizeX, gridSizeY];

        // Get bottom left corner of the grid
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

        // At each coordinate in the grid generate a node
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                // Get the world point of the new node
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                
                // Check collisions at the node's position
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                
                // Assign the new node to the grid
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    // Method for getting all neighbors of a node
    public List<Node> GetNeighbors(Node node)
    {
        // Init list of neighbors
        List<Node> neighbors = new List<Node>();

        // Iterate through all potential neighbors
        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Ignore self
                if(x == 0 && y == 0)
                {
                    continue;
                }

                // Index values to check
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // Check that neighbors are in the grid (for edge cases)
                if(checkX >= 0 && checkX < gridSizeX  && checkY >= 0 && checkY < gridSizeY)
                {
                    // Add to list of neighbors
                    neighbors.Add(grid[checkX,checkY]);
                }
            }
        }

        // Return list of neighbors
        return neighbors;
    }

    // Method used for getting a node on the grid from
    // a world position
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        // Get the percentages
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        
        // Clamp the percentages to be values between 0 and 1
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // Get the indices needed to grab the right node
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x,y];
    }
}
