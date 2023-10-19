using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    // Fields for grid and target
    public Vector3 target;
    public Grid grid;
    public List<Node> targetPath;

    // Update the path every frame
    void Update()
    {
        FindPath(transform.position, target);
    }

    // Method for finding the shortest path using the A* Algorithm
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // Get nodes on the grid from the start and end points
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        // Init open set and closet set
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        // Add the starting node to open set
        openSet.Add(startNode);
        
        // Main loop for finding path
        while (openSet.Count > 0)
        {
            // Remove current node from the open set
            // and add it to the closed set
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            // Once you've reached the goal, set the grid path and return
            if(currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            // Iterate through each neighboring node of the current node
            foreach(Node neighbor in grid.GetNeighbors(currentNode))
            {
                // Skip neighbor if unwalkable or already part of the path
                if(!neighbor.isWalkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                // Calculate the g cost from the current node to the neighbor
                int costToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if(costToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    // Set the g cost and calculate the h cost of moving to neighbor node
                    // heuristic being used here is distance to end
                    neighbor.gCost = costToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);

                    // Set current node as parent node for retracing
                    neighbor.parent = currentNode;

                    // Add neighbor node to the open set
                    // if it hasn't been added already
                    if(!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    // Method for retracing path after the shortest path has been found
    void RetracePath(Node startNode, Node endNode)
    {
        // Init path list and current node
        List<Node> path = new List<Node>();
        Node currentNode = endNode; 

        // Iteratively work up the node chain and add
        // to the path list
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        // Reverse the path to go in the right direction
        path.Reverse();

        // Set the path to the list
        targetPath = path;
    }

    // Method for calculating distance between nodes
    int GetDistance(Node nodeA, Node nodeB)
    {
        // Get the distance values between the nodes
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        // Cases for return values
        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);

        return 14 * distX + 10 * (distY - distX);
    }
}
