using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class LowerBridge : MonoBehaviour
{
    [SerializeField] private int degreePerSec; // Degree the bridge lowers per second
    private LayerMask _layerMask;
    private bool _bridgeRot = false;
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform endpoint;
    private Quaternion _startingRot;
    private bool _loweredFully = false;
    [CanBeNull] public GameObject barrier;
    [CanBeNull] public GameObject barrier2;

    // Tolerance for checking rotation stability
    public const float RotationTolerance = 0.1f;
    // Distance for raycasting to detect the ground
    public const float RaycastDistance = 0.2f;

    void Start()
    {
        GameEvents.current.lowerBridge += OnLowerBridge;
        GameEvents.current.raiseBridge += OnRaiseRotation;
        _layerMask = LayerMask.GetMask("Default"); // Adjust layer as needed
        if (pivot == null) pivot = transform.GetChild(0);
        if (endpoint == null) endpoint = transform.GetChild(1);
        _startingRot = transform.rotation;
    }

    void Update()
    {
        if (_bridgeRot && !_loweredFully)
        {
            RotateBridge();
        }
        else if (!_bridgeRot)
        {
            RaiseBridge();
        }
    }

    private void OnLowerBridge(int id)
    {
        if (gameObject.GetInstanceID() == id)
        {
            _bridgeRot = true;
        }
    }

    private void OnRaiseRotation(int id)
    {
        if (gameObject.GetInstanceID() == id)
        {
            _bridgeRot = false;
        }
    }

    private void RotateBridge()
    {
        if (Physics.Raycast(endpoint.position, Vector3.down, out RaycastHit hit, RaycastDistance, _layerMask))
        {
            _loweredFully = true;
            if (barrier != null) barrier.SetActive(false);
            if (barrier2 != null) barrier2.SetActive(false);
        }
        else
        {
            transform.RotateAround(pivot.position, transform.forward, degreePerSec * Time.deltaTime);
        }
    }

    private void RaiseBridge()
    {
        if (!IsRotationCloseTo(_startingRot, RotationTolerance))
        {
            transform.RotateAround(pivot.position, -transform.forward, degreePerSec * Time.deltaTime);
        }
        else
        {
            _loweredFully = false;
        }
        if (barrier != null) barrier.SetActive(true);
        if (barrier2 != null) barrier2.SetActive(true);
    }

    private bool IsRotationCloseTo(Quaternion targetRotation, float tolerance)
    {
        return Quaternion.Angle(transform.rotation, targetRotation) < tolerance;
    }
}
