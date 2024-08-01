using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lever for door
public class DoorLever : MonoBehaviour
{
    [SerializeField] GameObject visualLever;
    [SerializeField] GameObject door; // Reference to the door GameObject
    [SerializeField] bool switchOn;
    [SerializeField] float yRot;
    float rotationRange = 20f;
    float rotation;
    float speed = 5;
    float hitCooldownMax = 0.25f;
    [SerializeField] float hitCooldown;
    
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
        UpdateDoorState(); // Ensure bridge state is correct at start
    }

    // Update is called once per frame
    void Update()
    {
        hitCooldown += Time.deltaTime;
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
        if (hitCooldown > hitCooldownMax)
        {
            if (other.gameObject.CompareTag("rat") || other.gameObject.CompareTag("Player"))
            {
                switchOn = !switchOn;
                UpdateDoorState();
                hitCooldown = 0;
            }

        } else
        {
            Debug.Log("Switch Cooldown too Low: " + hitCooldown + " / " + hitCooldownMax); 
        }
    }

    private void UpdateDoorState()
    {
        if (door != null)
        {
            door.SetActive(!switchOn);
        }
    }
}
