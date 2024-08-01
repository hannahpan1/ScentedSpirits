using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParticleSys2 : MonoBehaviour
{
    public GameObject particle;
    public int particlesEmitted = 4; // how many particles are emitted
    public float particleVelocity = 1; // starting velocity of particles
    public float emissionInterval = 2; // how many secs before emitting new particles
    public float radius;
    private LayerMask layerMask;
    private List<(GameObject, Rigidbody)> particleObjects;
    private GameObject foodItem;
    private float timer = 0;
    private float timer2 = 0;
    private bool onGround = false;
    private Rigidbody foodRb;
    private bool stopped = false;
    private bool AoE = false;
    
    // Start is called before the first frame update
    void Start()
    { 
        particleObjects = new List<(GameObject, Rigidbody)>();
        layerMask &= ~(1 << 3);
        layerMask |= (1 << 0);
        foodRb = gameObject.GetComponent<Rigidbody>();
        GameEvents.current.fanTriggered += IncreaseRadius;
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn a bunch of particles 
        if (!onGround)
        {
            GroundCol();
        } 
        else
        {
            CheckMoving();
            if (stopped)
            {
                if (!AoE)
                {
                    // Get AoE sprite and show
                    GameObject scentRadius = gameObject.transform.GetChild(0).gameObject;
                    scentRadius.SetActive(true);
                    AoE = true;
                }
                
                // Spawn particles
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    timer = emissionInterval;
                    SpawnParticles();
                }
            
                // Get rid of particles outside of the radius
                CheckRadius();
            }
        }
    }
    
    private void GroundCol()
    {
        RaycastHit hit;
        Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 1f, layerMask);
        if (hit.collider != null)
        {
            onGround = true;
        }
    }
    
    private void CheckMoving()
    {
        if (foodRb.velocity.magnitude < 0.5)
        {
            stopped = true;
        }
    }

    private void SpawnParticles()
    {
        for (var i = 0; i < particlesEmitted; i++)
        {
            // Create a random particle starting at the food location
            GameObject p = Instantiate(particle, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            Rigidbody rb = p.GetComponent<Rigidbody>();
            Vector3 rand = Random.onUnitSphere;
            rand.y = Mathf.Abs(rand.y);
            rb.velocity = rand * particleVelocity;
            particleObjects.Add((p, rb));
        }
    }
    
    private void CheckRadius()
    {
        foreach ((GameObject, Rigidbody) p in particleObjects.ToList())
        {
            if (Vector3.Distance(p.Item1.transform.position, gameObject.transform.position) > radius)
            {
                particleObjects.Remove(p);
                Destroy(p.Item1);
            }
        }
    }

    void IncreaseRadius(float newRadius)
    {
        radius = newRadius;
    }
}
