using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // Get the animator instance
    private Animator animator;

    // Declare any animation hashes here
    int speedHash;

    // Get movement control component
    PathMovement movementControls;

    // Start is called before the first frame update
    void Start()
    {
        // Init animator reference and hashes
        animator = gameObject.GetComponent<Animator>();
        speedHash = Animator.StringToHash("Speed");
        movementControls = GetComponent<PathMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update speed hash with current speed
        animator.SetFloat(speedHash, movementControls.rb.velocity.magnitude / movementControls.maxSpeed);
    }
}
