using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationControls : MonoBehaviour
{
    // Get the animator instance
    private Animator animator;

    // Declare any animation hashes here
    int walkingHash;
    int attackHash;

    // Get rigid body component
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // Init animator reference and hashes
        animator = gameObject.GetComponent<Animator>();
        walkingHash = Animator.StringToHash("IsWalking");
        attackHash = Animator.StringToHash("Attack");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update walking hash with walk status
        if(rb.velocity.magnitude > 0.0f)
            animator.SetBool(walkingHash, true);
        else
            animator.SetBool(walkingHash, false);
    }

    // Method for playing attack animation
    public void PlayAttackAnimation()
    {
        // Play the animation if not already playing
        if(!AttackAnimationPlaying())
        {
            animator.SetTrigger(attackHash);
        }
    }

    // Method for getting if currently attacking
    public bool AttackAnimationPlaying()
    {
        // Check animator state to see if attack animation is playing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return true;
        return false;
    }
}
