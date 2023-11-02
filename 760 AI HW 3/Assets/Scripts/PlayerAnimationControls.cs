using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControls : MonoBehaviour
{
    // Get the animator instance
    private Animator animator;

    // Declare any animation hashes here
    int walkingHash;
    int blockingHash;

    // Get rigid body component
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // Init animator reference and hashes
        animator = gameObject.GetComponent<Animator>();
        walkingHash = Animator.StringToHash("IsWalking");
        blockingHash = Animator.StringToHash("IsBlocking");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update walking hash with walk status
        if (rb.velocity.magnitude > 0.0f)
            animator.SetBool(walkingHash, true);
        else
            animator.SetBool(walkingHash, false);
    }

    // Method for setting block state
    public void SetBlock(bool isBlocking)
    {
        // Update blocking hash with method parameter
        animator.SetBool(blockingHash, isBlocking);
    }
}
