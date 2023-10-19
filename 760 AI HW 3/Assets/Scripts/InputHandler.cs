using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // Public fields to set in editor
    public Camera camera;
    public LayerMask traversalMask;

    // Reference to pathfinding component
    AStar pathFinder;

    // Get the pathfinding component to reference path
    void Start()
    {
        pathFinder = GetComponent<AStar>();
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse click input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Set the current target
            pathFinder.target = GetTargetPoint();
        }
    }

    public Vector3 GetTargetPoint()
    {
        // Trace a ray from the camera to the world
        // where the mouse is pointing
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, traversalMask))
        {
            // Return the hit point
            return hit.point;
        }

        // Return out of player view if not a proper target point
        return new Vector3(0, -50, 0);
    }
}
