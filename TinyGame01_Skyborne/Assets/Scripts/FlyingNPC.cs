using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingNPC : NPC
{
    protected override void Move()
    {
        if (rigidBody.velocity.y != 0)
        {
            if (directionOfMovement == 1) // Right
            {
                xMovementDirection = Vector2.right;
            }
            else if (directionOfMovement == -1) // Left
            {
                xMovementDirection = Vector2.left;
            }
        }

        rigidBody.AddForce(xMovementDirection * acceleration, ForceMode2D.Impulse);
        ClampVelocity();
    }

    protected override void Patrol()
    {
        if (patrolPoints.Count > 0)
        {
            float xDistanceFromPoint = patrolPoints[patrolPointIndex].position.x - transform.position.x;
            float yDistanceFromPoint = patrolPoints[patrolPointIndex].position.y - transform.position.y;

            if (Mathf.Abs(xDistanceFromPoint) < 1 && Mathf.Abs(yDistanceFromPoint) < 1)
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

            if (yDistanceFromPoint < 0) // Below
            {
                rigidBody.AddForce(Vector2.down * acceleration, ForceMode2D.Impulse);
            }
            else // Above
            {
                rigidBody.AddForce(Vector2.up * acceleration, ForceMode2D.Impulse);
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
