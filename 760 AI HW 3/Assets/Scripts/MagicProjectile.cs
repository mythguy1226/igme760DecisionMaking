using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    // Projectile fields
    public float projectileSpeed;
    public GameObject collisionEffect;
    Rigidbody rb;
    bool isDeflected = false;
    public float damage = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the projectile is deflected
        if (!isDeflected)
            // Update projectile velocity at set speed
            rb.velocity = transform.forward * projectileSpeed;
        else
            // Update projectile velocity at set speed in reverse
            rb.velocity = transform.forward * -projectileSpeed;
    }

    // Method for handling collisions
    private void OnCollisionEnter(Collision collision)
    {
        // Spawn the collision effect and then destroy the projectile
        GameObject effect = Instantiate(collisionEffect, rb.position, rb.rotation);
        
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Reverse direction of projectile if a shield is hit
        if (other.gameObject.name.Contains("Shield"))
        {
            isDeflected = true;
        }
    }
}
