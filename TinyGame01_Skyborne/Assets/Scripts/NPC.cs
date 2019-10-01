using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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

    // Awake is called before the object is drawn to the screen, runs before Start
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        xMovementDirection = Vector2.left;
    }

    private void FixedUpdate()
    {
        if (!isStatic)
        {
            if (patrolPoints.Count != 0)
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
        Destroy(gameObject); // Destroys the attached gameObject, may change later when animations are added.
    }

    protected virtual void Move()
    {
        if (directionOfMovement == 1) // Right
        {
            xMovementDirection = Vector2.right;
            transform.rotation = new Quaternion(transform.rotation.x, 180, 0, 0);
        }
        else if (directionOfMovement == -1) // Left
        {
            xMovementDirection = Vector2.left;
            transform.rotation = new Quaternion(transform.rotation.x, 0, 0, 0);
        }

        rigidBody.AddForce(xMovementDirection * acceleration, ForceMode2D.Impulse);
        ClampVelocity();
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

    protected virtual void Patrol()
    {
        if (patrolPoints.Count > 0)
        {
            float xDistanceFromPoint = patrolPoints[patrolPointIndex].position.x - transform.position.x;

            if (Mathf.Abs(xDistanceFromPoint) < 1)
            {
                if (patrolPointIndex + 1 > patrolPoints.Count - 1)
                {
                    patrolPointIndex = 0;
                }
                else
                {
                    patrolPointIndex++;
                }
            }

            if (xDistanceFromPoint < 0)
            {
                directionOfMovement = -1; // Point is on the left so set directionOfMovement to match
            }
            else
            {
                directionOfMovement = 1; // Point is on the right so set directionOfMovement to match
            }
        }
    }
}
