using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Pressure plate for bridge
public class BridgePressurePlate : MonoBehaviour
{
    [SerializeField] GameObject bridge; // Reference to the bridge GameObject
    [SerializeField] bool switchOn = false;
    private Renderer _renderer;
    public Material switchOnMaterial;
    public float pressedAmount = 2; // How far the pressure plate sinks
    private Material defaultMaterial;
    private int numOnPlate;
    private Vector3 startingPos;
    private GameObject visualPlate;

    // Start is called before the first frame update
    void Start()
    {
        // Right now pressure plate's renderer is in child component
        visualPlate = transform.parent.gameObject;
        _renderer = transform.parent.GetChild(0).gameObject.GetComponent<Renderer>();
        defaultMaterial = _renderer.material;
        startingPos = transform.position;
        UpdateBridgeState();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("rat") || other.gameObject.CompareTag("Player"))
        {
            numOnPlate += 1;
            UpdatePressurePlate();
            UpdateBridgeState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("rat") || other.gameObject.CompareTag("Player"))
        {
            numOnPlate -= 1;
            UpdatePressurePlate();
            UpdateBridgeState();
        }
    }

    private void UpdateBridgeState()
    {
        if (bridge is not null)
        {
            if (switchOn)
            {
                GameEvents.current.lowerBridge?.Invoke(bridge.GetInstanceID());
            }
            else
            {
                GameEvents.current.stopBridge?.Invoke(bridge.GetInstanceID());
            }
        }
    }
    
    private void UpdatePressurePlate()
    {
        if (numOnPlate >= 1)
        {
            _renderer.material = switchOnMaterial;
            
            // Move pressure plate down
            if (transform.position == startingPos)
            {
                Vector3 updatedPos = gameObject.transform.position;
                updatedPos.y -= pressedAmount;
                visualPlate.transform.position = updatedPos;
            }
            switchOn = true;
        }
        else
        {
            _renderer.material = defaultMaterial;
            visualPlate.transform.position = startingPos;
            switchOn = false;
        }
    }
}