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
    // Store current state
    EnemyStates currentState;

    // Declare AI components here
    PathMovement movementControls;
    AIFieldOfView visionDetector;
    AStar pathFinder;
    EnemyAnimationControls animControls;
    CombatComponent combatControls;

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
        animControls = GetComponent<EnemyAnimationControls>();
        combatControls = GetComponent<CombatComponent>();
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
                // Return if the target object is null
                if (targetObject == null)
                    return;

                // Calculate distance to the target
                float distanceToTarget = Vector3.Distance(transform.position, targetObject.transform.position);

                // Check if within attacking range
                if (distanceToTarget <= combatControls.attackRange)
                {
                    currentState = EnemyStates.Attacking;
                }

                // Check if enemy is in middle of attack animation
                if (animControls.AttackAnimationPlaying())
                {
                    // Stop movement
                    movementControls.isStopped = true;
                }

                // Enable movement and set agent's target to
                // be the target object's position
                movementControls.isStopped = false;
                pathFinder.target = targetObject.transform.position;
                break;

            // Handle Attacking behavior
            case EnemyStates.Attacking:
                // Return if the target object is null
                if (targetObject == null)
                    return;

                // Stop movement but still face target
                movementControls.isStopped = true;
                movementControls.FaceTarget(targetObject.transform.position - transform.position);

                // Play attack sequence
                if(combatControls.canAttack)
                {
                    animControls.PlayAttackAnimation();
                    combatControls.canAttack = false;
                    combatControls.cooldownTimer = combatControls.attackCooldown;
                }

                // Check if within attacking range
                if (Vector3.Distance(transform.position, targetObject.transform.position) > combatControls.attackRange)
                {
                    currentState = EnemyStates.Pursuing;
                }
                break;

            // If anything weird ever happens to enum value,
            // just default to idle behavior
            default:
                currentState = EnemyStates.Idle;
                break;
        }

    }
}
