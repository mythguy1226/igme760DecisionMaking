using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatComponent : MonoBehaviour
{
    // Get attacking range, projectile obj, and rigidbody
    public float attackRange;
    public GameObject fireBallObj;
    Rigidbody rb;

    // Get point reference to casting location
    public GameObject castingLocationObject;

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
        // Get collision object as magic projectile
        MagicProjectile proj = collision.gameObject.GetComponent<MagicProjectile>();
        if(proj != null)
        {
            // Get projectile's damage
            float damage = proj.damage;

            // Negate health and if below 0 then destroy object
            health -= damage;
            if (health <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }


    // Method for instantiating spell projectile
    // Will be called from anim event inside attack animation 
    public void CastSpell()
    {
        // Instantiate the fireball
        GameObject fball = Instantiate(fireBallObj, castingLocationObject.transform.position, rb.rotation);
    }
}
