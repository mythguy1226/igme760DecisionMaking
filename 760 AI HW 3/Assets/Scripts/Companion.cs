using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    // Get the player character
    public GameObject targetObject;

    // Get reference to A* component
    AStar pathFinder;

    // Get the pathfinding component to reference path
    void Start()
    {
        pathFinder = GetComponent<AStar>();
    }

    // Runs once per frame
    void Update()
    {
        // Set the target of the companion
        pathFinder.target = targetObject.transform.position;

        // Maintain height of target object
        transform.position = new Vector3(
            transform.position.x,
            targetObject.transform.position.y,
            transform.position.z);
    }
}
