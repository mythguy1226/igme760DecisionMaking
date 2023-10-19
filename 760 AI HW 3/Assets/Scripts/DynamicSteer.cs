using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSteer : MonoBehaviour
{
    // Public fields
    public float maxSpeed;
    public float maxAcceleration;
    public float targetRadius;
    public float slowRadius;

    // Private fields
    private Rigidbody rigidBody;
    private Vector3 currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        currentVelocity = new Vector3();
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    // Method for steering towards a target
    public void Steer(Vector3 targetPosition)
    {
        // Get the direction to steer towards and the distance to the target
        Vector3 direction = targetPosition - transform.position;
        float distance = direction.magnitude;

        // Stop moving since you have reached your destination
        if(distance <= targetRadius)
        {
            rigidBody.velocity = new Vector3();
            currentVelocity = new Vector3();
            return;
        }

        // Init target values
        float targetSpeed = 0;
        Vector3 targetVelocity = new Vector3();

        // Calculate speed when outside of slow radius
        if (distance > slowRadius)
        {
            targetSpeed = maxSpeed;
        }
        else // When inside slow radius
        {
            targetSpeed = maxSpeed * (distance / slowRadius);
        }

        // Calculate the target velocity
        targetVelocity = direction.normalized * targetSpeed;

        // Calculate the target acceleration
        Vector3 acceleration = targetVelocity - rigidBody.velocity;
        acceleration /= Time.deltaTime;

        // Clamp the acceleration to the max
        if(acceleration.magnitude > maxAcceleration)
        {
            acceleration = acceleration.normalized * maxAcceleration;
        }

        // Generate vector for new velocity
        currentVelocity = new Vector3(
            currentVelocity.x + acceleration.x * Time.deltaTime,
            currentVelocity.y + acceleration.y * Time.deltaTime,
            currentVelocity.z + acceleration.z * Time.deltaTime
            );

        // Face the target based on new velocity
        FaceTarget(currentVelocity);

        // Set the new velocity
        rigidBody.velocity = currentVelocity;
    }

    // Method used for facing the target
    public void FaceTarget(Vector3 velocity)
    {
        // Return if not moving
        if (velocity.magnitude == 0)
            return;

        // Set rotation towards velocity direction
        transform.rotation = Quaternion.LookRotation(velocity.normalized, rigidBody.transform.up);
    }

    // Method for getting current speed value
    public float GetCurrentSpeed()
    {
        return rigidBody.velocity.magnitude;
    }
}
