using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    Pursuing,
    Defending,
    Collecting
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

    // Get game object to pursue
    public GameObject goalCollectible;

    // Particle system for post-collection
    public GameObject collectParticles;

    // Start is called before the first frame update
    void Start()
    {
        // Pursue goal by default
        currentState = PlayerStates.Pursuing;

        // Get the AI components from game object
        movementControls = GetComponent<PathMovement>();
        visionDetector = GetComponent<AIFieldOfView>();
        animControls = GetComponent<PlayerAnimationControls>();

        // Tell movement controls to set new goal
        movementControls.SetTargetPath(goalCollectible.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // Finite State machine for handling main character behaviors
        switch (currentState)
        {
            // Handle Pursuing behavior
            case PlayerStates.Pursuing:
                // Enable movement and disable shields
                movementControls.isStopped = false;
                animControls.SetBlock(false);
                shieldObject.SetActive(false);

                // Check visual detections for enemy encounters
                if (visionDetector.detectedColliders.Count > 0)
                {
                    // Set the state to blocking
                    currentState = PlayerStates.Defending;
                }

                // Check if player has reached their goal
                if(movementControls.hasReachedDestination && goalCollectible != null)
                {
                    // Set the state to collecting
                    currentState = PlayerStates.Collecting;
                }

                break;

            // Handle Defending behavior
            case PlayerStates.Defending:
                // Disable movement and enable shield
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

            // Handle Collecting behavior
            case PlayerStates.Collecting:
                // Disable movement and collect goal item
                movementControls.isStopped = true;

                // Trigger collection in anim controls
                animControls.CollectItem();

                break;

            // If anything weird ever happens to enum value,
            // just default to pursue behavior
            default:
                currentState = PlayerStates.Pursuing;
                break;
        }

    }

    // Method used for triggering collection of item
    // Will be called from anim event inside collect animation
    public void DestroyCollectedItem()
    {
        // Destroy goal collectible
        Destroy(goalCollectible);

        // Set state back to pursue
        currentState = PlayerStates.Pursuing;

        // Get children of object as particle systems
        ParticleSystem[] particles = collectParticles.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem particle in particles)
        {
            // Play particle
            particle.Play();
        }
    }
}
