using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    Pursuing,
    Defending,
    Attacking
}

public class Player : MonoBehaviour
{
    // Store current state
    PlayerStates currentState;

    // Declare AI components here
    PathMovement movementControls;
    AIFieldOfView visionDetector;


    // Start is called before the first frame update
    void Start()
    {
        // Pursue goal by default
        currentState = PlayerStates.Pursuing;

        // Get the AI components from game object
        movementControls = GetComponent<PathMovement>();
        visionDetector = GetComponent<AIFieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        // Finite State machine for handling main character behaviors
        switch (currentState)
        {
            // Handle Pursuing behavior
            case PlayerStates.Pursuing:
                movementControls.isStopped = false;
                break;

            // Handle Defending behavior
            case PlayerStates.Defending:
                break;

            // Handle Attacking behavior
            case PlayerStates.Attacking:
                break;

            // If anything weird ever happens to enum value,
            // just default to pursue behavior
            default:
                currentState = PlayerStates.Pursuing;
                break;
        }

    }
}
