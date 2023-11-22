using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;

    List<RaycastHit2D> castCollision = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        // If movement input is not 0, try to move
        if (movementInput != Vector2.zero) {
            bool success = TryMove(movementInput.normalized);

            if (!success) {
                // If movement fails, try to move in the X direction
                success = TryMove(new Vector2(movementInput.x, 0).normalized);

                if (!success) {
                    // If movement fails, try to move in the Y direction
                    success = TryMove(new Vector2(0, movementInput.y).normalized);
                }
            }

            animator.SetBool("isMoving", success);
        } else {
            animator.SetBool("isMoving", false);
        }


    }

    private bool TryMove(Vector2 direction){
        int count = rb.Cast(
                direction, // X and Y direction values between -1 and 1
                movementFilter, // Filter for what objects to collide with
                castCollision, // List of collisions 
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset to prevent clipping

            if (count == 0) {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            } else {
                return false;
            }
    }
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}
