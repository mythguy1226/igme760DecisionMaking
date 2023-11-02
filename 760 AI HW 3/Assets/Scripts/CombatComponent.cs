using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatComponent : MonoBehaviour
{
    // Get attacking range, projectile obj, and rigidbody
    public float attackRange;
    public GameObject fireBallObj;
    Rigidbody rb;

    // Attack cooldown management
    public float attackCooldown = 2.0f;
    public float cooldownTimer;
    public bool canAttack = true;

    // Field for health
    public float health = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Set attack fields
        cooldownTimer = attackCooldown;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update attack timer and attack status
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0.0f)
        {
            canAttack = true;
        }
    }

    // Handle blocking collisions here
    private void OnCollisionEnter(Collision collision)
    {
        // Collision handler for fireball projectiles
        if (collision.gameObject.name.Contains("Fireball"))
        {
            // Negate health and if below 0 then destroy object
            health -= 50.0f;
            if (health <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }


    // Method for instantiating spell projectile
    public void CastSpell()
    {
        // Instantiate the fireball
        GameObject fball = Instantiate(fireBallObj, rb.position + new Vector3(0, 1, 0) + (rb.transform.forward * 2), rb.rotation);
    }
}
