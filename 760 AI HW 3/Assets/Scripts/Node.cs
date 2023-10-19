using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {

    // Public fields
    public bool isWalkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public Node parent;

    // Private field for storing heap index
    int HeapIndex;
    
    // Constructor
    public Node(bool walkable, Vector3 position, int gridXPos, int gridYPos)
    {
        isWalkable = walkable;
        worldPosition = position;

        gridX = gridXPos;
        gridY = gridYPos;
    }

    // Property for getting the fCost of the node
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    // Property for getting and setting the index
    // of this node on the heap data structure
    public int heapIndex
    {
        get
        {
            return HeapIndex;
        }
        set
        {
            HeapIndex = value;
        }
    }

    // Method used for comparing the cost of two nodes
    public int CompareTo(Node nodeToCompare)
    {
        // First check fCost and if those are equal
        // then bring it down to hCost
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
