using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lever for door
public class Lever : MonoBehaviour
{
    [SerializeField] GameObject visualLever;
    [SerializeField] GameObject switchObject; 
    [SerializeField] bool switchOn;
    [SerializeField] float yRot;
    float rotationRange = 20f;
    float rotation;
    float speed = 5;
    float hitCooldownMax = 0.25f;
    [SerializeField] float hitCooldown;
    private bool isActivated = false; // Track if the lever has been activated

    // Start is called before the first frame update
    void Start()
    {
        yRot = transform.eulerAngles.y;
        if (switchOn)
        {
            transform.localEulerAngles = new Vector3(rotationRange, yRot, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(-rotationRange, yRot, 0);
        }

        switchOn = false;
        UpdateState(); // Ensure object state is correct at start
    }

    // Update is called once per frame
    void Update()
    {
        hitCooldown += Time.deltaTime;
        RotateVisualLever();
    }

    private void RotateVisualLever()
    {
        if (switchOn)
        {
            rotation += (rotationRange - rotation) * speed * Time.deltaTime;
        }
        else
        {
            rotation += (-rotationRange - rotation) * speed * Time.deltaTime;
        }
        transform.localEulerAngles = new Vector3(rotation, yRot, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isActivated)
        {
            return; // If the lever is already activated, do nothing
        }

        if (hitCooldown > hitCooldownMax)
        {
            if (other.gameObject.CompareTag("rat") || other.gameObject.CompareTag("Player"))
            {
                switchOn = !switchOn;
                UpdateState();
                hitCooldown = 0;
                isActivated = true; // Mark the lever as activated
            }
        } 
        else
        {
            Debug.Log("Switch Cooldown too Low: " + hitCooldown + " / " + hitCooldownMax); 
        }
    }

    private void UpdateState()
    {
        if (switchObject != null)
        {
            if (switchObject.CompareTag("Bridge"))
            {
                if (switchOn)
                {
                    GameEvents.current.lowerBridge?.Invoke(switchObject.GetInstanceID());
                }
            }

            if (switchObject.CompareTag("Door"))
            {
                switchObject.SetActive(!switchOn);   
            }
        }
    }
}
