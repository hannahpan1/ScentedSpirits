using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LowerBridge : MonoBehaviour
{
    // Bridge specific attributes
    [SerializeField] int degreePerSec; // Degree the bridge lowers per second, increasing will lead to faster decrease
    private LayerMask _layerMask;
    private bool _bridgeRot = false;
    [SerializeField] Transform pivot;
    [SerializeField] private Transform endpoint;
    private Quaternion _startingRot;
    private bool _loweredFully = false;
    
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.lowerBridge += OnLowerBridge;
        GameEvents.current.raiseBridge += OnRaiseRotation;
        _layerMask |= (1 << 0);
        pivot = gameObject.transform.GetChild(0);
        endpoint = gameObject.transform.GetChild(1);
        _startingRot = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (_bridgeRot && !_loweredFully)
        {
            RotateBridge();
        }
        else
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
        // Debug.DrawLine(endpoint.position, endpoint.position + Vector3.down * 1f, Color.red);
        RaycastHit hit;
        Physics.Raycast(endpoint.position, Vector3.down, out hit, 0.2f, _layerMask);
        if (hit.collider is not null)
        {
            _loweredFully = true;
        }
        else
        {
            gameObject.transform.RotateAround(pivot.position, transform.forward, degreePerSec * Time.deltaTime);
        }
    }
    
    private void RaiseBridge()
    {
        if (transform.rotation != _startingRot)
        {
            gameObject.transform.RotateAround(pivot.position, transform.forward * -1, degreePerSec * Time.deltaTime);
            _loweredFully = false;
        }
    }
}
