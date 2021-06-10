using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController controller;
    public float movementSpeed = 14f;

    // v = sqrt(h - 2 * g) where v: velocity, h: height, g: gravity
    public float jumpVelocity = 10f;
    public float dashVelocity = 500f;
    public float gravity = -25f;

    // collision detection
    public Transform groundCheck;
    // the radius of the sphere around which collisions will be detected
    public float sphereRadius = 0.2f;
    // only game objects with this layer mask will be included in the collision detection
    public LayerMask groundMask;


    private Vector3 velocity;
    private bool grounded;
    private bool doubleJumped;


    // Update is called once per frame
    void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);

        if(grounded && velocity.y < 0f)
        {
            velocity.y = 0f;
            doubleJumped = false;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        int dash = 0;
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            dash = 1;
        }

        // use relative coordinates to move in the direction the camera is facing
        Vector3 moveDirection = transform.right * x + transform.forward * z;
        // normalize to avoid moving at double speed when moving both vertically and horizontally
        moveDirection.Normalize();
        moveDirection = movementSpeed * moveDirection + transform.forward * dashVelocity * dash;
        controller.Move(moveDirection * Time.deltaTime);

        // jumping
        if(Input.GetButtonDown("Jump"))
        {
            if(grounded)
            {
                velocity.y = jumpVelocity;
            }else
            {
                if(doubleJumped == false)
                {
                    velocity.y = jumpVelocity;
                    doubleJumped = true;
                }
            }
        }

        // v = 0.5 * g * t^2
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
