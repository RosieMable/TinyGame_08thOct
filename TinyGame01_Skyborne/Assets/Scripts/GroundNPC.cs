using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundNPC : NPC
{
    protected override void Move()
    {
        if (directionOfMovement == 1) // Right
        {
            xMovementDirection = Vector2.right;
            transform.rotation = new Quaternion(transform.rotation.x, 180, 0, 0);

            RaycastHit2D rightLedge = Physics2D.Raycast((Vector2)transform.position + Vector2.right, Vector2.down, 1, groundLayer);
            Debug.DrawRay((Vector2)transform.position + Vector2.right, Vector2.down, Color.red);

            if (rightLedge.collider == null)
            {
                directionOfMovement = -1;
            }

        }
        else if (directionOfMovement == -1) // Left
        {
            xMovementDirection = Vector2.left;
            transform.rotation = new Quaternion(transform.rotation.x, 0, 0, 0);

            RaycastHit2D leftLedge = Physics2D.Raycast((Vector2)transform.position + Vector2.left, Vector2.down, 1, groundLayer);
            Debug.DrawRay((Vector2)transform.position + Vector2.left, Vector2.down, Color.red);

            if (leftLedge.collider == null)
            {
                directionOfMovement = 1;
            }
        }

        rigidBody.AddForce(xMovementDirection * acceleration, ForceMode2D.Impulse);
        ClampVelocity();
    }
}
