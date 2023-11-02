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
    EnemyAnimationControls animControls;

    // Declare object used for target references
    GameObject targetObject;

    // Get attacking range for enemy
    public float attackRange;
    public GameObject fireBallObj;
    Rigidbody rb;
    public float attackCooldown = 5.0f;
    float cooldownTimer;
    bool canAttack;
    float health = 100.0f;

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
        rb = GetComponent<Rigidbody>();

        // Set attack fields
        cooldownTimer = attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        // Update attack cooldown timers
        cooldownTimer -= Time.deltaTime;
        if(cooldownTimer <= 0.0f)
        {
            canAttack = true;
        }


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

                // Calculate distance to the target
                float distanceToTarget = Vector3.Distance(transform.position, targetObject.transform.position);

                // Check if within attacking range
                if (distanceToTarget <= attackRange)
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

                // Stop movement but still face target
                movementControls.isStopped = true;
                movementControls.FaceTarget(targetObject.transform.position - transform.position);

                // Play attack sequence
                if(canAttack)
                {
                    animControls.PlayAttackAnimation();
                    canAttack = false;
                    cooldownTimer = 3.0f;
                }

                // Check if within attacking range
                if (Vector3.Distance(transform.position, targetObject.transform.position) > attackRange)
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Contains("Fireball"))
        {
            health -= 50.0f;

            if(health <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void CastSpell()
    {
        // Instantiate the fireball
        GameObject fball = Instantiate(fireBallObj, rb.position + new Vector3(0, 1, 0) + (rb.transform.forward * 2), rb.rotation);
    }
}
