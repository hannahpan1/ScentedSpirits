using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider col;
    float speed = 50;
    float topSpeed = 6;
    float rotationGoal;
    float currentRotation;
    public float jumpStrength = 10f; // Increased jump strength
    public float gravityMultiplier = 2f; // Added gravity multiplier
    LayerMask jumpMask;
    public float playerDeath; // how far down before player death
    public Animator myAnim; //used for player animations

    public bool HasKey { get; set; }
    public bool canMove = true; // New boolean to control movement

    public void PickUpKey()
    {
        HasKey = true;
        Debug.Log("Key picked up!");
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        // Set drag to make movement less floaty
        //rb.drag = 5f;

        // Get the layer number of "Ignore Raycast"
        int ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");

        // Create a LayerMask that includes all layers
        jumpMask = ~0;

        // Exclude the "Ignore Raycast" layer
        jumpMask &= ~(1 << ignoreRaycastLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            rb.isKinematic = false;
        }// Prevent movement if canMove is false
        else
        {
            rb.isKinematic = true;
        }


        if (gameObject.transform.position.y < playerDeath)
        {
            GameEvents.current.gameOver?.Invoke();
        }
        
        if (Mathf.Abs(rb.velocity.x) > topSpeed / 10f)
        {
            if (rb.velocity.x < 0)
            {
                rotationGoal = 0;
            }
            else
            {
                rotationGoal = 180;
            }
        }

        //if (Input.GetButtonDown("Jump"))
        //{
        //    jump();
        //}

        currentRotation = currentRotation + (rotationGoal - currentRotation) * 10f * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, currentRotation, 0);

        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        // Get the surface normal

        float stickRadius = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude;

        // Project the movement direction onto the plane defined by the surface normal
        moveDirection = Vector3.ProjectOnPlane(moveDirection, SurfaceNormal());


        if (!groundCol())
        {
            rb.AddForce(moveDirection * speed * stickRadius, ForceMode.Acceleration);
        } else
        {
            rb.AddForce(moveDirection * speed * stickRadius * 0.2f, ForceMode.Acceleration);
        }
        

        //Apply custom gravity
        //if (!Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.1f))
        //{
        //    rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
        //}

        // Clamp the velocity to the top speed
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -topSpeed, topSpeed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -topSpeed, topSpeed));

        myAnim.SetBool("IsMoving", moveDirection.x != 0 || moveDirection.z != 0 );

    }

    private bool groundCol()
    {
        Vector3 start = transform.position - (new Vector3(0, col.height / 2) * 0.95f);
        Vector3 dir = Vector3.down;
        Debug.DrawRay(start, dir, Color.red, 1);
        RaycastHit hit;
        Physics.Raycast(start, dir, out hit, col.height * 0.7f, jumpMask);
        if (hit.collider != null)
        {
            return true;
        }
            return false;
        
    }
    private Vector3 SurfaceNormal()
    {
        
        Vector3 start = transform.position;
        Vector3 dir = Vector3.down;
        Debug.DrawRay(start, dir, Color.red, col.height / 2);
        RaycastHit hit;
        Physics.Raycast(start, dir, out hit, 0.2f, jumpMask);
        if (hit.collider == null)
        {
            return Vector3.zero;
        }
        return hit.normal;
    }

    private void jump()
    {
       if (groundCol())
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }
    }
    private Vector3 launchVector(Vector3 startPos, Vector3 targetPos, Rigidbody rb)
    {
        // Calculate the direction to the target
        Vector3 direction = (targetPos - startPos);
        direction.y = 0; // We only need the horizontal direction
        direction.Normalize();

        // Calculate the distance to the target
        float distance = Vector3.Distance(new Vector3(startPos.x, 0, startPos.z), new Vector3(targetPos.x, 0, targetPos.z));

        // Calculate the difference in height
        float deltaY = targetPos.y - startPos.y;

        // Calculate the initial speed (velocity) required to reach the target
        // We use the equation of motion: v^2 = u^2 + 2as
        // And rearrange it to solve for u (initial velocity): u = sqrt(v^2 - 2as)
        // Where:
        // v is the final velocity (0 in this case, at the highest point of the trajectory)
        // u is the initial velocity (which we're trying to find)
        // a is the acceleration (gravity)
        // s is the distance (vertical distance in this case)
        float initialSpeed = Mathf.Sqrt((Physics.gravity.magnitude * distance * distance) / (2 * deltaY));

        // Calculate the launch vector
        Vector3 launchVector = direction * initialSpeed;

        return launchVector;
    }
}
