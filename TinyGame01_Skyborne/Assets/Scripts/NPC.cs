using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public abstract class NPC : MonoBehaviour
{
    [SerializeField] protected float acceleration = 2;
    [SerializeField] protected float maxSpeed = 3;
    protected Vector2 xMovementDirection;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected List<Transform> patrolPoints = new List<Transform>();
    protected int patrolPointIndex = 0;
    protected Rigidbody2D rigidBody;
    [SerializeField] private bool isStatic = false;
    protected int directionOfMovement = -1;
    private AudioSource audioSource;
    [SerializeField] private AudioClip deathClip;

    // Awake is called before the object is drawn to the screen, runs before Start
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        xMovementDirection = Vector2.left;
    }

    // FixedUpdate is called every Physics update (Slower than Update)
    private void FixedUpdate()
    {
        if (!isStatic) // If the NPC is not intended to be static...
        {
            if (patrolPoints.Count != 0) // If there are any patrol points...
            {
                Patrol();
            }

            Move();
        }        
    }

    /// <summary>
    /// Performs the functionality on death for the NPC
    /// </summary>
    public void Die()
    {
        audioSource.clip = deathClip;
        audioSource.Play();

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(gameObject, 2); // Destroys the attached gameObject, may change later when animations are added.
    }

    protected virtual void Move()
    {
        if (directionOfMovement == 1) // Rightwards movement
        {
            xMovementDirection = Vector2.right; // Record movement vector
            transform.rotation = new Quaternion(transform.rotation.x, 180, 0, 0); // Rotate NPC to match movement
        }
        else if (directionOfMovement == -1) // Leftwards movement
        {
            xMovementDirection = Vector2.left; // Record movement vector
            transform.rotation = new Quaternion(transform.rotation.x, 0, 0, 0); // Rotate NPC to match movement
        }

        rigidBody.AddForce(xMovementDirection * acceleration, ForceMode2D.Impulse); // Apply force in the direction of the movement vector
        ClampVelocity(); // Clap velocity after any movement to ensure it stays withing bounds
    }

    /// <summary>
    /// Clamps the velocity of the player character so that they don't continue to accelerate to unintended speeds,
    /// </summary>
    protected void ClampVelocity()
    {
        // Overrides the players velocity if it is below the minimum, or above the maximum threshhold
        // Note: Negative values are used for the minimum as when traversing left along the X axis, velocity is negative to go 'backwards'
        rigidBody.velocity = new Vector2(Mathf.Clamp(rigidBody.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rigidBody.velocity.y, -maxSpeed, maxSpeed));
    }


    /// <summary>
    /// Has the NPC patrol in order between all patrolPoints assigned
    /// </summary>
    protected virtual void Patrol()
    {
        if (patrolPoints.Count > 0) // If there are any patrol points...
        {
            float xDistanceFromPoint = patrolPoints[patrolPointIndex].position.x - transform.position.x; // Calculate and store distance to current patrol point

            if (Mathf.Abs(xDistanceFromPoint) < 1) // If the NPC gets within close distance of the patrol point...
            {
                if (patrolPointIndex + 1 > patrolPoints.Count - 1) // If there no more patrol points after this one...
                {
                    patrolPointIndex = 0; // Go back to the start
                }
                else // Otherwise there must be more patrol points...
                {
                    patrolPointIndex++; // Look at the next patrol point
                }
            }

            if (xDistanceFromPoint < 0) // If the calculated distance shows the point is on the left (negative value)
            {
                directionOfMovement = -1; // Point is on the left so set directionOfMovement to match
            }
            else // Otherwise the calculated distance shows that the point is on the right (positive value)
            {
                directionOfMovement = 1; // Point is on the right so set directionOfMovement to match
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Tar>())
        {
            Die();
            // Add extra scream sound here?
        }
    }
}
