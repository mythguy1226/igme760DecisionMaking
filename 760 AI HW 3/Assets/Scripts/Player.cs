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
    PlayerAnimationControls animControls;

    // Get reference to shield object
    public GameObject shieldObject;

    // Start is called before the first frame update
    void Start()
    {
        // Pursue goal by default
        currentState = PlayerStates.Pursuing;

        // Get the AI components from game object
        movementControls = GetComponent<PathMovement>();
        visionDetector = GetComponent<AIFieldOfView>();
        animControls = GetComponent<PlayerAnimationControls>();
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
                animControls.SetBlock(false);
                shieldObject.SetActive(false);

                // Check visual detections for enemy encounters
                if (visionDetector.detectedColliders.Count > 0)
                {
                    // Set the state to blocking
                    currentState = PlayerStates.Defending;
                }

                break;

            // Handle Defending behavior
            case PlayerStates.Defending:
                movementControls.isStopped = true;
                animControls.SetBlock(true);
                shieldObject.SetActive(true);

                // Check visual detections for enemy encounters
                if (visionDetector.detectedColliders.Count == 0)
                {
                    // Set the state to blocking
                    currentState = PlayerStates.Pursuing;
                }
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
