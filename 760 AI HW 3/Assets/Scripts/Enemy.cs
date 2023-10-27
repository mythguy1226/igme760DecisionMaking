using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    Idle,
    Pursuing,
    Attacking
}

public class Enemy : MonoBehaviour
{
    EnemyStates currentState;

    // Declare AI components here
    PathMovement movementControls;
    AIFieldOfView visionDetector;
    AStar pathFinder;

    // Declare object used for target references
    GameObject targetObject;

    // Start is called before the first frame update
    void Start()
    {
        // Pursue goal by default
        currentState = EnemyStates.Idle;

        // Get the AI components from game object
        movementControls = GetComponent<PathMovement>();
        visionDetector = GetComponent<AIFieldOfView>();
        pathFinder = GetComponent<AStar>();
    }

    // Update is called once per frame
    void Update()
    {
        // Finite State machine for handling main character behaviors
        switch (currentState)
        {
            // Handle Idle behavior
            case EnemyStates.Idle:
                // Movement is disabled in this state
                movementControls.isStopped = true;

                // Check visual detections for player encounters
                if (visionDetector.detectedColliders.Count > 0)
                {
                    // Set the target as the first player object seen
                    targetObject = visionDetector.detectedColliders[0].gameObject;
                    currentState = EnemyStates.Pursuing;
                }
                break;

            // Handle Pursuing behavior
            case EnemyStates.Pursuing:
                // Enable movement and set agent's target to
                // be the target object's position
                movementControls.isStopped = false;
                pathFinder.target = targetObject.transform.position;
                break;

            // Handle Attacking behavior
            case EnemyStates.Attacking:
                break;

            // If anything weird ever happens to enum value,
            // just default to idle behavior
            default:
                currentState = EnemyStates.Idle;
                break;
        }

    }
}
