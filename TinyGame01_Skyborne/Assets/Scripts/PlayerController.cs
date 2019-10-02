﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float acceleration = 2;
    [SerializeField] private float jumpStrength = 5;
    private float jumpDelay = 0;
    [SerializeField] private float jumpCooldown = 0.5f;
    public float JumpStrength { get { return JumpStrength; } set { JumpStrength = value; } }
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpRaycastLength = 1;
    private Rigidbody2D rigidBody;

    // Awake is called before the object is drawn to the screen, runs before Start
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float xMovement = Input.GetAxis("Horizontal"); // Store recorded input on the X axis
        Vector2 movement = new Vector2(xMovement * acceleration, 0); // Convert into Vector2

        if (xMovement != 0) // If there is input detected...
        {
            if (xMovement > 0)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else if (xMovement < 0)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }

            rigidBody.AddForce(movement, ForceMode2D.Impulse); // Add force in the given direction
        }
        else // Otherwise if there is no input detected...
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }

        ClampVelocity();
    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space)) // If the player presses the spacebar...
        {
            if (Time.time > jumpDelay)
            {
                jumpDelay = Time.time + jumpCooldown;
                Jump();
            }
        }
    }

    /// <summary>
    /// Adds upwards force to the player character when pressing the jump key, granted that they are grounded.
    /// </summary>
    private void Jump()
    {
        if (IsGrounded()) // Check if we are grounded, if this returns true...
        {
            // Add upwards force based on 'JumpStrength', uses 'ForceMode2D.Impulse' as this applies the entirety of the force at once, which is what is wanted from a jump.
            rigidBody.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse); 
        }
    }

    /// <summary>
    /// Clamps the velocity of the player character so that they don't continue to accelerate to unintended speeds,
    /// </summary>
    private void ClampVelocity()
    {
        // Overrides the players velocity if it is below the minimum, or above the maximum threshhold
        // Note: Negative values are used for the minimum as when traversing left along the X axis, velocity is negative to go 'backwards'
        rigidBody.velocity = new Vector2(Mathf.Clamp(rigidBody.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rigidBody.velocity.y, -maxSpeed * 1.6f, maxSpeed * 1.6f));
    }

    /// <summary>
    ///  Checks that the player character is grounded via raycast
    /// </summary>
    private bool IsGrounded()
    {
        RaycastHit2D hit; // Variable to store reference of what the raycast hits
        hit = Physics2D.Raycast(transform.position, Vector2.down, jumpRaycastLength, groundLayer); // Raycast downwards from centre of the player checking if we are above any objects on the ground layer.
        Debug.DrawRay(transform.position, Vector2.down * jumpRaycastLength, Color.green, 0.5f); // Draw the ray inside of the scene editor only for a limited time

        if (hit.collider != null) // If we hit something...
        {
            return true; // Return true, we are grounded
        }
        else // Otherwise if we did not...
        {
            return false; // Return false, we are not grounded
        }
    }

    private void Die()
    {
        // TBD
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<NPC>())
        {
            NPC NPCSquished = collision.gameObject.GetComponent<NPC>();
            NPCSquished.Die();

            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            rigidBody.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
        }

        if (collision.gameObject.GetComponent<Tar>())
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<NPC>())
        {
            Die();
        }
    }
}