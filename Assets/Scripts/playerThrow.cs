using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerThrow : MonoBehaviour
{
    public GameObject throwablePrefab;  // throw object
    private float baseThrowForce = 2f;  // base throw force
    private float maxThrowForce = 14f;  
    private float forceChange = 4f;  // rate of increase in throw force
    private int trajectoryResolution = 30;  // number of points in the trajectory

    public LineRenderer lineRenderer; 
    public Transform throwDirectionIndicator;  

    private float currentThrowAngle = -60f;  // default angle set to 60 degrees
    private float currentThrowForce;     
    private bool isCharging = false;
    private Rigidbody rb;
    private InventoryManager inventoryManager;

    public bool canThrow = true; // New boolean to control throwing

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        rb = GetComponent<Rigidbody>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.enabled = false; 

        if (throwDirectionIndicator == null)
        {
            Debug.LogError("ThrowDirectionIndicator not assigned.");
        }

        currentThrowForce = baseThrowForce;
    }

    void Update()
    {
        if (!canThrow) return; // Prevent throwing if canThrow is false

        if (!isCharging)
        {
            // Handle movement only if not charging
            HandleMovement();
        }

        // update the rotation direction even when charging
        UpdateThrowDirection();

        if (!IsPointerOverUIElement() && (inventoryManager.GetItemCount("Cheese") > 0)) {
            if (Input.GetMouseButtonDown(0))
            {
                isCharging = true;
                currentThrowForce = baseThrowForce;
                lineRenderer.enabled = true;  // enable line renderer when charging starts
                rb.velocity = Vector3.zero; // stop any movement when starting to charge
                rb.angularVelocity = Vector3.zero; // stop any rotational movement
            }

            if (Input.GetMouseButton(0) && isCharging)
            {
                currentThrowForce += forceChange * Time.deltaTime;
                currentThrowForce = Mathf.Clamp(currentThrowForce, baseThrowForce, maxThrowForce);
                UpdateThrowIndicator();  // update indicator while charging
            }

            if (Input.GetMouseButtonUp(0) && isCharging)
            {
                ThrowObject();
                inventoryManager.RemoveItem("Cheese", 1);
                isCharging = false;
                lineRenderer.enabled = false;  // hide line renderer after throwing
            }
        }
    }

    bool IsPointerOverUIElement()
    {
        // Check if the pointer is over any UI elements
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void HandleMovement()
    {
        Vector3 inputDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) inputDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) inputDirection += Vector3.back;
        if (Input.GetKey(KeyCode.A)) inputDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D)) inputDirection += Vector3.right;

        // adjust character position based on input
        if (inputDirection != Vector3.zero)
        {
            rb.MovePosition(rb.position + inputDirection * Time.deltaTime);
        }
    }

    private void UpdateThrowDirection()
    {
        // update throw direction based on WASD keys
        Vector3 inputDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) inputDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) inputDirection += Vector3.back;
        if (Input.GetKey(KeyCode.A)) inputDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D)) inputDirection += Vector3.right;

        if (inputDirection != Vector3.zero)
        {
            throwDirectionIndicator.rotation = Quaternion.LookRotation(inputDirection);
        }
    }

    void UpdateThrowIndicator()
    {
        if (lineRenderer != null)
        {
            Vector3 forwardDirection = throwDirectionIndicator.forward;
            Vector3 rightAxis = Vector3.Cross(Vector3.up, forwardDirection).normalized;
            Quaternion angleRotation = Quaternion.AngleAxis(currentThrowAngle, rightAxis);
            Vector3 adjustedDirection = angleRotation * forwardDirection;

            Vector3[] trajectoryPoints = CalculateTrajectory(transform.position, adjustedDirection, currentThrowForce);
            lineRenderer.positionCount = trajectoryPoints.Length;
            lineRenderer.SetPositions(trajectoryPoints);
            lineRenderer.startColor = Color.grey;
            lineRenderer.endColor = Color.grey;
        }
    }

    // PROJECTILE MOTION CALCULATION
    Vector3[] CalculateTrajectory(Vector3 startPosition, Vector3 direction, float force)
    {
        Vector3[] trajectoryPoints = new Vector3[trajectoryResolution];
        float timeStep = 0.1f;  // time step for each point
        float velocity = force / rb.mass; // assuming mass is part of the throw force

        for (int i = 0; i < trajectoryResolution; i++)
        {
            float t = i * timeStep;
            Vector3 point = startPosition + direction * velocity * t + 0.5f * Physics.gravity * t * t;
            trajectoryPoints[i] = new Vector3(point.x, point.y, point.z);
        }

        return trajectoryPoints;
    }

    void ThrowObject()
    {
        Vector3 throwPosition = transform.position;
        GameObject throwableObject = Instantiate(throwablePrefab, throwPosition, Quaternion.identity);
        Rigidbody rb = throwableObject.GetComponent<Rigidbody>();

        Vector3 forwardDirection = throwDirectionIndicator.forward;
        Vector3 rightAxis = Vector3.Cross(Vector3.up, forwardDirection).normalized;
        Quaternion angleRotation = Quaternion.AngleAxis(currentThrowAngle, rightAxis);
        Vector3 adjustedDirection = angleRotation * forwardDirection;

        rb.AddForce(adjustedDirection.normalized * currentThrowForce, ForceMode.Impulse);
    }
}
