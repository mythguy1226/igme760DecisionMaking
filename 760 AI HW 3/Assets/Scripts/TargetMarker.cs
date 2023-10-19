using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMarker : MonoBehaviour
{
    // Reference to player
    public GameObject playerRef;

    // Needed components to reference from player
    PathMovement movementControls;
    InputHandler inputControls;
    AStar pathFinder;

    // Called at beginning of play
    void Start()
    {
        // Get component references from player reference
        movementControls = playerRef.GetComponent<PathMovement>();
        inputControls = playerRef.GetComponent<InputHandler>();
        pathFinder = playerRef.GetComponent<AStar>();
    }

    // Update is called once per frame
    void Update()
    {
        // Create vector for slight offset, making target more visible
        Vector3 offset = new Vector3(0.0f, 2.0f, 0.0f);

        // Moving
        if (movementControls.rb.velocity.magnitude > 0)
        {
            transform.position = pathFinder.target;
        }
        else // Not Moving
        {
            transform.position = inputControls.GetTargetPoint();
        }

        // Continuously rotate the target
        transform.rotation *= Quaternion.AngleAxis(0.1f, new Vector3(0.0f, 0.0f, 1.0f));
    }
}
