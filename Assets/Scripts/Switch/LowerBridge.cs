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
    
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.lowerBridge += OnLowerBridge;
        GameEvents.current.stopBridge += OnStopRotation;
        _layerMask |= (1 << 0);
        pivot = gameObject.transform.GetChild(0);
        endpoint = gameObject.transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (_bridgeRot)
        {
            RotateBridge();
        }
    }

    private void OnLowerBridge()
    {
        _bridgeRot = true;
    }
    
    private void OnStopRotation()
    {
        _bridgeRot = false;
    }
    
    private void RotateBridge()
    {
        // Debug.DrawLine(endpoint.position, endpoint.position + Vector3.down * 1f, Color.red);
        RaycastHit hit;
        Physics.Raycast(endpoint.position, Vector3.down, out hit, 0.2f, _layerMask);
        if (hit.collider is not null)
        {
            _bridgeRot = false;
        }
        else
        {
            gameObject.transform.RotateAround(pivot.position, transform.forward, degreePerSec * Time.deltaTime);
        }
    }
}
