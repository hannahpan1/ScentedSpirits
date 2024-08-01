using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Pressure plate for bridge
public class BridgeController : MonoBehaviour
{
    [SerializeField] GameObject bridge; // Reference to the bridge GameObject
    [SerializeField] bool switchOn = false;
    private Renderer _renderer;
    public Color switchOnColor;
    private Color defaultColor;
    private int numOnPlate;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
        defaultColor = _renderer.material.color;
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
            bridge.SetActive(switchOn);
        }
    }
    
    private void UpdatePressurePlate()
    {
        if (numOnPlate >= 1)
        {
            _renderer.material.SetColor("_Color", switchOnColor);
            switchOn = true;
        }
        else
        {
            _renderer.material.SetColor("_Color", defaultColor);
            switchOn = false;
        }
    }
}