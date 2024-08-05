using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Fans : MonoBehaviour
{
    public float airVelocity; // How fast the particles
    public Vector3 RegularBiscuitAoeScale;
    public Vector3 SpicyBiscuitAoeScale;
    private float _fanLength;
    private int fanID;
    
    // Start is called before the first frame update
    void Start()
    {
        _fanLength = gameObject.GetComponent<BoxCollider>().size.z;
        fanID = transform.parent.gameObject.GetInstanceID();
    }

    void OnTriggerEnter(Collider other)
    {
        // Turn off AoE effect when colliding with fan
        if (other.CompareTag("cheese"))
        {
            GameObject obj = other.gameObject;
            if (obj.transform.parent != null)
            {
                // Because other colliders like eatingRadius is tagged cheese we need to go to parent
                // to get the AoE object
                obj.transform.parent.GetChild(0).localScale = RegularBiscuitAoeScale;
            }
            else
            {
                obj.transform.GetChild(0).localScale = RegularBiscuitAoeScale;
            }
            
            // Change radius to box collider's length
            GameEvents.current.fanTriggered(_fanLength);
        }
        
        if (other.CompareTag("SpicyBiscuit"))
        {
            GameObject obj = other.gameObject;
            if (obj.transform.parent != null)
            {
                // Because other colliders like eatingRadius is tagged cheese we need to go to parent
                // to get the AoE object
                obj.transform.parent.GetChild(0).localScale = SpicyBiscuitAoeScale;
            }
            else
            {
                obj.transform.GetChild(0).localScale = SpicyBiscuitAoeScale;
            }
            
            // Change radius to box collider's length
            GameEvents.current.fanTriggered(_fanLength);
        }

        if (other.CompareTag("Particle"))
        {
            GameEvents.current.spinPropeller?.Invoke(fanID);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("cheese") || other.CompareTag("SpicyBiscuit"))
        {
            GameEvents.current.stopPropeller?.Invoke(fanID);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Particle"))
        {
            // distance away from fan
            float distance = Vector3.Distance(other.transform.position, gameObject.transform.position);
            Rigidbody rb = other.GetComponent<Rigidbody>();
            Vector3 particleVelocity = rb.velocity;
            rb.velocity = gameObject.transform.forward * (airVelocity / distance) + particleVelocity;
        }
    }
}
