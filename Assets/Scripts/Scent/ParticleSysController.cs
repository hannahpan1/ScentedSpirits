using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleSysController : MonoBehaviour
{
    private GameObject foodItem;
    private ParticleSystem ps;
    private bool scentFound = false;
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        BoxCollider ratCollider =
            GameObject.FindGameObjectWithTag("rat").GetComponent<BoxCollider>();
        if (ratCollider is not null)
        {
            ps.trigger.SetCollider(0, ratCollider);
        }

        foodItem = gameObject.transform.parent.gameObject;
    }

    private void OnParticleTrigger()
    {
        // Needs improvement: Current design is that the pet's ability is only turned back
        // to humanoid when the food is eaten not when it leaves the scent cloud.
        // scentFound is for performance and queue management (so that follow scent
        // is invoked just once and not with every collision)
        // if (!scentFound)
        // {
            // Invoke delegate to change ability
            if (foodItem.CompareTag("SpicyBiscuit"))
            {
                GameEvents.current.changeAbility?.Invoke("JumpAgent");   
            }
        
            // Check if food has dropped to the ground
            // if (Mathf.Approximately(foodItem.GetComponent<Rigidbody>().velocity.y, 0) 
            //     & !scentFound) 
            if (Mathf.Approximately(foodItem.GetComponent<Rigidbody>().velocity.y, 0))
            {
                // Invoke delegate to follow the scent
                GameEvents.current.followScent?.Invoke(gameObject.transform.position);
                // scentFound = true;
            }
        // }
    }
}
