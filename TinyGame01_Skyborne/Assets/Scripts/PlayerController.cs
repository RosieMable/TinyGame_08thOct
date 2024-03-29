﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
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
    private Animator anim;
    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip screamClip;
    private GameObject gameCamera;

    // Awake is called before the object is drawn to the screen, runs before Start
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>(); // Store reference
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        gameCamera = FindObjectOfType<Camera>().gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float xMovement = Input.GetAxis("Horizontal"); // Store recorded input on the X axis
        Vector2 movement = new Vector2(xMovement * acceleration, 0); // Convert into Vector2

        if (xMovement != 0) // If there is input detected...
        {
            if (xMovement > 0) // If we are moving towards the right...
            {
                transform.rotation = new Quaternion(0, 180, 0, 0); // Rotate object to face right
            }
            else if (xMovement < 0) // Else if we are moving left...
            {
                transform.rotation = new Quaternion(0, 0, 0, 0); // Rotate object to face left (default)
            }

            rigidBody.AddForce(movement, ForceMode2D.Impulse); // Add force in the given direction
        }
        else // Otherwise if there is no input detected...
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y); // Reset X velocity but maintain Y velocity incase the player is mid jump or falling
        }

        anim.SetFloat("xVelocity", Mathf.Abs(rigidBody.velocity.x));
        anim.SetFloat("yVelocity", rigidBody.velocity.y);
        ClampVelocity(); // Clamp velocity after any physics movement has occured to ensure velocity stays within bounds
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // If the player presses the spacebar...
        {
            if (Time.time > jumpDelay) // Check if enough time has passed for another jump...
            {
                jumpDelay = Time.time + jumpCooldown; // Put jump on cooldown
                Jump();
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            // Scream + Camera Shake?
            CameraShake();
            anim.SetBool("IsScreaming", true);

            audioSource.clip = screamClip;
            audioSource.loop = true;
            audioSource.pitch = 2;

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
            audioSource.pitch = 1;
            audioSource.loop = false;
            anim.SetBool("IsScreaming", false);
        }

        anim.SetBool("IsGrounded", IsGrounded());
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

            if (!audioSource.isPlaying)
            {
                audioSource.clip = jumpClip;
                audioSource.Play();
            }
        }
    }

    /// <summary>
    /// Clamps the velocity of the player character so that they don't continue to accelerate to unintended speeds,
    /// </summary>
    private void ClampVelocity()
    {
        // Overrides the players velocity if it is below the minimum, or above the maximum threshhold
        // Note: Negative values are used for the minimum as when traversing left along the X axis, velocity is negative to go 'backwards'
        rigidBody.velocity = new Vector2(Mathf.Clamp(rigidBody.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rigidBody.velocity.y, -jumpStrength, jumpStrength));
    }

    /// <summary>
    ///  Checks that the player character is grounded via raycast
    /// </summary>
    private bool IsGrounded()
    {
        RaycastHit2D hit; // Variable to store reference of what the raycast hits
        //hit = Physics2D.Raycast(transform.position, Vector2.down, jumpRaycastLength, groundLayer); // Raycast downwards from centre of the player checking if we are above any objects on the ground layer.
        hit = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0, Vector2.down, jumpRaycastLength, groundLayer);
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
        GameManager.instance.GameOver();
    }

    private void CameraShake()
    {
        Vector3 cameraOriginalPos = gameCamera.transform.localPosition;

        float x = Random.Range(gameCamera.transform.position.x - 0.03f, gameCamera.transform.position.x + 0.03f);
        float y = Random.Range(gameCamera.transform.position.y - 0.03f, gameCamera.transform.position.y + 0.03f);

        gameCamera.transform.localPosition = new Vector3(x, y, cameraOriginalPos.z);
    }

    private void FinishLevel()
    {
        GameManager.instance.LevelComplete();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<NPC>()) // If the trigger area is an NPC...
        {
            NPC NPCSquished = collision.gameObject.GetComponent<NPC>(); // Store reference to NPC
            NPCSquished.Die(); // Call Die method from NPC

            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0); // Reset y velocity
            rigidBody.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse); // Boost player upwards

            if (!audioSource.isPlaying)
            {
                audioSource.clip = jumpClip;
                audioSource.Play();
            }

            GameManager.instance.IncreaseScore();
        }

        if (collision.gameObject.GetComponent<Tar>()) // If the trigger area is the Tar...
        {
            Die();
        }

        if (collision.gameObject.layer == 10) // If the trigger area is the goal...
        {
            FinishLevel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<NPC>()) // If the collision is with an NPC...
        {
            Die();
        }
    }
}
