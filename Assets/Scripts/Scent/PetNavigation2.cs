using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using JetBrains.Annotations;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class PetNavigation2 : MonoBehaviour
{
    private Vector3 spawnLocation;
    private NavMeshAgent agent;
    private Dictionary<string, int> agentTypes;
    private NavMeshPath path;
    public NavMeshSurface noLinksNavMesh; // NavMeshSurface with no jumping
    private NavMeshQueryFilter navMeshQueryFilter;
    private GameObject player;
    private InventoryManager inventory;
    public float followCoolDown = 0.5f; // how many sec between following
    private float timer;
    private bool isMoving; // Keeps track of whether the pet is moving
    public Animator myAnim;
    private bool followingScent = false; // checks if currently following a scent
    public float stoppingDistance;
    private UniqueQueue<GameObject> scentLocations;
    public float followDistance; // the distance the pet starts following the player
    

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.itemEaten += OnEaten;
        GameEvents.current.changeAbility += OnChangeAbility;
        GameEvents.current.setPetSpawn += OnSetSpawnPoint;
        GameEvents.current.resetPetPos += OnResetPetPos;
        
        inventory = FindObjectOfType<InventoryManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        
        // Get agent types
        agentTypes = new Dictionary<string, int>();
        int count = UnityEngine.AI.NavMesh.GetSettingsCount();
        string[] agentTypeNames = new string[count + 2];
        for (var i = 0; i < count; i++)
        {
            int id = UnityEngine.AI.NavMesh.GetSettingsByIndex(i).agentTypeID;
            string name = UnityEngine.AI.NavMesh.GetSettingsNameFromID(id);
            agentTypes[name] = id;
        }
        
        // Used to check if pet and player are on same plaform. Queries for
        // a navMeshAgent and surface where jumping is impossible
        path = new NavMeshPath();
        navMeshQueryFilter = new NavMeshQueryFilter();
        navMeshQueryFilter.agentTypeID = noLinksNavMesh.agentTypeID;
        navMeshQueryFilter.areaMask = noLinksNavMesh.layerMask;
        
        // Set starting spawn location
        spawnLocation = gameObject.transform.position;
        scentLocations = new UniqueQueue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowScent();
        // Timer so that the pet isn't following the player every frame
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = followCoolDown;
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        myAnim.SetBool("IsMoving",isMoving);
        // This checks if path (full or partial) is possible between the player and pet
        // without jumping
        bool pathPossible = NavMesh.CalculatePath(transform.position,
            player.transform.position, navMeshQueryFilter, path);
        float playerDistance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        
        // Check if player and pet are on the same platform
        if (pathPossible & path.status == NavMeshPathStatus.PathComplete)
        {
            // Check distance between player and pet
            if (playerDistance <= followDistance)
            {
                // Check pet is not colliding with a particle and player has cheese in inventory
                if (!followingScent && inventory.isHoldingCheese)
                {
                    agent.stoppingDistance = stoppingDistance;
                    agent.destination = player.transform.position;
                }   
            }
        }
        // Update animations if moving
        if (agent.velocity != new Vector3(0, 0, 0))
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        
        if (agent.velocity.x > 0)
        {
            myAnim.SetBool("Flipped",true);
        }
        else if(agent.velocity.x < 0)
        {
            myAnim.SetBool("Flipped",false);
        }

    }
    
    private void OnChangeAbility(string agentType)
    {
        int agentID;
        if (agentTypes.TryGetValue(agentType, out agentID))
        {
            if (agent.agentTypeID != agentID)
            {
                agent.agentTypeID = agentID;
            }
        }
    }
    
    

    private void FollowScent()
    {
        if (scentLocations.Count != 0)
        {
            agent.stoppingDistance = 0f;
            GameObject scentItem = scentLocations.Dequeue();
            if (scentItem??false)
            {
                Vector3 scentLocation = scentItem.transform.position;
                agent.SetDestination(scentLocation);
            
                // Change the ability of the dog based on the scent it's following
                if (scentItem.CompareTag("SpicyBiscuit"))
                {
                    OnChangeAbility("JumpAgent");
                }
                followingScent = true;
            }
        }
    }
    
    private void OnEaten()
    {
        followingScent = false;
    }

    private void OnSetSpawnPoint(Vector3 position)
    {
        spawnLocation = position;
    }
    
    private void OnResetPetPos()
    {
        agent.ResetPath();
        gameObject.SetActive(false);
        gameObject.transform.position = spawnLocation;
        gameObject.SetActive(true);
        followingScent = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scent") || other.CompareTag("Particle"))
        {
            scentLocations.Enqueue(other.transform.parent.gameObject);
        }
    }
}
